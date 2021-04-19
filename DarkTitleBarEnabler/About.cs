using DarkTitleBarEnabler.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace DarkTitleBarEnabler {
    public partial class About : Form {        
        private static bool formClose       = false;
        private static bool shown           = false;        
        public const int darkColor          = -12763843;
        public const int darkColorInactive  = -10526881;

        /// <summary>
        /// The Context Menu Items.
        /// </summary>
        private static ToolStripMenuItem menuItemExit;
        private static ToolStripMenuItem menuItemDarkTheme;
        private static ToolStripMenuItem menuItemUserTheme;
        private static ToolStripMenuItem menuItemWatcher;
        private static ToolStripMenuItem menuItemOptions;
        private static ToolStripMenuItem menuItemAbout;
        private static ToolStripMenuItem menuItemLog;

        /// <summary>
        /// Define a constant for the StringComparison option. 
        /// </summary>
        private const StringComparison ccIgnoreCase = StringComparison.CurrentCultureIgnoreCase;

        /// <summary>
        /// Settings instance.
        /// </summary>
        private static Settings set;

        public About() {
            // Initialize this form first.
            InitializeComponent();
            BackColor = Color.FromArgb(darkColor);
            ForeColor = Color.White;
            shown     = true;

            // Initialize DarkTitleBarHack Class.
            DarkEnabler.Write("Initializing DarkTitleBarHack()...");
            set                              = new Settings();
            DarkEnabler.darkTitleBar         = set.DarkTitleBar;
            DarkEnabler.darkTitleBarInactive = set.DarkTitleBarInactive;
            DarkEnabler.userColor            = set.UserColor;
            DarkEnabler.userColorInactive    = set.UserColorInactive;
            DarkEnabler._userColor           = set.UserColor_;
            DarkEnabler.interval             = set.Interval;
            DarkEnabler.hackApplied          = set.HackApplied;
            DarkEnabler.settings             = new Settings();
            DarkEnabler.WriteLine("OK");

            // Initialize components.
            menuItemExit      = new ToolStripMenuItem();
            menuItemDarkTheme = new ToolStripMenuItem();
            menuItemUserTheme = new ToolStripMenuItem();
            menuItemWatcher   = new ToolStripMenuItem();
            menuItemOptions   = new ToolStripMenuItem();
            menuItemAbout     = new ToolStripMenuItem();
            menuItemLog       = new ToolStripMenuItem();

            // Set MessagBox variables.
            MessagBox.DialogBack = MessagBox.ButtonBack   = Color.FromArgb(darkColorInactive);
            MessagBox.DialogFore = MessagBox.ButtonBorder = MessagBox.FormFore = Color.White;
            MessagBox.FormBack   = Color.FromArgb(darkColor);

            // First check for Admin rights aka is this process elevated?
            bool dwm_ok = false;
            if (CheckForAdminRights()) {
                // Check DWM registry entry and disable important buttons if false.
                dwm_ok = DarkEnabler.DWM.Exists();

                // If the Windows /DWM/ RegistryEntry Exists or is Accessable.
                if (dwm_ok) {
                    // Shall we Hack on Start?
                    if (set.HackOnStart) {
                        // Check if the 'AccentColorInactive' value exists within the Registry. If not, add it to it.
                        RegCheck state = DarkEnabler.AccentColorInactive.Exists();
                        if (state == RegCheck.False || !DarkEnabler.AccentColorInactive.FromRegistry().Equals(DarkEnabler.GetAccentColorInactive(), ccIgnoreCase)) {
                            if (!DarkEnabler.AccentColorInactive.Hack()) dwm_ok = false;
                            else DarkEnabler.SetHackApplied();
                        } // Check if reg Color do match stored one. Change it if not.
                        if (dwm_ok && !DarkEnabler.AccentColor.FromRegistry().Equals(DarkEnabler.GetAccentColor(), ccIgnoreCase)) {
                            if (!DarkEnabler.AccentColor.Hack()) dwm_ok = false;
                            else DarkEnabler.SetHackApplied();
                        } // Toogle Color prevalence
                        state = DarkEnabler.ColorPrevalence.Exists();
                        if (dwm_ok && state == RegCheck.True && !DarkEnabler.ColorPrevalence.FromRegistry().Equals(1)) {
                            if (!DarkEnabler.ColorPrevalence.Toogle()) dwm_ok = false;
                            else DarkEnabler.SetHackApplied();
                        }
                    }

                    // Shall we run the Registry Watcher on start of the app?
                    if (set.RunWatcherOnStart && dwm_ok) {
                        DarkEnabler.RunRegistryWatcher();

                        // Check if watcher is running and format context menu entry name.
                        if (DarkEnabler.IsWatcherRunning()) {
                            menuItemWatcher.Text = "Stop Watcher";
                            menuItemWatcher.ToolTipText = "Stop the Registry Watcher";
                            menuItemWatcher.Image = Resources.stop.ToBitmap();
                        }
                    }
                }
            } else Options.Error("You must run this app with Administrator rights.\nAll registry functions have been disabled.");

            // Initialize menuItems
            menuItemExit.MergeIndex       = 0;
            menuItemExit.Text             = "Exit";
            menuItemExit.Image            = Resources.close.ToBitmap();
            menuItemExit.Click           += new EventHandler(MenuItemExit_Click);
            menuItemOptions.MergeIndex    = 1;
            menuItemOptions.Enabled       = dwm_ok;
            menuItemOptions.Text          = "Options";
            menuItemOptions.ToolTipText   = "Lets you easely pick up custom colors for the Title Bar Hack";
            menuItemOptions.Image         = Resources.signpost.ToBitmap();
            menuItemOptions.Click        += new EventHandler(MenuItemOptions_Click);
            menuItemWatcher.MergeIndex    = 2;
            menuItemWatcher.Enabled       = dwm_ok;
            menuItemWatcher.Text          = "Run Watcher";
            menuItemWatcher.ToolTipText   = "Run the Registry Watcher";
            menuItemWatcher.Image         = Resources.play.ToBitmap();
            menuItemWatcher.Click        += new EventHandler(MenuItemWatcher_Click);
            menuItemDarkTheme.MergeIndex  = 3;
            menuItemDarkTheme.Enabled     = dwm_ok;
            menuItemDarkTheme.Text        = "Apply Dark Theme";
            menuItemDarkTheme.ToolTipText = "Applys the Dark Title Bar and Inactive Dark Title Bar Colors";
            menuItemDarkTheme.Image       = Resources.apply.ToBitmap();
            menuItemDarkTheme.Click      += new EventHandler(MenuItemApplyDarkTheme_Click);
            menuItemUserTheme.MergeIndex  = 4;
            menuItemUserTheme.Enabled     = dwm_ok;
            menuItemUserTheme.Text        = "Apply User Theme";
            menuItemUserTheme.ToolTipText = "Applys the Title Bar and Inactive Title Bar User Colors";
            menuItemUserTheme.Image       = Resources.apply.ToBitmap();
            menuItemUserTheme.Click      += new EventHandler(MenuItemApplyUserTheme_Click);
            menuItemAbout.MergeIndex      = 5;
            menuItemAbout.Text            = "About";
            menuItemAbout.Image           = Resources.about.ToBitmap();
            menuItemAbout.Click          += new EventHandler(MenuItemAbout_Click);
            menuItemLog.MergeIndex        = 6;
            menuItemLog.Text              = "Notify";
            menuItemLog.Image             = Resources.bug.ToBitmap();
            menuItemLog.Click            += new EventHandler(MenuItemNotify_Click);

            // Initialize contextMenu
            contextMenu.BackColor = Color.FromArgb(darkColor);
            contextMenu.ForeColor = Color.White;
            contextMenu.Renderer  = new DarkRenderer();
            contextMenu.Items.AddRange(new ToolStripMenuItem[] { menuItemExit, menuItemOptions, menuItemWatcher, menuItemDarkTheme, menuItemUserTheme, menuItemAbout, menuItemLog });
        }

        /// <summary>
        /// Check if this process is elevated.
        /// </summary>
        /// <returns>True, if this process is elevated, else false.</returns>
        private bool CheckForAdminRights() {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Show this Form.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemOptions_Click(object Sender, EventArgs e) {
            Options opt = new Options();
            if (opt.ShowDialog() == DialogResult.OK) opt.Close();
        }

        /// <summary>
        /// Turn the Registry Watcher on or off.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemWatcher_Click(object Sender, EventArgs e) {
            if (DarkEnabler.IsWatcherRunning()) {
                DarkEnabler.StopRegistryWatcher();
                menuItemWatcher.Text = "Run Watcher";
                menuItemWatcher.ToolTipText = "Run the Registry Watcher";
                menuItemWatcher.Image = Resources.play.ToBitmap();
            } else {
                DarkEnabler.RunRegistryWatcher();
                menuItemWatcher.Text = "Stop Watcher";
                menuItemWatcher.ToolTipText = "Stop the Registry Watcher";
                menuItemWatcher.Image = Resources.stop.ToBitmap();
            }
        }

        /// <summary>
        /// Apply the Dark Theme.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemApplyDarkTheme_Click(object Sender, EventArgs e) {
            DarkEnabler.SetUserFlag(false);
            if (DarkEnabler.AccentColor.Hack()) DarkEnabler.SetHackApplied();
            if (DarkEnabler.AccentColorInactive.Hack()) DarkEnabler.SetHackApplied();
            if (!DarkEnabler.ColorPrevalence.FromRegistry().Equals(1)) {
                if (DarkEnabler.ColorPrevalence.Toogle()) DarkEnabler.SetHackApplied();
            }
        }

        /// <summary>
        /// Apply the User Theme.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemApplyUserTheme_Click(object Sender, EventArgs e) {
            DarkEnabler.SetUserFlag(true);
            if (DarkEnabler.AccentColor.Hack()) DarkEnabler.SetHackApplied();
            if (DarkEnabler.AccentColorInactive.Hack()) DarkEnabler.SetHackApplied();
            if (!DarkEnabler.ColorPrevalence.FromRegistry().Equals(1)) {
                if (DarkEnabler.ColorPrevalence.Toogle()) DarkEnabler.SetHackApplied();
            }
        }

        /// <summary>
        /// Show this Form.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemAbout_Click(object Sender, EventArgs e) {
            set.Reload();
            DarkEnabler.hackApplied = set.HackApplied;
            Show();
        }

        /// <summary>
        /// Close the form, which closes the application.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemExit_Click(object Sender, EventArgs e) {
            formClose = true;
            Close();
        }

        /// <summary>
        /// Show the message log.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void MenuItemNotify_Click(object sender, EventArgs e) {
            Messages notes = new Messages();
            if (notes.ShowDialog() == DialogResult.OK) notes.Close();
        }

        /// <summary>
        /// Hide this Form.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void About_FormClosing(object sender, FormClosingEventArgs e) {
            if (!formClose) e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Determine that this is the first shown event and we need to hide the form from the user.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void About_Shown(object sender, EventArgs e) {
            if (shown) {
                Hide();
                shown = false;
            }
        }

        /// <summary>
        /// Open the showen link in a browser.
        /// </summary>
        /// <param name="Sender">The Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { Process.Start(linkLabel.Text); }
    }

    public static class StringExtension {
        /// <summary>
        /// A string that represents a valid Hex Value
        /// </summary>
        private static readonly string hex = "0123456789ABCDEFabcdef";

        /// <summary>
        /// Check if a input string is a valid Hex Value.
        /// </summary>
        /// <param name="value">The value to check for.</param>
        /// <returns>True if the value contains only Hex values, else False.</returns>
        public static bool IsHex(this string value) {
            for (int i = 0; i < value.Length; i++) if (!hex.Contains(value[i].ToString())) return false;     // Loop trough the whole string and check every single digit if it is not a hex value. If so return false.
            return true;
        }

        /// <summary>
        /// Check if the input string is Hex align.
        /// </summary>
        /// <param name="value">The string to check for.</param>
        /// <returns>True, if the hex string is hex alinged, else false.</returns>
        public static bool IsHexAlign(this string value) {
            if (value.Length % 2 == 0) return true;
            return false;
        }
    }
}
