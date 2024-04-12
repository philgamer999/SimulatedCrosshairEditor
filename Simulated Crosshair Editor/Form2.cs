using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulated_Crosshair_Editor
{
    public partial class CrosshairWindow : Form
    {
        public string picturePath;

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        MainWindow mainWindow = new MainWindow();

        public CrosshairWindow()
        {
            this.BackColor = Color.FromArgb(1, 1, 1);
            this.AllowTransparency = true;
            this.TransparencyKey = Color.FromArgb(1, 1, 1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetDesktopLocation(0, 0);
            this.TopMost = true;

            InitializeComponent();
            picturePath = File.ReadAllText(mainWindow.libraryPath + "/active.txt");
            CrosshairPicture.Image = Image.FromFile(picturePath);
        }
        
        private void CrosshairWindow_Load(object sender, EventArgs e)
        {
            SetWindowStyle();
        }

        private void SetWindowStyle()
        {
            IntPtr hwnd = this.Handle;
            int currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            // Add WS_EX_LAYERED and WS_EX_TRANSPARENT styles
            SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }

        public void Exit()
        {
            this.Close();
        }
    }
}
