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
    public partial class Form2 : Form
    {
        public string picturePath;

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        public Form2()
        {
            InitializeComponent();
            picturePath = File.ReadAllText(Application.StartupPath + "/data/active.txt");
            CrosshairPicture.Image = Image.FromFile(picturePath);
        }
        
        private void Form2_Load(object sender, EventArgs e)
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
    }
}
