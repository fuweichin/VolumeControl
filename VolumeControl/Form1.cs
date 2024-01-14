using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace VolumeControl
{
    public partial class Form1 : Form
    {
        string appName = "VolumeControl";
        string appRegKeyPath = @"SOFTWARE\NaCl\VolumeControl";
        string runRegKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        Icon appIcon;
        string colorTheme = "light";
        string executablePath;
        string settingsPath;
        DeviceSettings settings;
        List<DeviceInfo> outputDevices;
        DeviceInfo selectedDeviceInfo; // in-app selected
        DeviceInfo defaultDeviceInfo; // system default
        MMDeviceEnumerator devEnu;
        volatile bool initVisible = true;
        volatile bool willExit = false;

        public Form1()
        {
            InitializeComponent();
            // (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);
        }

        public Form1(bool initVisible) : this()
        {
            this.initVisible = initVisible;
            if (!initVisible)
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        // Detect dark mode
        // https://stackoverflow.com/a/68845708/2189544
        private static bool AppsUseLightTheme()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if(regKey==null)
                {
                    return true;
                }
                return ((int)regKey.GetValue("AppsUseLightTheme", 0)) > 0;
            }
        }

        // Set dark titlebar
        // https://stackoverflow.com/a/62811758/2189544
        // https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public enum DWMWA
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        }

        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            int windowsBuild = Environment.OSVersion.Version.Build;
            if (windowsBuild >= 17763)
            {
                int attribute = windowsBuild >= 18985 ? (int)DWMWA.DWMWA_USE_IMMERSIVE_DARK_MODE :
                    (int)DWMWA.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }
            return false;
        }

        public enum DwmWindowAttribute
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        }

        // register global hot key
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        private void applyColorTheme(string theme)
        {
            colorTheme = theme;
            if (theme.Equals("light"))
            {
                UseImmersiveDarkMode(Handle, false);
                SuspendLayout();
                this.BackColor = SystemColors.Window;
                this.ForeColor = SystemColors.WindowText;
                btnSave.BackColor = SystemColors.Control;
                ResumeLayout();
            }
            else if (theme.Equals("dark"))
            {
                UseImmersiveDarkMode(Handle, true);
                SuspendLayout();
                this.BackColor = Color.FromArgb(0x20, 0x21, 0x24);
                this.ForeColor = Color.FromArgb(0xF8, 0xF9, 0xFA);
                btnSave.BackColor = Color.FromArgb(0x54, 0x54, 0x54);
                ResumeLayout();
            }
        }

        private DeviceSettings loadSettings(string path)
        {
            DeviceSettings settings = JsonConvert.DeserializeObject<DeviceSettings>(File.ReadAllText(path));
            if (settings == null)
            {
                throw new Exception("Failed to load settings");
            }
            if (settings.Version != 1)
            {
                throw new Exception("Unexpected settings version " + settings.Version);
            }
            if (settings.OutputDevices == null)
            {
                settings.OutputDevices = new List<DeviceInfo>();
            }
            return settings;
        }
        private void storeSettings(DeviceSettings settings, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(settings, Formatting.Indented), Encoding.UTF8);
        }

        public void saveSettingsIfNecessary()
        {
            foreach (DeviceInfo d in outputDevices)
            {
                if (d.IsVolatile && d.IsNotDefault())
                {
                    settings.OutputDevices.Add(d);
                    d.IsVolatile = false;
                }
            }
            storeSettings(settings, settingsPath);
        }

        private IntPtr hotKey_Pressed(Message m)
        {
            if (defaultDeviceInfo.VolStepEnabled)
            {
                DeviceInfo info = defaultDeviceInfo;
                AudioEndpointVolume volume = info.Handler.MMDevice.AudioEndpointVolume;
                float maxVol = info.MaxVolEnabled ? info.MaxVol : 100;
                float currVol = (float)Math.Round(volume.MasterVolumeLevelScalar * 100, 2);
                bool currMute = volume.Mute;
                float newVol;

                int id = m.WParam.ToInt32();
                Keys key = (Keys)id;
                if (key == Keys.VolumeDown)
                {
                    if (currVol == 0 && !currMute && info.DontAutoMute)
                    {
                        // mute directly
                        VolumeState ignoreState = new VolumeState(currVol, true, info.Handler.Stopwatch.ElapsedMilliseconds);
                        info.Handler.SetIgnoreState(ignoreState);
                        volume.Mute = true;
                        return IntPtr.Zero;
                    }
                    newVol = Math.Max(currVol - defaultDeviceInfo.VolStep, 0);
                    if (newVol < defaultDeviceInfo.VolStep)
                    {
                        newVol = 0;
                    }
                }
                else //if (key == Keys.VolumeUp)
                {
                    newVol = Math.Min(currVol + defaultDeviceInfo.VolStep, maxVol);
                }
                if (newVol != currVol)
                {
                    volume.MasterVolumeLevelScalar = newVol / 100;
                    if (key == Keys.VolumeDown)
                    {
                        if (newVol == 0 && !info.DontAutoMute)
                        {
                            // auto mute passingly
                            volume.Mute = true;
                        }
                    }
                    else //if (key == Keys.VolumeUp)
                    {
                        if (currVol == 0 && currMute)
                        {
                            // auto unmute passingly
                            volume.Mute = false;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            executablePath = Assembly.GetExecutingAssembly().Location;
            string dirPath = Path.GetDirectoryName(executablePath);

            // load icons
            Tuple<string, int> iconPath = IconExtractor.ParsePath(@"%windir%\system32\ddores.dll,1");
            appIcon = IconExtractor.Extract(iconPath.Item1, iconPath.Item2, true);

            // load settings from json
            settingsPath = Path.Combine(dirPath, "settings.json");
            if (File.Exists(settingsPath))
            {
                try
                {
                    settings = loadSettings(settingsPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.Message);
                    Application.Exit();
                    return;
                }
            }
            else
            {
                settings = new DeviceSettings();
                settings.OutputDevices = new List<DeviceInfo>();
            }
            Dictionary<string, DeviceInfo> savedDevices = settings.OutputDevices.ToDictionary((DeviceInfo d) => d.Id);
            
            // load settings from regsitry
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(runRegKeyPath))
            {
                string value = (string)regKey.GetValue(appName, "");
                settings.AutoRunOnStartup = value.Length > 0;
            }
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(appRegKeyPath))
            {
                if (regKey == null)
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(appRegKeyPath, true))
                    {
                        settings.AutoSaveOnShutdown = false;
                        settings.AutoUseColorTheme = false;
                        settings.AlwaysOnTop = false;
                        key.SetValue("AutoSaveOnShutdown", settings.AutoSaveOnShutdown ? 1 : 0, RegistryValueKind.DWord);
                        key.SetValue("AutoUseColorTheme", settings.AutoUseColorTheme ? 1 : 0, RegistryValueKind.DWord);
                        key.SetValue("AlwaysOnTop", settings.AlwaysOnTop ? 1 : 0, RegistryValueKind.DWord);
                        
                    }
                }
                else
                {
                    settings.AutoSaveOnShutdown = (int)regKey.GetValue("AutoSaveOnShutdown", 0) != 0;
                    settings.AutoUseColorTheme = (int)regKey.GetValue("AutoUseColorTheme", 0) != 0;
                    settings.AlwaysOnTop = (int)regKey.GetValue("AlwaysOnTop", 0) != 0;
                }
            }

            // enumerate devices
            devEnu = new MMDeviceEnumerator();
            outputDevices = new List<DeviceInfo>();
            foreach (MMDevice dev in devEnu.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                DeviceInfo info;
                string id = dev.ID;
                if (savedDevices.ContainsKey(id))
                {
                    info = savedDevices[id];
                    info.Name = dev.FriendlyName;
                    outputDevices.Add(info);
                    DeviceHandler handler = new DeviceHandler(dev, info);
                    info.Handler = handler;
                    if (info.MaxVolEnabled)
                    {
                        handler.Reset();
                        dev.AudioEndpointVolume.OnVolumeNotification += handler.AudioEndpointVolume_OnVolumeNotification;
                    }
                }
                else
                {
                    info = new DeviceInfo();
                    info.Id = dev.ID;
                    info.Name = dev.FriendlyName;
                    info.IsVolatile = true;
                    outputDevices.Add(info);
                    DeviceHandler handler = new DeviceHandler(dev, info);
                    info.Handler = handler;
                }
                dev.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                Tuple<string, int> iconInfo = IconExtractor.ParsePath(dev.IconPath);
                info.Icon = IconExtractor.Extract(iconInfo.Item1, iconInfo.Item2, true);
            }
            MMDevice mmDevice = devEnu.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            // bind data source
            SuspendLayout();
            List<StepInfo> stepItems = new List<StepInfo>
            {
                new StepInfo(0.25f),
                new StepInfo(0.5f),
                new StepInfo(1),
                new StepInfo(2),
                new StepInfo(4),
                new StepInfo(8)
            };
            cmbVolStep.DisplayMember = "Step";
            cmbVolStep.ValueMember = "Step";
            cmbVolStep.DataSource = stepItems;
            cmbVolStep.SelectedIndex = 2;

            cmbDevices.DisplayMember = "Name";
            cmbDevices.ValueMember = "Id";
            cmbDevices.DataSource = outputDevices;
            if (mmDevice != null)
            {
                int index = outputDevices.FindIndex((DeviceInfo dev) => dev.Id == mmDevice.ID);
                if (cmbDevices.SelectedIndex != index)
                {
                    cmbDevices.SelectedIndex = index;
                }
                defaultDeviceInfo = outputDevices[index];
            }
            else if (outputDevices.Count > 0)
            {
                cmbDevices.SelectedIndex = 0;
            }
            cmbDevices_SelectedIndexChanged(null, EventArgs.Empty);
            if (settings.AutoRunOnStartup)
                tsmiAutoRunOnStartup.Checked = true;
            if (settings.AlwaysOnTop)
            {
                tsmiAlwaysOnTop.Checked = this.TopMost = true;
            }
            if (settings.AutoUseColorTheme)
            {
                applyColorTheme(!AppsUseLightTheme() ? "dark" : "light");
                tsmiAutoUseColorTheme.Checked = true;
            }
            notifyIcon1.Text = appName;
            this.Icon = notifyIcon1.Icon = appIcon;
            ResumeLayout(true);

            // bind events
            cmbDevices.SelectedIndexChanged += cmbDevices_SelectedIndexChanged;
            devEnu.RegisterEndpointNotificationCallback(new MMNotificationClient(new DefaultDeviceChangedCallBack(DefaultDeviceChanged)));
            Application.ApplicationExit += Application_ApplicationExit;

            this.Click += Form1_Click;
            groupBox1.Click += Form1_Click;
            groupBox2.Click += Form1_Click;

            trackCurrVol.MouseWheel += TrackCurrVol_MouseWheel;
        }

        private void TrackCurrVol_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Wheel {0}", e.Delta);
        }

        /// <summary>
        /// Register hot keys
        /// </summary>
        public const int WM_QUERYENDSESSION = 0x11;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_SYSCOMMAND = 0x112;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {

                m.Result = hotKey_Pressed(m);
                return;
            }
            else if (m.Msg == TrayUtils.WM_SHOWME)
            {
                ShowAndActivate();
                return;
            }
            else if (m.Msg == WM_QUERYENDSESSION)
            {
                if (settings.AutoSaveOnShutdown)
                {
                    saveSettingsIfNecessary();
                }
            }
            base.WndProc(ref m);
        }

        bool hotkeysRegistered = false;
        private void RegisterHotKeys(bool register)
        {
            if (register)
            {
                if (hotkeysRegistered)
                    return;
                List<Keys> registering = new List<Keys>
                {
                    Keys.VolumeDown,
                    Keys.VolumeUp
                };
                List<Keys> registered = new List<Keys>();
                foreach(Keys key in registering)
                {
                    if (!RegisterHotKey(this.Handle, (int)key, KeyModifiers.None, key))
                    {
                        Marshal.GetLastWin32Error();
                        foreach (Keys k in registered)
                        {
                            UnregisterHotKey(this.Handle, (int)k);
                        }
                        throw new Exception("Failed to register hot key " + key.ToString());
                    }
                    registered.Add(key);
                }
                hotkeysRegistered = true;
            }
            else
            {
                if (!hotkeysRegistered)
                    return;
                UnregisterHotKey(this.Handle, (int)Keys.VolumeDown);
                UnregisterHotKey(this.Handle, (int)Keys.VolumeUp);
                hotkeysRegistered = false;
            }
        }

        private void ShowAndActivate()
        {
            if (!Visible)
            {
                Show();
            }
            if (WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            Activate();
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            if (selectedDeviceInfo != defaultDeviceInfo)
            {
                return;
            }
            trackCurrVol.Invoke(new MethodInvoker(delegate ()
            {
                SuspendLayout();
                trackCurrVol.Value = Convert.ToInt32(data.MasterVolume * 100);
                lblCurrVol.Text = "" + Math.Round(data.MasterVolume * 100, 2);
                lblCurrVolLevel.Text = data.Muted ? "-∞dB (muted)" : selectedDeviceInfo.Handler.MMDevice.AudioEndpointVolume.MasterVolumeLevel + "dB";
                ResumeLayout(true);
            }));
        }

        public void DefaultDeviceChanged(string defaultDeviceId)
        {
            int index = outputDevices.FindIndex(d => d.Id == defaultDeviceId);
            if (index == -1)
            {
                MMDevice dev = devEnu.GetDevice(defaultDeviceId);
                DeviceInfo info = new DeviceInfo();
                info.Id = dev.ID;
                info.Name = dev.FriendlyName;
                info.IsVolatile = true;
                info.Handler = new DeviceHandler(dev, info);
                outputDevices.Add(info);
                defaultDeviceInfo = info;
            }
            else
            {
                defaultDeviceInfo = outputDevices[index];
            }
        }

        private void toggleEventListeners(bool flag)
        {
            if (flag)
            {
                chkEnableMaxVol.CheckedChanged += chkEnableMaxVol_CheckedChanged;
                numMaxVol.ValueChanged += numMaxVol_ValueChanged;
                chkEnableVolStep.CheckedChanged += chkEnableVolStep_CheckedChanged;
                cmbVolStep.SelectedIndexChanged += cmbVolStep_SelectedIndexChanged;
                chkDontAutoMute.CheckedChanged += chkDontAutoMute_CheckedChanged;
            }
            else
            {
                chkEnableMaxVol.CheckedChanged -= chkEnableMaxVol_CheckedChanged;
                numMaxVol.ValueChanged -= numMaxVol_ValueChanged;
                chkEnableVolStep.CheckedChanged -= chkEnableVolStep_CheckedChanged;
                cmbVolStep.SelectedIndexChanged -= cmbVolStep_SelectedIndexChanged;
                chkDontAutoMute.CheckedChanged -= chkDontAutoMute_CheckedChanged;
            }
        }

        private void cmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbDevices.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            toggleEventListeners(false);
            DeviceInfo info = outputDevices[index];
            float masterVol = info.Handler.MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
            if (sender != null)
                SuspendLayout();
            chkEnableMaxVol.Checked = info.MaxVolEnabled;
            numMaxVol.Value = info.MaxVol;
            numMaxVol.Enabled = info.MaxVolEnabled;
            chkEnableVolStep.Checked = info.VolStepEnabled;
            cmbVolStep.SelectedValue = info.VolStep;
            cmbVolStep.Enabled = info.VolStepEnabled;
            chkDontAutoMute.Checked = info.DontAutoMute;
            trackCurrVol.Value = Convert.ToInt32(masterVol * 100);
            lblCurrVol.Text = "" + Math.Round(masterVol * 100, 2);

            AudioEndpointVolume volume = info.Handler.MMDevice.AudioEndpointVolume;
            AudioEndpointVolumeVolumeRange r = volume.VolumeRange;
            lblDynamicRange.Text = string.Format("{0}dB~{1}dB; increment={2}dB", r.MinDecibels, r.MaxDecibels, r.IncrementDecibels);
            lblCurrVolLevel.Text = volume.Mute ? "-∞dB (muted)" : volume.MasterVolumeLevel + "dB";

            picDeviceIcon.Image = info.Icon?.ToBitmap();
            if (sender != null)
                ResumeLayout(true);
            toggleEventListeners(true);
            this.selectedDeviceInfo = info;
            try
            {
                RegisterHotKeys(info.VolStepEnabled);
            } catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(5000, "Failed to register hot keys VolumeUp/VolumeDown.\n"+ ex.Message, "VolumeControl", ToolTipIcon.Warning);
            }
        }

        private void chkEnableMaxVol_CheckedChanged(object sender, EventArgs e)
        {
            DeviceHandler deviceHandler = selectedDeviceInfo.Handler;
            if (chkEnableMaxVol.Checked)
            {
                deviceHandler.Reset();
                deviceHandler.MMDevice.AudioEndpointVolume.OnVolumeNotification += deviceHandler.AudioEndpointVolume_OnVolumeNotification;
                numMaxVol.Enabled = true;
                deviceHandler.LimitVolumeIfNeeded();
                selectedDeviceInfo.MaxVolEnabled = true;
            }
            else
            {
                deviceHandler.MMDevice.AudioEndpointVolume.OnVolumeNotification -= deviceHandler.AudioEndpointVolume_OnVolumeNotification;
                numMaxVol.Enabled = false;
                selectedDeviceInfo.MaxVolEnabled = false;
            }
        }

        private void numMaxVol_ValueChanged(object sender, EventArgs e)
        {
            selectedDeviceInfo.MaxVol = (int)numMaxVol.Value;
            // Console.WriteLine("MaxVol is changed to {0}", selectedDeviceInfo.MaxVol);
            selectedDeviceInfo.Handler.LimitVolumeIfNeeded();
        }

        private void chkEnableVolStep_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableVolStep.Checked)
            {
                cmbVolStep.Enabled = true;
                selectedDeviceInfo.VolStepEnabled = true;
                if (selectedDeviceInfo == defaultDeviceInfo)
                    RegisterHotKeys(true);
            }
            else
            {
                cmbVolStep.Enabled = false;
                selectedDeviceInfo.VolStepEnabled = false;
                if (selectedDeviceInfo == defaultDeviceInfo)
                    RegisterHotKeys(false);
            }
        }

        private void cmbVolStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDeviceInfo.VolStep = (float)cmbVolStep.SelectedValue;
            if (selectedDeviceInfo == defaultDeviceInfo)
                defaultDeviceInfo.Handler.SetNearestMultipleVolumeIfNeeded(defaultDeviceInfo.VolStep);
        }

        private void chkDontAutoMute_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDontAutoMute.Checked)
            {
                selectedDeviceInfo.DontAutoMute = true;
            }
            else
            {
                selectedDeviceInfo.DontAutoMute = false;
            }
        }

        public static void SetTimeout(Action<Task> action, int delay)
        {
            Task.Delay(delay).ContinueWith(action, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            saveSettingsIfNecessary();
            SetTimeout((task) => { btnSave.Enabled = true; }, 400);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                notifyIcon1_Click(sender, e);
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (!initVisible && !ShowInTaskbar)
            {
                this.ShowInTaskbar = true;
            }
            ShowAndActivate();
        }

        // see https://www.youtube.com/watch?v=FTO3LjJFLR4
        private void tsmiAutoRunOnStartup_Click(object sender, EventArgs e)
        {
            if (tsmiAutoRunOnStartup.Checked)
            {
                settings.AutoRunOnStartup = tsmiAutoRunOnStartup.Checked = false;
            }
            else
            {
                settings.AutoRunOnStartup = tsmiAutoRunOnStartup.Checked = true;
            }
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(runRegKeyPath, true))
            {
                if (settings.AutoRunOnStartup)
                {
                    regKey.SetValue(appName, "\"" + executablePath + "\" --hide-window", RegistryValueKind.String);
                }
                else
                {
                    regKey.DeleteValue(appName);
                }
            }
        }

        private void tsmiAutoSaveOnShutdown_Click(object sender, EventArgs e)
        {
            if (tsmiAutoSaveOnShutdown.Checked)
            {
                settings.AutoSaveOnShutdown = tsmiAutoSaveOnShutdown.Checked = false;
            }
            else
            {
                settings.AutoSaveOnShutdown = tsmiAutoSaveOnShutdown.Checked = true;
            }
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(appRegKeyPath, true))
            {
                regKey.SetValue("AutoSaveOnShutdown", settings.AutoSaveOnShutdown ? 1 : 0, RegistryValueKind.DWord);
            }
        }

        private void tsmiAutoUseColorTheme_Click(object sender, EventArgs e)
        {
            if (tsmiAutoUseColorTheme.Checked)
            {
                if(colorTheme!="light")
                {
                    applyColorTheme("light");
                }
                tsmiAutoUseColorTheme.Checked = false;
                settings.AutoUseColorTheme = false;
            }
            else
            {
                string newTheme = AppsUseLightTheme() ? "light" : "dark";
                if (newTheme != colorTheme)
                    applyColorTheme(newTheme);
                tsmiAutoUseColorTheme.Checked = true;
                settings.AutoUseColorTheme = true;
            }
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(appRegKeyPath, true))
            {
                regKey.SetValue("AutoUseColorTheme", settings.AutoUseColorTheme ? 1 : 0, RegistryValueKind.DWord);
            }
        }

        private void tsmiAlwaysOnTop_Click(object sender, EventArgs e)
        {
            if (tsmiAlwaysOnTop.Checked)
            {
                this.TopMost = false;
                tsmiAlwaysOnTop.Checked = false;
                settings.AlwaysOnTop = false;
            }
            else
            {
                this.TopMost = true;
                tsmiAlwaysOnTop.Checked = true;
                settings.AlwaysOnTop =  true;
            }
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(appRegKeyPath, true))
            {
                regKey.SetValue("AlwaysOnTop", settings.AlwaysOnTop ? 1 : 0, RegistryValueKind.DWord);
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            AssemblyName info = assembly.GetName();

            string version = info.Version.Major + "." + info.Version.Minor + "." + info.Version.Revision;
            string content = info.Name + " v" + version + "\n" +
                copyright;
            MessageBox.Show(content, "About " + info.Name);
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            // set willExit before Application.Exit(), since Form1_FormClosing may cancel exit
            this.willExit = true;
            Application.Exit();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (ActiveControl == numMaxVol)
            {
                this.ActiveControl = null;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!willExit)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            RegisterHotKeys(false);
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (outputDevices != null)
            {
                foreach (DeviceInfo info in outputDevices)
                {
                    if (info.Icon != null)
                    {
                        IconExtractor.DestroyIcon(info.Icon.Handle);
                    }
                }
            }
        }

    }
}
