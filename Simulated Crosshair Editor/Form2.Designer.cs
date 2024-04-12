
namespace Simulated_Crosshair_Editor
{
    partial class CrosshairWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrosshairWindow));
            this.CrosshairPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CrosshairPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // CrosshairPicture
            // 
            this.CrosshairPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CrosshairPicture.Enabled = false;
            this.CrosshairPicture.Location = new System.Drawing.Point(15, 30);
            this.CrosshairPicture.Name = "CrosshairPicture";
            this.CrosshairPicture.Size = new System.Drawing.Size(63, 63);
            this.CrosshairPicture.TabIndex = 0;
            this.CrosshairPicture.TabStop = false;
            // 
            // CrosshairWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(93, 93);
            this.Controls.Add(this.CrosshairPicture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CrosshairWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CrosshairWindow";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CrosshairWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CrosshairPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CrosshairPicture;
    }
}