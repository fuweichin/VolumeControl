using System;
using System.Collections.Generic;

namespace VolumeControl
{
    public class DeviceSettings
    {
        public int Version { get; set; }
        // json settings
        public List<DeviceInfo> OutputDevices { get; set; }
        public bool HideOSD { get; set; }
        // registry settings
        public bool AutoRunOnStartup { get; set; }
        public bool AutoSaveOnShutdown { get; set; }
        public bool AutoUseColorTheme { get; set; }
        public bool AlwaysOnTop { get; set; }
    }
}
