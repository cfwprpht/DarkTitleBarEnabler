using DarkTitleBarEnabler.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DarkTitleBarEnabler {
    /// <summary>
    /// Enumuration for the registry checks.
    /// </summary>
    public enum RegCheck {
        True,
        False,
        Error
    }

    /// <summary>
    /// Changes the Title Bar Color without to change the Boarder.
    /// Will also change the Color of the Title Bar of a Inactive Window.
    /// </summary>
    public static class DarkEnabler {
        #region Globals
        /// <summary>
        /// Dark Title Bar Color defination.
        /// </summary>
        internal static string darkTitleBar = string.Empty;

        /// <summary>
        /// User defined Title Bar Color.
        /// </summary>
        internal static string userColor = string.Empty;

        /// <summary>
        /// Dark Title Bar Inactive Color defination.
        /// </summary>
        internal static string darkTitleBarInactive = string.Empty;

        /// <summary>
        /// User defined Title Bar Inactive Color.
        /// </summary>
        internal static string userColorInactive = string.Empty;

        /// <summary>
        /// Buffer to store messages into.
        /// </summary>
        internal static string messageBuffer = string.Empty;

        /// <summary>
        /// Flag to determine to use the user defined color.
        /// </summary>
        internal static bool _userColor = false;

        /// <summary>
        /// Tell the Loop inside the Watcher to stop.
        /// </summary>
        internal static bool watcherStop = false;

        /// <summary>
        /// Define a Interval after when the routine shall check for the changed Registry entry.
        /// </summary>
        internal static uint interval = 0;

        /// <summary>
        /// Counts up every time the routine changed the Title Bar back.
        /// </summary>
        internal static uint hackApplied = 0;

        /// <summary>
        /// Internal Settings Instance;
        /// </summary>
        internal static Settings settings;

        /// <summary>
        /// A Background Thread which will watch the registry.
        /// </summary>
        internal static Thread watcher;
        #endregion Globals

        /// <summary>
        /// Notifies the system of an event that an application has performed. An application should use this function if it performs an action that may affect the Shell.
        /// </summary>
        /// <param name="wEventId">Event ID.</param>
        /// <param name="uFlags">Flags.</param>
        /// <param name="dwItem1">First Item.</param>
        /// <param name="dwItem2">Second Item.</param>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// Store Class messages into a buffer and adds a Line break on end, to display in a window.
        /// </summary>
        /// <param name="message">The message to store to.</param>
        internal static void WriteLine(string message) { messageBuffer += message + "\n"; }

        /// <summary>
        /// Store Class messages into a buffer, to display in a window.
        /// </summary>
        /// <param name="message">The message to store to.</param>
        internal static void Write(string message) { messageBuffer += message; }

        /// <summary>
        /// Get the stored Accent color, depending on the set user flag.
        /// </summary>
        /// <returns>The User defined Accent Color if the User Color flag is set, else the Dark Accent Color.</returns>
        internal static string GetAccentColor() {
            if (_userColor) return GetColor();
            else return GetDarkColor();
        }

        /// <summary>
        /// Get the stored Inactive Accent color, depending on the set user flag.
        /// </summary>
        /// <returns>The User defined Inactive Accent Color if the User Color flag is set, else the Dark Inactive Accent Color.</returns>
        internal static string GetAccentColorInactive() {
            if (_userColor) return GetColorInactive();
            else return GetDarkColorInactive();
        }

        /// <summary>
        /// Checks if the Registry Watcher is running.
        /// </summary>
        /// <returns>True, if the Watcher is running, else false.</returns>
        internal static bool IsWatcherRunning() {
            if (watcher == null) return false;
            return watcher.IsAlive;
        }

        /// <summary>
        /// The Watcher Thread it self.
        /// </summary>
        internal static void RegistryWatcher() {
            while (!watcherStop) {
                if (AccentColor.CheckRegValue()         == RegCheck.False) { if (AccentColor.Hack())         SetHackApplied(); }
                if (AccentColorInactive.CheckRegValue() == RegCheck.False) { if (AccentColorInactive.Hack()) SetHackApplied(); }
                Thread.Sleep((int)(interval * 1000));
            }
        }

        /// <summary>
        /// Runs the Checker and Hacker Routines in a Background Thread.
        /// </summary>
        internal static void RunRegistryWatcher() {
            watcherStop = false;
            watcher     = new Thread(RegistryWatcher) { Name = "TitleBarHackRegistryWatcher", IsBackground = true, };
            watcher.SetApartmentState(ApartmentState.STA);
            watcher.Start();
        }

        /// <summary>
        /// Stops the Checker and Hacker Routines in the Background Thread.
        /// </summary>
        internal static void StopRegistryWatcher() { if (watcher != null) { if (watcher.IsAlive) { watcherStop = true; watcher.Abort(); } } }

        /// <summary>
        /// Get Dark Theme color.
        /// </summary>
        /// <returns>The Dark Title Bar color.</returns>
        internal static string GetDarkColor() { return darkTitleBar; }

        /// <summary>
        /// Get Dark Theme Inactive color.
        /// </summary>
        /// <returns>The Dark Title Bar Inactive color.</returns>
        internal static string GetDarkColorInactive() { return darkTitleBarInactive; }

        /// <summary>
        /// Set User defined custom color.
        /// </summary>
        /// <param name="color">The Hex color code to set, as string.</param>
        internal static void SetColor(string color) {
            settings.UserColor = userColor = color;
            settings.Save();
            settings.Reload();
        }

        /// <summary>
        /// Get User defined custom color.
        /// </summary>
        /// <returns>The user defined color.</returns>
        internal static string GetColor() { return userColor; }

        /// <summary>
        /// Get User defined Inactive custom color.
        /// </summary>
        /// <returns>The user defined color.</returns>
        internal static string GetColorInactive() { return userColorInactive; }

        /// <summary>
        /// Set User defined Inactive custom color.
        /// </summary>
        /// <param name="color">The Hex color code to set, as string.</param>
        internal static void SetColorInactive(string color) {
            userColorInactive = settings.UserColorInactive = color;
            settings.Save();
            settings.Reload();
        }

        /// <summary>
        /// Set the user flag to tell the routine to use the user defined color or not.
        /// </summary>
        /// <param name="state">The state to change to.</param>
        internal static void SetUserFlag(bool state) {
            _userColor = settings.UserColor_ = state;
            settings.Save();
            settings.Reload();
        }

        /// <summary>
        /// Returns the actual state of the user flag.
        /// </summary>
        /// <returns>The state fo the user flag.</returns>
        internal static bool GetUserFlag() { return _userColor; }

        /// <summary>
        /// Sets a new Interval for the Routine for when to check for the changed Registry entry.
        /// </summary>
        /// <param name="intv">The interval to set.</param>
        internal static void SetInterval(uint intv) {
            interval = settings.Interval = intv;
            settings.Save();
            settings.Reload();
        }

        /// <summary>
        /// Return the actual set interval.
        /// </summary>
        /// <returns>The actual set interval.</returns>
        internal static uint GetInterval() { return interval; }

        /// <summary>
        /// Count one up on the hackApplied state.
        /// </summary>
        internal static void SetHackApplied() {
            settings.HackApplied = ++hackApplied;
            settings.Save();
            settings.Reload();
        }

        /// <summary>
        /// Returns the number of, how often the hack was applied.
        /// </summary>
        /// <returns>the number of, how often the hack was applied.</returns>
        internal static uint GetHackApplied() { return hackApplied; }

        /// <summary>
        /// AccentColor RegistryKey.
        /// </summary>
        internal static class AccentColor {
            /// <summary>
            /// Get the actual set AccentColor from the Registry.
            /// </summary>
            /// <returns>The actual set Accent Color as a hex string.</returns>
            internal static string FromRegistry() { return ((int)Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\").GetValue("AccentColor", "error")).ToString("X"); }

            /// <summary>
            /// Check the Registry value which defines the Titlebar Color.
            /// </summary>
            /// <returns>True, if the value in the registry has not changed, else false.</returns>
            internal static RegCheck CheckRegValue() {
                string colorToCheck = darkTitleBar;
                if (_userColor) colorToCheck = userColor;

                try {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\");
                    if (key != null) {
                        string entryVTrue = ((int)key.GetValue("AccentColor")).ToString("X");
                        key.Close();
                        if (entryVTrue != string.Empty) {
                            if (colorToCheck.Equals(entryVTrue, StringComparison.OrdinalIgnoreCase)) {
                                WriteLine("[Check][Active]: Color do match. Nothing to do here.");
                                return RegCheck.True;
                            }
                            return RegCheck.False;
                        } else {
                            WriteLine("[Check][Active][Error]: Reading AccentColor Value from Registry.");
                            return RegCheck.Error;
                        }
                    } else WriteLine("[Check][Active][Error]: Couldn't get Registry Entry!");
                    key.Close();
                } catch (Exception e) { WriteLine("[Check][Active][Error]:\n" + e.ToString() + "\n[END]"); }
                return RegCheck.Error;
            }

            /// <summary>
            /// Change the Title Bar color  without to change the boarder too.
            /// </summary>
            /// <returns>True, when the new color was set, else false.</returns>
            internal static bool Hack() {
                string colorToHack = darkTitleBar;
                if (_userColor) colorToHack = userColor;

                try {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\", true);
                    if (key != null) key.SetValue("AccentColor", BitConverter.GetBytes(Convert.ToInt32(colorToHack, 16)), RegistryValueKind.Binary);
                    else {
                        WriteLine("[Hack][Active][Error]: Couldn't get Registry Key for writing!");
                        key.Close();
                        return false;
                    }
                    key.Close();

                    // Inform the shell that something has changed. Make the changes visible.
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                } catch (Exception e) {
                    Debug.WriteLine("[Hack][Active][Error]:\n " + e.ToString() + "\n[END]");
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// AccentColorInactive RegistryKey.
        /// </summary>
        internal static class AccentColorInactive {
            /// <summary>
            /// Get the actual set AccentColorInactive from the Registry.
            /// </summary>
            /// <returns>The actual set Inactive Accent Color as a hex string.</returns>
            internal static string FromRegistry() { return ((int)Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\").GetValue("AccentColorInactive")).ToString("X"); }

            /// <summary>
            /// Check if the 'AccentColorInactive' Entry Exists.
            /// </summary>
            /// <returns>True, if the entry exists, else false.</returns>
            internal static RegCheck Exists() {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\");
                if (key != null) {
                    IList<string> entrys = key.GetValueNames();
                    key.Close();
                    if (entrys.Count > 0) {
                        if (entrys.Contains("AccentColorInactive")) return RegCheck.True;
                        else return RegCheck.False;
                    } else {
                        WriteLine("[EXISTS][Inactive][Error]: DWM entry is empty.");
                        return RegCheck.Error;
                    }
                }
                WriteLine(@"[EXISTS][Inactive][Error]: 'Software\Microsoft\Windows\DWM\' Key is null.");
                key.Close();
                return RegCheck.Error;
            }

            /// <summary>
            /// Check the Registry value which defines the Titlebar Inactive Color.
            /// </summary>
            /// <returns>True, if the value in the registry has not changed, else false.</returns>
            internal static RegCheck CheckRegValue() {
                string colorToCheck = darkTitleBarInactive;
                if (_userColor) colorToCheck = userColorInactive;

                try {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\");
                    if (key != null) {
                        string entryVTrue = FromRegistry();
                        key.Close();
                        if (entryVTrue != string.Empty) {
                            if (colorToCheck.Equals(entryVTrue, StringComparison.OrdinalIgnoreCase)) {
                                WriteLine("[Check][Inactive]: Color do match. Nothing to do here.");
                                return RegCheck.True;
                            }
                            return RegCheck.False;
                        } else {
                            WriteLine("[Check][Inactive][Error]: Reading AccentColorInactive Value from Registry.");
                            return RegCheck.Error;
                        }
                    } else WriteLine("[Check][Inactive][Error]: Couldn't get Registry Entry!");
                    key.Close();
                } catch (Exception e) { WriteLine("[Check][Inactive][Error]:\n" + e.ToString() + "\n[END]"); }
                return RegCheck.Error;
            }

            /// <summary>
            /// Change the Title Bar Inactive color without to change the boarder too.
            /// </summary>
            /// <returns>True, when the new color was set, else false.</returns>
            internal static bool Hack() {
                string colorToHack = darkTitleBarInactive;
                if (_userColor) colorToHack = userColorInactive;

                try {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\", true);
                    if (key != null) key.SetValue("AccentColorInactive", BitConverter.GetBytes(Convert.ToInt32(colorToHack, 16)), RegistryValueKind.Binary);
                    else {
                        WriteLine("[Hack][Inactive][Error]: Couldn't get Registry Key for writing!");
                        key.Close();
                        return false;
                    }
                    key.Close();

                    // Inform the shell that something has changed. Make the changes visible.
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                } catch (Exception e) {
                    WriteLine("[Hack][Inactive][Error]:\n" + e.ToString() + "\n[END]");
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// ColorPrevalence RegistryKey.
        /// </summary>
        internal static class ColorPrevalence {
            /// <summary>
            /// Get the actual set ColorPrevalence from the Registry.
            /// </summary>
            /// <returns>The actual set Color Prevalence as a hex string.</returns>
            internal static int FromRegistry() { return (int)Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\").GetValue("ColorPrevalence"); }

            /// <summary>
            /// Check if the 'ColorPrevalence' Entry Exists.
            /// </summary>
            /// <returns>True, if the entry exists, else false.</returns>
            internal static RegCheck Exists() {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\");
                if (key != null) {
                    IList<string> entrys = key.GetValueNames();
                    key.Close();
                    if (entrys.Count > 0) {
                        if (entrys.Contains("ColorPrevalence")) return RegCheck.True;
                        else return RegCheck.False;
                    } else {
                        WriteLine("[EXISTS][PREVALENCE][Error]: DWM entry is empty.");
                        return RegCheck.Error;
                    }
                }
                WriteLine(@"[EXISTS][PREVALENCE][Error]: 'Software\Microsoft\Windows\DWM\' Key is null.");
                key.Close();
                return RegCheck.Error;
            }

            /// <summary>
            /// Toogle the ColorPrevalence state.
            /// </summary>
            /// <returns>True if the value could be toogled, else false.</returns>
            internal static bool Toogle() {
                try {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\", true);
                    if (key != null) key.SetValue("ColorPrevalence", !(bool)key.GetValue("ColorPrevalence"));
                    else {
                        Debug.WriteLine("[Toogle][Prevalence][Error]: Couldn't get Registry Key for writing!");
                        key.Close();
                        return false;
                    }
                    key.Close();

                    // Inform the shell that something has changed. Make the changes visible.
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                } catch (Exception e) {
                    WriteLine("[Toogle][Prevalence][Error]:\n" + e.ToString() + "\n[END]");
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// DWM RegistryKey.
        /// </summary>
        internal static class DWM {
            /// <summary>
            /// Check if we can access the DWM RegistryKey.
            /// </summary>
            /// <returns>True if we can access it, else false.</returns>
            internal static bool Exists() { return (Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM\") != null); }
        }
    }
}
