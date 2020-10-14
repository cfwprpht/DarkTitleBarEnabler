using System.Configuration;

namespace DarkTitleBarEnabler.Properties {
    internal sealed partial class Settings {
        
        public Settings() {}

        [UserScopedSetting]
        [DefaultSettingValue("FF3D3D3D")]
        public string DarkTitleBar {
            get { return (string)this["DarkTitleBar"]; }
            set { this["DarkTitleBar"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("FF002FFF")]
        public string UserColor {
            get { return (string)this["UserColor"]; }
            set { this["UserColor"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("FF5F5F5F")]
        public string DarkTitleBarInactive {
            get { return (string)this["DarkTitleBarInactive"]; }
            set { this["DarkTitleBarInactive"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("FF738DFF")]
        public string UserColorInactive {
            get { return (string)this["UserColorInactive"]; }
            set { this["UserColorInactive"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool UserColor_ {
            get { return (bool)this["UserColor_"]; }
            set { this["UserColor_"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("1")]
        public uint Interval {
            get { return (uint)this["Interval"]; }
            set { this["Interval"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("0")]
        public uint HackApplied {
            get { return (uint)this["HackApplied"]; }
            set { this["HackApplied"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool HackOnStart {
            get { return (bool)this["HackOnStart"]; }
            set { this["HackOnStart"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool RunWatcherOnStart {
            get { return (bool)this["RunWatcherOnStart"]; }
            set { this["RunWatcherOnStart"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool ShowHelp {
            get { return (bool)this["ShowHelp"]; }
            set { this["ShowHelp"] = value; }
        }
    }
}
