namespace DarkTitleBarEnabler {
    partial class Messages {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Messages));
            this.rtbNotify = new DarkTitleBarEnabler.ColorRichTextBox();
            this.SuspendLayout();
            // 
            // rtbNotify
            // 
            this.rtbNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.rtbNotify.BorderColor = System.Drawing.Color.White;
            this.rtbNotify.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.rtbNotify.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbNotify.DetectUrls = false;
            this.rtbNotify.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbNotify.ForeColor = System.Drawing.Color.White;
            this.rtbNotify.Location = new System.Drawing.Point(12, 12);
            this.rtbNotify.MouseHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(255)))));
            this.rtbNotify.Name = "rtbNotify";
            this.rtbNotify.Size = new System.Drawing.Size(776, 426);
            this.rtbNotify.TabIndex = 0;
            this.rtbNotify.Text = "";
            this.rtbNotify.TextChangedBorderColor = System.Drawing.Color.Yellow;
            // 
            // Messages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbNotify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Messages";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Messages";
            this.Load += new System.EventHandler(this.Messages_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private ColorRichTextBox rtbNotify;
    }
}