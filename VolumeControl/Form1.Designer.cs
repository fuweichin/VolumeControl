namespace VolumeControl
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackCurrVol = new System.Windows.Forms.TrackBar();
            this.lblCurrVol = new System.Windows.Forms.Label();
            this.numMaxVol = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.chkEnableMaxVol = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkEnableVolStep = new System.Windows.Forms.CheckBox();
            this.chkDontAutoMute = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAutoRunOnStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAutoSaveOnShutdown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAutoUseColorTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCurrVolLevel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDynamicRange = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.picDeviceIcon = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbVolStep = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackCurrVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxVol)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeviceIcon)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output device:";
            // 
            // cmbDevices
            // 
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.Location = new System.Drawing.Point(176, 20);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(279, 28);
            this.cmbDevices.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Master volume:";
            // 
            // trackCurrVol
            // 
            this.trackCurrVol.Enabled = false;
            this.trackCurrVol.LargeChange = 10;
            this.trackCurrVol.Location = new System.Drawing.Point(148, 58);
            this.trackCurrVol.Maximum = 100;
            this.trackCurrVol.Name = "trackCurrVol";
            this.trackCurrVol.Size = new System.Drawing.Size(227, 45);
            this.trackCurrVol.TabIndex = 4;
            this.trackCurrVol.TickFrequency = 10;
            this.trackCurrVol.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // lblCurrVol
            // 
            this.lblCurrVol.AutoSize = true;
            this.lblCurrVol.Location = new System.Drawing.Point(381, 69);
            this.lblCurrVol.Name = "lblCurrVol";
            this.lblCurrVol.Size = new System.Drawing.Size(17, 20);
            this.lblCurrVol.TabIndex = 4;
            this.lblCurrVol.Text = "0";
            // 
            // numMaxVol
            // 
            this.numMaxVol.Location = new System.Drawing.Point(214, 63);
            this.numMaxVol.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numMaxVol.Name = "numMaxVol";
            this.numMaxVol.Size = new System.Drawing.Size(65, 27);
            this.numMaxVol.TabIndex = 3;
            this.numMaxVol.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Maximum volume:";
            // 
            // chkEnableMaxVol
            // 
            this.chkEnableMaxVol.AutoSize = true;
            this.chkEnableMaxVol.Location = new System.Drawing.Point(20, 32);
            this.chkEnableMaxVol.Name = "chkEnableMaxVol";
            this.chkEnableMaxVol.Size = new System.Drawing.Size(360, 24);
            this.chkEnableMaxVol.TabIndex = 1;
            this.chkEnableMaxVol.Text = "Listen volume changes and limit volume if needed";
            this.chkEnableMaxVol.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Volume step:";
            // 
            // chkEnableVolStep
            // 
            this.chkEnableVolStep.AutoSize = true;
            this.chkEnableVolStep.Location = new System.Drawing.Point(20, 98);
            this.chkEnableVolStep.Name = "chkEnableVolStep";
            this.chkEnableVolStep.Size = new System.Drawing.Size(368, 24);
            this.chkEnableVolStep.TabIndex = 4;
            this.chkEnableVolStep.Text = "Listen volume keys and use custom volume adjuster";
            this.chkEnableVolStep.UseVisualStyleBackColor = true;
            // 
            // chkDontAutoMute
            // 
            this.chkDontAutoMute.AutoSize = true;
            this.chkDontAutoMute.Location = new System.Drawing.Point(20, 162);
            this.chkDontAutoMute.Name = "chkDontAutoMute";
            this.chkDontAutoMute.Size = new System.Drawing.Size(392, 24);
            this.chkDontAutoMute.TabIndex = 7;
            this.chkDontAutoMute.Text = "Don\'t auto-mute when volume is changed to 0 (lowest)";
            this.chkDontAutoMute.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(355, 524);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAutoRunOnStartup,
            this.tsmiAutoSaveOnShutdown,
            this.toolStripSeparator2,
            this.tsmiAutoUseColorTheme,
            this.tsmiAlwaysOnTop,
            this.toolStripSeparator1,
            this.tsmiAbout,
            this.tsmiExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(256, 148);
            // 
            // tsmiAutoRunOnStartup
            // 
            this.tsmiAutoRunOnStartup.Name = "tsmiAutoRunOnStartup";
            this.tsmiAutoRunOnStartup.Size = new System.Drawing.Size(255, 22);
            this.tsmiAutoRunOnStartup.Text = "Auto run on startup/login";
            this.tsmiAutoRunOnStartup.Click += new System.EventHandler(this.tsmiAutoRunOnStartup_Click);
            // 
            // tsmiAutoSaveOnShutdown
            // 
            this.tsmiAutoSaveOnShutdown.Name = "tsmiAutoSaveOnShutdown";
            this.tsmiAutoSaveOnShutdown.Size = new System.Drawing.Size(255, 22);
            this.tsmiAutoSaveOnShutdown.Text = "Auto save on shutdown/logout";
            this.tsmiAutoSaveOnShutdown.Click += new System.EventHandler(this.tsmiAutoSaveOnShutdown_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(252, 6);
            // 
            // tsmiAutoUseColorTheme
            // 
            this.tsmiAutoUseColorTheme.Name = "tsmiAutoUseColorTheme";
            this.tsmiAutoUseColorTheme.Size = new System.Drawing.Size(255, 22);
            this.tsmiAutoUseColorTheme.Text = "Auto use light/dark theme";
            this.tsmiAutoUseColorTheme.Click += new System.EventHandler(this.tsmiAutoUseColorTheme_Click);
            // 
            // tsmiAlwaysOnTop
            // 
            this.tsmiAlwaysOnTop.Name = "tsmiAlwaysOnTop";
            this.tsmiAlwaysOnTop.Size = new System.Drawing.Size(255, 22);
            this.tsmiAlwaysOnTop.Text = "Always on top";
            this.tsmiAlwaysOnTop.Click += new System.EventHandler(this.tsmiAlwaysOnTop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(252, 6);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(255, 22);
            this.tsmiAbout.Text = "About";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(255, 22);
            this.tsmiExit.Text = "Exit";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblCurrVolLevel);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblDynamicRange);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblCurrVol);
            this.groupBox1.Controls.Add(this.trackCurrVol);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 147);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Volume Info";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Volume Info";
            // 
            // lblCurrVolLevel
            // 
            this.lblCurrVolLevel.AutoSize = true;
            this.lblCurrVolLevel.Location = new System.Drawing.Point(151, 107);
            this.lblCurrVolLevel.Name = "lblCurrVolLevel";
            this.lblCurrVolLevel.Size = new System.Drawing.Size(102, 20);
            this.lblCurrVolLevel.TabIndex = 6;
            this.lblCurrVolLevel.Text = "-∞dB (muted)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "Volume level:";
            // 
            // lblDynamicRange
            // 
            this.lblDynamicRange.AutoSize = true;
            this.lblDynamicRange.Location = new System.Drawing.Point(151, 30);
            this.lblDynamicRange.Name = "lblDynamicRange";
            this.lblDynamicRange.Size = new System.Drawing.Size(194, 20);
            this.lblDynamicRange.TabIndex = 2;
            this.lblDynamicRange.Text = "-60dB~0dB; increment=1dB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Dynamic range:";
            // 
            // picDeviceIcon
            // 
            this.picDeviceIcon.Location = new System.Drawing.Point(138, 19);
            this.picDeviceIcon.Name = "picDeviceIcon";
            this.picDeviceIcon.Size = new System.Drawing.Size(32, 32);
            this.picDeviceIcon.TabIndex = 13;
            this.picDeviceIcon.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cmbVolStep);
            this.groupBox2.Controls.Add(this.chkEnableMaxVol);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkEnableVolStep);
            this.groupBox2.Controls.Add(this.chkDontAutoMute);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numMaxVol);
            this.groupBox2.Location = new System.Drawing.Point(12, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 234);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Per-device Settings";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 20);
            this.label9.TabIndex = 6;
            this.label9.Text = "Per-device Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(55, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(277, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "This option is secondary to above listeners";
            // 
            // cmbVolStep
            // 
            this.cmbVolStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVolStep.FormattingEnabled = true;
            this.cmbVolStep.Location = new System.Drawing.Point(214, 128);
            this.cmbVolStep.Name = "cmbVolStep";
            this.cmbVolStep.Size = new System.Drawing.Size(65, 28);
            this.cmbVolStep.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(475, 571);
            this.Controls.Add(this.picDeviceIcon);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDevices);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "VolumeControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackCurrVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxVol)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeviceIcon)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackCurrVol;
        private System.Windows.Forms.Label lblCurrVol;
        private System.Windows.Forms.CheckBox chkEnableMaxVol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMaxVol;
        private System.Windows.Forms.CheckBox chkEnableVolStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkDontAutoMute;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoRunOnStartup;
        private System.Windows.Forms.PictureBox picDeviceIcon;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDynamicRange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrVolLevel;
        private System.Windows.Forms.ComboBox cmbVolStep;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem tsmiAlwaysOnTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoUseColorTheme;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoSaveOnShutdown;
    }
}