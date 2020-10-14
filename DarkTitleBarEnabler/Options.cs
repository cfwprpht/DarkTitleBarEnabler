using DarkTitleBarEnabler.Properties;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DarkTitleBarEnabler {
    public partial class Options : Form {
        #region Vars
        private static bool isDarkThemedShowen     = false;
        private static bool isRegUseInactiveShowen = false;
        private static bool isRegUseActiveShowen   = false;
        private static bool isPictureBoxRegShowen  = false;
        private static bool isInactiveEntry        = false;
        private static bool isActiveEntry          = false;
        private static bool isApplyShowen          = false;
        private static bool isIntervalShown        = false;
        private static bool isShowHelp             = true;
        private static readonly Regex regEx        = new Regex("^\\d(\\d|(?<!-)-)*\\d$");
        #endregion Vars

        /// <summary>
        /// Class initializer.
        /// </summary>
        public Options() { InitializeComponent(); }

        /// <summary>
        /// Ask a custom question before doing stuff with the registry.
        /// </summary>
        /// <param name="caption">The title of the question window.</param>
        /// <param name="mes">The message to display.</param>
        /// <returns>True, when the user clicked OK, else false.</returns>
        private bool Question(string caption, string mes) {
            if (MessagBox.Question(caption, "\n" + mes + "\nAre you sure?") == DialogResult.OK) return true;
            return false;
        }

        /// <summary>
        /// Display a custom error message.
        /// </summary>
        /// <param name="mes">The message to display.</param>
        internal static void Error(string mes) { MessagBox.Error("Error", mes); }

        /// <summary>
        /// OnLoad of Form do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Options_Load(object sender, EventArgs e) {
            // Add tool tip to the childs of numUpdown by code.
            foreach (Control child in numUpDown.Controls) {
                child.MouseHover += (_sender, _e) => {
                    if (isShowHelp && !isIntervalShown) {
                        Help.ShowPopup(numUpDown,
                                       "The Interval of the Registry Watcher to check values be changed, if used.",
                                       new Point(Location.X + numUpDown.Right, Location.Y + numUpDown.Bottom));
                        isIntervalShown = true;
                    }
                };
                child.MouseLeave += (_sender, _e) => { if (isShowHelp) isIntervalShown = false; };
            }

            // Load from settings.
            Settings set                   = new Settings();
            checkHelp.Checked              = isShowHelp = set.ShowHelp;
            textBoxActive.Text             = set.UserColor;
            textBoxInactive.Text           = set.UserColorInactive;
            numUpDown.Value                = set.Interval;
            checkHackOnStart.Checked       = set.HackOnStart;
            checkRunWatcherOnStart.Checked = set.RunWatcherOnStart;
        }

        /// <summary>
        /// On Button click do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PBDarkTheme_Click(object sender, EventArgs e) {
            if (Question("DarkTheme", "This will apply the DarkTheme to your Registry")) {
                DarkEnabler.SetUserFlag(false);
                if (DarkEnabler.AccentColor.Hack()) DarkEnabler.SetHackApplied();
                if (DarkEnabler.AccentColorInactive.Hack()) DarkEnabler.SetHackApplied();
            }
        }

        /// <summary>
        /// On Button click do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonApply_Click(object sender, EventArgs e) {
            if (Question("ApplyTheme", "This will apply the Colors to your Registry")) {
                if (textBoxActive.Text == string.Empty) {
                    Error("Active Text Field is empty.");
                    return;
                } else if (textBoxInactive.Text == string.Empty) {
                    Error("Inactive Text Field is empty.");
                    return;
                } else if (textBoxActive.TextLength > 13) {
                    Error("Value of Active Text Field is to long.");
                    return;
                } else if (textBoxInactive.TextLength > 13) { // Why 13?
                    Error("Value of Inactive Text Field is to long.");
                    return;
                } else if (!textBoxActive.Text.IsHex() || !textBoxInactive.Text.IsHex()) {
                    if (!regEx.IsMatch(textBoxActive.Text.Replace(",", "")) || !regEx.IsMatch(textBoxInactive.Text.Replace(",", ""))) {
                        Error("\nNot a Hex Number nor a RGB Color Integer\nPlease use: FFAAB7D9\nor\n255, 45, 123\nas formating.");
                        return;
                    } else {
                        string[] split = textBoxActive.Text.Split(',');
                        if (split.Length != 3) {
                            Error("RGB Color of Active TextField is to short.\nValues have to be 3.");
                            return;
                        } else {
                            uint result2, result3;
                            if (!uint.TryParse(split[0], out uint result)) {
                                Error("First RGB Color value of Active TextField is not a 'non negative' number.");
                                return;
                            } else if (!uint.TryParse(split[1], out result2)) {
                                Error("Second RGB Color value of Active TextField is not a 'non negative' number.");
                                return;
                            } else if (!uint.TryParse(split[2], out result3)) {
                                Error("Third RGB Color value of Active TextField is not a 'non negative' number.");
                                return;
                            } else if (result > 255) {
                                Error("First RGB Color value of Active TextField is greater then 255.\nImpossibrew !!");
                                return;
                            } else if (result2 > 255) {
                                Error("Second RGB Color value of Active TextField is greater then 255.\nImpossibrew !!");
                                return;
                            } else if (result3 > 255) {
                                Error("Third RGB Color value of Active TextField is greater then 255.\nImpossibrew !!");
                                return;
                            }
                            byte[] toHex = new byte[3];
                            toHex[0] = Convert.ToByte(result);
                            toHex[1] = Convert.ToByte(result2);
                            toHex[2] = Convert.ToByte(result3);
                            textBoxActive.Text = "ff" + BitConverter.ToString(toHex).Replace("-", "");
                        }

                        split = textBoxInactive.Text.Split(',');
                        if (split.Length != 3) {
                            Error("RGB Color of Inactive TextField is to short.\nValues have to be 3.");
                            return;
                        } else {
                            uint result2, result3;
                            if (!uint.TryParse(split[0], out uint result)) {
                                Error("First RGB Color value of Inactive TextField is not a 'non negative' number.");
                                return;
                            } else if (!uint.TryParse(split[1], out result2)) {
                                Error("Second RGB Color value of Inactive TextField is not a 'non negative' number.");
                                return;
                            } else if (!uint.TryParse(split[2], out result3)) {
                                Error("Third RGB Color value of Inactive TextField is not a 'non negative' number.");
                                return;
                            } else if (result > 255) {
                                Error("First RGB Color value of Inactive TextField is greater then 255.\nImpossibrew !!");
                                return;
                            } else if (result2 > 255) {
                                Error("Second RGB Color value of Inactive TextField is greater then 255.\nImpossibrew !!");
                                return;
                            } else if (result3 > 255) {
                                Error("Third RGB Color value of Inactive TextField is greater then 255.\nImpossibrew !!");
                                return;
                            }
                            byte[] toHex = new byte[3];
                            toHex[0] = Convert.ToByte(result);
                            toHex[1] = Convert.ToByte(result2);
                            toHex[2] = Convert.ToByte(result3);
                            textBoxInactive.Text = "ff" + BitConverter.ToString(toHex).Replace("-", "");
                        }
                    }
                } else if (textBoxActive.TextLength > 8 || !textBoxActive.Text.IsHexAlign()) {
                    Error("The Active Color is either to long or not Hex Aligned!");
                    return;
                } else if (textBoxInactive.TextLength > 8 || !textBoxInactive.Text.IsHexAlign()) {
                    Error("The Inactive Color is either to long or not Hex Aligned!");
                    return;
                } else {
                    // Hex align the user input to 4 bytes.
                    string addActive   = string.Empty;
                    string addInactive = string.Empty;
                    for (int i = 0; i < (4 - textBoxActive.TextLength / 2); i++) {
                        addActive += "ff";
                    }
                    textBoxActive.Text = addActive + textBoxActive.Text;

                    for (int i = 0; i < (4 - textBoxInactive.TextLength / 2); i++) {
                        addInactive += "ff";
                    }
                    textBoxInactive.Text = addInactive + textBoxInactive.Text;
                }

                // Went everything like it should?
                if (textBoxActive.Text.Length > 8) {
                    Error("Something went very bad!\nThe Active Color is to long.\nStop.");
                    return;
                } else if (textBoxInactive.Text.Length > 8) {
                    Error("Something went very bad!\nThe Inactive Color is to long.\nStop.");
                    return;
                }

                // Add Testing ?

                DarkEnabler.SetColor(textBoxActive.Text);
                DarkEnabler.SetColorInactive(textBoxInactive.Text);
                DarkEnabler.SetUserFlag(true);
                if (DarkEnabler.AccentColor.Hack()) DarkEnabler.SetHackApplied();
                if (DarkEnabler.AccentColorInactive.Hack()) DarkEnabler.SetHackApplied();
            }
        }

        /// <summary>
        /// On Button click do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbActive_Click(object sender, EventArgs e) {
            if (Question("ActiveColor", "This will read the Active Color from your Registry")) {
                textBoxActive.Text = DarkEnabler.AccentColor.FromRegistry();
            }
        }

        /// <summary>
        /// On Button click do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbInactive_Click(object sender, EventArgs e) {
            if (Question("InactiveColor", "This will read the Inactive Color from your Registry")) {
                textBoxInactive.Text = DarkEnabler.AccentColorInactive.FromRegistry();
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TextBoxActive_MouseHover(object sender, EventArgs e) {
            if (isShowHelp && !isActiveEntry) {
                Help.ShowPopup(textBoxActive,
                               "The Forms Border Active Color",
                               new Point(Location.X + textBoxActive.Right, Location.Y + textBoxActive.Bottom)
                               );
                isActiveEntry = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TextBoxInactive_MouseHover(object sender, MouseEventArgs e) {
            if (isShowHelp && !isInactiveEntry) {                
                Help.ShowPopup(textBoxInactive,
                               "The Forms Border Inactive Color",
                               new Point(Location.X + textBoxInactive.Right, Location.Y + textBoxInactive.Bottom)
                               );             
                isInactiveEntry = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbDarkTheme_MouseHover(object sender, EventArgs e) {
            if (isShowHelp && !isDarkThemedShowen) {
                Help.ShowPopup(pBDarkTheme, "Applys the Dark Themed Title Bar Active and Inactive Colors to the Registry",
                           new Point(Location.X + pBDarkTheme.Right, Location.Y + pBDarkTheme.Bottom));
                isDarkThemedShowen = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbActive_MouseHover(object sender, EventArgs e) {
            if (isShowHelp && !isRegUseActiveShowen) {
                Help.ShowPopup(pBActive,
                               "..as Active Title Bar Color",
                               new Point(Location.X + pBActive.Right, Location.Y + pBActive.Bottom)
                               );
                isRegUseActiveShowen = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbInactive_MouseHover(object sender, EventArgs e) {            
            if (isShowHelp && !isRegUseInactiveShowen) {
                Help.ShowPopup(pBInactive,
                               "..as Inactive Title Bar Color",
                               new Point(Location.X + pBInactive.Right, Location.Y + pBInactive.Bottom)
                               );
                isRegUseInactiveShowen = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbReg_MouseHover(object sender, EventArgs e) {
            if (isShowHelp && !isPictureBoxRegShowen) {
             Help.ShowPopup(pBReg,
                            "Use actual set Window Color...",
                            new Point(Location.X + pBReg.Right, Location.Y + pBReg.Bottom)
                            );
                isPictureBoxRegShowen = true;
            }
        }

        /// <summary>
        /// On MouseHover do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonApply_MouseHover(object sender, EventArgs e) {
            if (isShowHelp && !isApplyShowen) {                
                 Help.ShowPopup(buttonApply,
                                "This will apply the Hex Colors to your Registry",
                                new Point(Location.X + buttonApply.Right, Location.Y + buttonApply.Bottom)
                                );                 
                isApplyShowen = true;
            }
        }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbReg_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isPictureBoxRegShowen = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbBDarkTheme_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isDarkThemedShowen = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbInactive_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isRegUseInactiveShowen = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PbActive_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isRegUseActiveShowen = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TextBoxInactive_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isInactiveEntry = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TextBoxActive_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isActiveEntry = false; }

        /// <summary>
        /// On MouseLeave do.
        /// </summary>
        /// <param name="sender">The Event Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonApply_MouseLeave(object sender, EventArgs e) { if (isShowHelp) isApplyShowen = false; }

        /// <summary>
        /// On CheckBox checked changed do.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckBoxHelp_CheckedChanged(object sender, EventArgs e) {
            if (checkHelp.Checked) isShowHelp = true;
            else isShowHelp = false;

            Settings set = new Settings() { ShowHelp = isShowHelp };
            set.Save();
        }

        /// <summary>
        /// On HackOnStart checkBox changed do.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckHackOnStart_CheckedChanged(object sender, EventArgs e) {
            Settings set = new Settings() { HackOnStart = checkHackOnStart.Checked };
            set.Save();
        }

        /// <summary>
        /// On RunWatcherOnStart checkBox changed do.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckRunWatcherOnStart_CheckedChanged(object sender, EventArgs e) {
            Settings set = new Settings() { RunWatcherOnStart = checkRunWatcherOnStart.Checked };
            set.Save();
        }

        /// <summary>
        /// On FormClosing do.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Options_FormClosing(object sender, FormClosingEventArgs e) { DarkEnabler.SetInterval((uint)numUpDown.Value); }
    }
}
