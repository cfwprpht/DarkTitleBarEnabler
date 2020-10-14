namespace DarkTitleBarEnabler {
    partial class Options {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.labelActive = new System.Windows.Forms.Label();
            this.labelInactive = new System.Windows.Forms.Label();
            this.labelInterval = new System.Windows.Forms.Label();
            this.pBDarkTheme = new System.Windows.Forms.PictureBox();
            this.pBActive = new System.Windows.Forms.PictureBox();
            this.pBInactive = new System.Windows.Forms.PictureBox();
            this.pBReg = new System.Windows.Forms.PictureBox();
            this.checkHelp = new DarkTitleBarEnabler.ColorCheckBox();
            this.numUpDown = new DarkTitleBarEnabler.ColorNumUpDown();
            this.buttonApply = new DarkTitleBarEnabler.ColorButton();
            this.textBoxInactive = new DarkTitleBarEnabler.ColorTextBox();
            this.textBoxActive = new DarkTitleBarEnabler.ColorTextBox();
            this.checkHackOnStart = new DarkTitleBarEnabler.ColorCheckBox();
            this.checkRunWatcherOnStart = new DarkTitleBarEnabler.ColorCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBDarkTheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBInactive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // labelActive
            // 
            this.labelActive.AutoSize = true;
            this.labelActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActive.ForeColor = System.Drawing.Color.White;
            this.labelActive.Location = new System.Drawing.Point(40, 9);
            this.labelActive.Name = "labelActive";
            this.labelActive.Size = new System.Drawing.Size(77, 29);
            this.labelActive.TabIndex = 0;
            this.labelActive.Text = "Active";
            // 
            // labelInactive
            // 
            this.labelInactive.AutoSize = true;
            this.labelInactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInactive.ForeColor = System.Drawing.Color.White;
            this.labelInactive.Location = new System.Drawing.Point(203, 9);
            this.labelInactive.Name = "labelInactive";
            this.labelInactive.Size = new System.Drawing.Size(94, 29);
            this.labelInactive.TabIndex = 1;
            this.labelInactive.Text = "Inactive";
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInterval.ForeColor = System.Drawing.Color.White;
            this.labelInterval.Location = new System.Drawing.Point(339, 9);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(90, 29);
            this.labelInterval.TabIndex = 2;
            this.labelInterval.Text = "Interval";
            // 
            // pBDarkTheme
            // 
            this.pBDarkTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBDarkTheme.Image = ((System.Drawing.Image)(resources.GetObject("pBDarkTheme.Image")));
            this.pBDarkTheme.Location = new System.Drawing.Point(337, 111);
            this.pBDarkTheme.Name = "pBDarkTheme";
            this.pBDarkTheme.Size = new System.Drawing.Size(125, 110);
            this.pBDarkTheme.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBDarkTheme.TabIndex = 6;
            this.pBDarkTheme.TabStop = false;
            this.pBDarkTheme.Click += new System.EventHandler(this.PBDarkTheme_Click);
            this.pBDarkTheme.MouseLeave += new System.EventHandler(this.PbBDarkTheme_MouseLeave);
            this.pBDarkTheme.MouseHover += new System.EventHandler(this.PbDarkTheme_MouseHover);
            // 
            // pBActive
            // 
            this.pBActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBActive.Image = global::DarkTitleBarEnabler.Properties.Resources.pfeilrauf;
            this.pBActive.Location = new System.Drawing.Point(84, 82);
            this.pBActive.Name = "pBActive";
            this.pBActive.Size = new System.Drawing.Size(51, 50);
            this.pBActive.TabIndex = 7;
            this.pBActive.TabStop = false;
            this.pBActive.Click += new System.EventHandler(this.PbActive_Click);
            this.pBActive.MouseLeave += new System.EventHandler(this.PbActive_MouseLeave);
            this.pBActive.MouseHover += new System.EventHandler(this.PbActive_MouseHover);
            // 
            // pBInactive
            // 
            this.pBInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBInactive.Image = global::DarkTitleBarEnabler.Properties.Resources.pfeilrauf;
            this.pBInactive.Location = new System.Drawing.Point(248, 82);
            this.pBInactive.Name = "pBInactive";
            this.pBInactive.Size = new System.Drawing.Size(51, 50);
            this.pBInactive.TabIndex = 8;
            this.pBInactive.TabStop = false;
            this.pBInactive.Click += new System.EventHandler(this.PbInactive_Click);
            this.pBInactive.MouseLeave += new System.EventHandler(this.PbInactive_MouseLeave);
            this.pBInactive.MouseHover += new System.EventHandler(this.PbInactive_MouseHover);
            // 
            // pBReg
            // 
            this.pBReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBReg.Image = global::DarkTitleBarEnabler.Properties.Resources.registry;
            this.pBReg.Location = new System.Drawing.Point(152, 85);
            this.pBReg.Name = "pBReg";
            this.pBReg.Size = new System.Drawing.Size(78, 75);
            this.pBReg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBReg.TabIndex = 9;
            this.pBReg.TabStop = false;
            this.pBReg.MouseLeave += new System.EventHandler(this.PbReg_MouseLeave);
            this.pBReg.MouseHover += new System.EventHandler(this.PbReg_MouseHover);
            // 
            // checkHelp
            // 
            this.checkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkHelp.AutoSize = true;
            this.checkHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.checkHelp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.checkHelp.BorderColorChecked = System.Drawing.Color.Yellow;
            this.checkHelp.BorderThikness = 1F;
            this.checkHelp.CheckBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.checkHelp.CheckColor = System.Drawing.Color.White;
            this.checkHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkHelp.ForeColor = System.Drawing.Color.White;
            this.checkHelp.Location = new System.Drawing.Point(12, 180);
            this.checkHelp.Name = "checkHelp";
            this.checkHelp.Padding = new System.Windows.Forms.Padding(3);
            this.checkHelp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkHelp.Size = new System.Drawing.Size(81, 23);
            this.checkHelp.TabIndex = 11;
            this.checkHelp.Text = "Show Help";
            this.checkHelp.UseVisualStyleBackColor = false;
            this.checkHelp.CheckedChanged += new System.EventHandler(this.CheckBoxHelp_CheckedChanged);
            // 
            // numUpDown
            // 
            this.numUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.numUpDown.BorderColor = System.Drawing.Color.White;
            this.numUpDown.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.numUpDown.BorderThikness = 2.5F;
            this.numUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUpDown.ForeColor = System.Drawing.Color.White;
            this.numUpDown.Location = new System.Drawing.Point(355, 42);
            this.numUpDown.MouseHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.numUpDown.Name = "numUpDown";
            this.numUpDown.Size = new System.Drawing.Size(93, 35);
            this.numUpDown.TabIndex = 5;
            this.numUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numUpDown.ValueChangedBorderColor = System.Drawing.Color.Yellow;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.buttonApply.BorderColor = System.Drawing.Color.White;
            this.buttonApply.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.buttonApply.BorderThikness = 2.5F;
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApply.ForeColor = System.Drawing.Color.White;
            this.buttonApply.Location = new System.Drawing.Point(12, 209);
            this.buttonApply.MouseHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(320, 62);
            this.buttonApply.TabIndex = 10;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            this.buttonApply.MouseLeave += new System.EventHandler(this.ButtonApply_MouseLeave);
            this.buttonApply.MouseHover += new System.EventHandler(this.ButtonApply_MouseHover);
            // 
            // textBoxInactive
            // 
            this.textBoxInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInactive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.textBoxInactive.BorderColor = System.Drawing.Color.White;
            this.textBoxInactive.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.textBoxInactive.BorderThikness = 2.5F;
            this.textBoxInactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxInactive.ForeColor = System.Drawing.Color.White;
            this.textBoxInactive.Location = new System.Drawing.Point(199, 41);
            this.textBoxInactive.MaxLength = 8;
            this.textBoxInactive.MouseHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.textBoxInactive.Name = "textBoxInactive";
            this.textBoxInactive.Size = new System.Drawing.Size(133, 35);
            this.textBoxInactive.TabIndex = 4;
            this.textBoxInactive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxInactive.TextChangedBorderColor = System.Drawing.Color.Yellow;
            this.textBoxInactive.MouseLeave += new System.EventHandler(this.TextBoxInactive_MouseLeave);
            this.textBoxInactive.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TextBoxInactive_MouseHover);
            // 
            // textBoxActive
            // 
            this.textBoxActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.textBoxActive.BorderColor = System.Drawing.Color.White;
            this.textBoxActive.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.textBoxActive.BorderThikness = 2.5F;
            this.textBoxActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxActive.ForeColor = System.Drawing.Color.White;
            this.textBoxActive.Location = new System.Drawing.Point(30, 41);
            this.textBoxActive.MaxLength = 8;
            this.textBoxActive.MouseHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.textBoxActive.Name = "textBoxActive";
            this.textBoxActive.Size = new System.Drawing.Size(133, 35);
            this.textBoxActive.TabIndex = 3;
            this.textBoxActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxActive.TextChangedBorderColor = System.Drawing.Color.Yellow;
            this.textBoxActive.MouseLeave += new System.EventHandler(this.TextBoxActive_MouseLeave);
            this.textBoxActive.MouseHover += new System.EventHandler(this.TextBoxActive_MouseHover);
            // 
            // checkHackOnStart
            // 
            this.checkHackOnStart.AutoSize = true;
            this.checkHackOnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.checkHackOnStart.BorderColor = System.Drawing.Color.White;
            this.checkHackOnStart.BorderColorChecked = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.checkHackOnStart.BorderThikness = 1F;
            this.checkHackOnStart.CheckBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.checkHackOnStart.CheckColor = System.Drawing.Color.Yellow;
            this.checkHackOnStart.ForeColor = System.Drawing.Color.White;
            this.checkHackOnStart.Location = new System.Drawing.Point(102, 180);
            this.checkHackOnStart.Name = "checkHackOnStart";
            this.checkHackOnStart.Padding = new System.Windows.Forms.Padding(3);
            this.checkHackOnStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkHackOnStart.Size = new System.Drawing.Size(96, 23);
            this.checkHackOnStart.TabIndex = 12;
            this.checkHackOnStart.Text = "Hack on start";
            this.checkHackOnStart.UseVisualStyleBackColor = true;
            this.checkHackOnStart.CheckedChanged += new System.EventHandler(this.CheckHackOnStart_CheckedChanged);
            // 
            // checkRunWatcherOnStart
            // 
            this.checkRunWatcherOnStart.AutoSize = true;
            this.checkRunWatcherOnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.checkRunWatcherOnStart.BorderColor = System.Drawing.Color.White;
            this.checkRunWatcherOnStart.BorderColorChecked = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.checkRunWatcherOnStart.BorderThikness = 1F;
            this.checkRunWatcherOnStart.CheckBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.checkRunWatcherOnStart.CheckColor = System.Drawing.Color.Yellow;
            this.checkRunWatcherOnStart.ForeColor = System.Drawing.Color.White;
            this.checkRunWatcherOnStart.Location = new System.Drawing.Point(199, 180);
            this.checkRunWatcherOnStart.Name = "checkRunWatcherOnStart";
            this.checkRunWatcherOnStart.Padding = new System.Windows.Forms.Padding(3);
            this.checkRunWatcherOnStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkRunWatcherOnStart.Size = new System.Drawing.Size(134, 23);
            this.checkRunWatcherOnStart.TabIndex = 13;
            this.checkRunWatcherOnStart.Text = "Run Watcher on start";
            this.checkRunWatcherOnStart.UseVisualStyleBackColor = true;
            this.checkRunWatcherOnStart.CheckedChanged += new System.EventHandler(this.CheckRunWatcherOnStart_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(471, 285);
            this.Controls.Add(this.checkRunWatcherOnStart);
            this.Controls.Add(this.checkHackOnStart);
            this.Controls.Add(this.checkHelp);
            this.Controls.Add(this.numUpDown);
            this.Controls.Add(this.pBReg);
            this.Controls.Add(this.pBInactive);
            this.Controls.Add(this.pBActive);
            this.Controls.Add(this.pBDarkTheme);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.textBoxInactive);
            this.Controls.Add(this.textBoxActive);
            this.Controls.Add(this.labelInterval);
            this.Controls.Add(this.labelInactive);
            this.Controls.Add(this.labelActive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBDarkTheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBInactive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelActive;
        private System.Windows.Forms.Label labelInactive;
        private System.Windows.Forms.Label labelInterval;
        private ColorTextBox textBoxActive;
        private ColorTextBox textBoxInactive;
        private ColorButton buttonApply;
        private System.Windows.Forms.PictureBox pBDarkTheme;
        private System.Windows.Forms.PictureBox pBActive;
        private System.Windows.Forms.PictureBox pBInactive;
        private System.Windows.Forms.PictureBox pBReg;
        private ColorNumUpDown numUpDown;
        private ColorCheckBox checkHelp;
        private ColorCheckBox checkHackOnStart;
        private ColorCheckBox checkRunWatcherOnStart;
    }
}