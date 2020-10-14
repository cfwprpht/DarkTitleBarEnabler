using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkTitleBarEnabler {
    public partial class Messages : Form {
        public Messages() { InitializeComponent(); }

        private void Messages_Load(object sender, EventArgs e) { rtbNotify.Text = DarkEnabler.messageBuffer; }
    }
}
