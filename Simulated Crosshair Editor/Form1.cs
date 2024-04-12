using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestStack.White.WindowsAPI;
using WindowsInput;
using WindowsInput.Native;

namespace Simulated_Crosshair_Editor
{
    public partial class MainWindow : Form
    {
        string appPath = "";
        string dataPath = "";
        public string libraryPath = "";
        string libraryGenPath = "";
        string libraryImgPath = "";
        public string crosshairWindowPath = "";

        bool form2open;

        CrosshairWindow crosshairWindow;
        Bitmap bmEdit;
        Graphics graphicsBMEdit;
        bool editingCrosshair = false;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        int index = 1;
        string rHEX = "FF";
        string gHEX = "FF";
        string bHEX = "FF";
        int paintAlpha = 255;
        Color paintingColorBase = Color.Black;
        Color paintingColor = Color.Black;
        List<char> rgbValues = new List<char>();
        bool hexChanging;

        public MainWindow()
        {
            InitializeComponent();
            CompanyLabel.Text = Application.CompanyName;
            VersionLabel.Text = "v:" + Application.ProductVersion;

            LibraryPanel.Visible = true;
            LibraryPanel.Enabled = true;

            Paths();

            GetLibraryItems();

            Bitmap tempCrosshair = (Bitmap)Bitmap.FromFile(File.ReadAllText(libraryPath + "/active.txt"));
            Bitmap corsshairShowcase = new Bitmap(tempCrosshair, 95, 95);
            CrosshairPicture.Image = corsshairShowcase;

            bmEdit = new Bitmap(233, 233);
            graphicsBMEdit = Graphics.FromImage(bmEdit);
            graphicsBMEdit.Clear(Color.Transparent);
            EditPanelViewCrosshairPicture.Image = bmEdit;
        }

        private void Paths()
        {
            appPath = Application.StartupPath;
            dataPath = appPath + "/data";
            libraryPath = dataPath + "/library";
            libraryGenPath = libraryPath + "/gen";
            libraryImgPath = libraryPath + "/img";
            crosshairWindowPath = dataPath + "/crosshair_window/CrosshairWindow.exe";
            if (!File.Exists(libraryPath + "/active.txt"))
            {
                File.Create(libraryPath + "/active.txt");
                File.WriteAllText(libraryPath + "/active.txt", libraryImgPath + "/test_63x.png");
                Application.Exit();
            }
            else if (File.ReadAllText(libraryPath + "/active.txt") == "")
            {
                File.WriteAllText(libraryPath + "/active.txt", libraryImgPath + "/test_63x.png");
                Application.Exit();
            }

            if (!File.Exists(File.ReadAllText(libraryPath + "/active.txt")))
            {
                File.WriteAllText(libraryPath + "/active.txt", "");
                Application.Exit();
            }
        }

        private void GetLibraryItems()
        {
            LibraryFlowLayoutPanel.Controls.Clear();
            EditItemSelected.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(libraryImgPath);

            string[] files = Directory.GetFiles(libraryImgPath);

            foreach (string filePath in files)
            {
                Button libraryBtn = new Button();
                libraryBtn.Size = new Size(100, 100);
                libraryBtn.BackColor = Color.FromArgb(25, 25, 25);
                libraryBtn.ForeColor = Color.FromArgb(100, 100, 100);
                libraryBtn.FlatStyle = FlatStyle.Popup;
                libraryBtn.TextAlign = ContentAlignment.BottomCenter;
                libraryBtn.Text = Path.GetFileNameWithoutExtension(filePath);
                libraryBtn.Click += new EventHandler(LibraryButton_Click);
                libraryBtn.Image = Image.FromFile(filePath);
                LibraryFlowLayoutPanel.Controls.Add(libraryBtn);
                EditItemSelected.Items.Add(libraryBtn.Text);
            }
        }

        private void LibraryButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            File.WriteAllText(libraryPath + "/active.txt", libraryImgPath + "/" + clickedButton.Text + ".png");
            Bitmap tempCrosshair = (Bitmap)Bitmap.FromFile(libraryImgPath + "/" + clickedButton.Text + ".png");
            Bitmap corsshairShowcase = new Bitmap(tempCrosshair, 95, 95);
            CrosshairPicture.Image = corsshairShowcase;
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void ButtonLibrary_Click(object sender, EventArgs e)
        {
            LibraryPanel.Enabled = true;
            LibraryPanel.Visible = true;

            ImportPanel.Enabled = false;
            ImportPanel.Visible = false;

            EditPanel.Enabled = false;
            EditPanel.Visible = false;
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            LibraryPanel.Enabled = false;
            LibraryPanel.Visible = false;

            ImportPanel.Enabled = true;
            ImportPanel.Visible = true;

            EditPanel.Enabled = false;
            EditPanel.Visible = false;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            LibraryPanel.Enabled = false;
            LibraryPanel.Visible = false;

            ImportPanel.Enabled = false;
            ImportPanel.Visible = false;

            EditPanel.Enabled = true;
            EditPanel.Visible = true;
        }

        private void ButtonSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                saveExist:
                if (EditPanelViewConfigsFileName.Text != "")
                {
                    if (!File.Exists(libraryImgPath + "/" + EditPanelViewConfigsFileName.Text + ".png"))
                    {
                        bmEdit.Save(libraryImgPath + "/" + EditPanelViewConfigsFileName.Text + ".png", ImageFormat.Png);

                        goto Saved;
                    }
                    else if (EditPanelViewConfigsFileName.Text != "" && File.Exists(libraryImgPath + "/" + EditPanelViewConfigsFileName.Text + "(" + i + ").png"))
                    {
                        i++;
                        goto saveExist;
                    }
                    bmEdit.Save(libraryImgPath + "/" + EditPanelViewConfigsFileName.Text + "(" + i + ").png", ImageFormat.Png);
                }
                else
                {
                    if (!File.Exists(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + ".png"))
                    {
                        bmEdit.Save(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + ".png", ImageFormat.Png);

                        goto Saved;
                    }
                    else if (File.Exists(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + "(" + i + ").png"))
                    {
                        i++;
                        goto saveExist;
                    }
                    bmEdit.Save(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + "(" + i + ").png", ImageFormat.Png);
                }
                
                Saved:
                GetLibraryItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonAddEdit_Click(object sender, EventArgs e)
        {
            int i = 0;

            newCrosshairExists:
            if (!File.Exists(libraryImgPath + "/new_crosshair.png"))
            {
                File.Copy(dataPath + "/crosshair_window/NewCrosshair.png", libraryImgPath + "/new_crosshair.png");

                goto Saved;
            }
            if (File.Exists(libraryImgPath +"/new_crosshair(" + i + ").png"))
            {
                i++;
                goto newCrosshairExists;
            }
            File.Copy(dataPath + "/crosshair_window/NewCrosshair.png", libraryImgPath + "/new_crosshair(" + i + ").png");

            Saved:
            GetLibraryItems();
        }

        private void ButtonDeleteEdit_Click(object sender, EventArgs e)
        {
            if (!editingCrosshair)
            {
                bmEdit = new Bitmap(233,233);
                graphicsBMEdit.Clear(Color.Transparent);
                EditPanelViewCrosshairPicture.Image = bmEdit;
                try
                {
                    File.Delete(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + ".png");
                    GetLibraryItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            List<string> fileContent = new List<string>();
            string filePath = string.Empty;
            string fileName = string.Empty;

            ImportFileDialog.InitialDirectory = "c:\\";
            ImportFileDialog.Filter = "TXT Files (*.txt)|*.txt|PNG Files (*.png)|*.png";
            ImportFileDialog.FilterIndex = 2;
            ImportFileDialog.RestoreDirectory = true;

            if (ImportFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (ImportFileDialog.FileName.EndsWith(".txt"))
                {
                    filePath = ImportFileDialog.FileName;
                    fileName = Path.GetFileName(filePath);
                    fileContent.AddRange(File.ReadAllLines(filePath));

                    using (StreamWriter writer = new StreamWriter(libraryGenPath + "/" + fileName))
                    {
                        for (int i = 0; i < fileContent.Count; i++)
                        {
                            writer.WriteLine(fileContent[i]);
                        }
                    }
                }
                else if (ImportFileDialog.FileName.EndsWith(".png"))
                {
                    MessageBox.Show(".png");
                    filePath = ImportFileDialog.FileName;
                    fileName = Path.GetFileName(filePath);

                    File.Copy(filePath, libraryImgPath + "/" + fileName);
                }

                GetLibraryItems();
            }
        }

        private void EditPanelViewConfigsEditMode_CheckStateChanged(object sender, EventArgs e)
        {
            if (EditPanelViewConfigsEditMode.CheckState == CheckState.Checked)
            {
                editingCrosshair = true;
            }
            else if (EditPanelViewConfigsEditMode.CheckState == CheckState.Unchecked)
            {
                editingCrosshair = false;
            }
        }

        private void EditPanelViewCrosshairPicture_MouseDown(object sender, MouseEventArgs e)
        {
            if (editingCrosshair)
            {
                paint = true;


                double dx1 = e.Location.X;
                double dy1 = e.Location.Y;

                float fx1 = (float)Math.Round(dx1 / 233 * bmEdit.Width);
                float fy1 = (float)Math.Round(dy1 / 233 * bmEdit.Width);

                py.X = Convert.ToInt32(fx1);
                py.Y = Convert.ToInt32(fy1);
            }
        }

        private void EditPanelViewCrosshairPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (editingCrosshair)
            {
                if (paint)
                {
                    if (index == 1)
                    {
                        double dx1 = e.Location.X;
                        double dy1 = e.Location.Y;

                        float fx1 = (float)Math.Round(dx1 / 233 * bmEdit.Width);
                        float fy1 = (float)Math.Round(dy1 / 233 * bmEdit.Width);

                        px.X = Convert.ToInt32(fx1);
                        px.Y = Convert.ToInt32(fy1);

                        graphicsBMEdit.DrawLine(p, px, py);
                        py = px;
                    }
                }
                EditPanelViewCrosshairPicture.Refresh();
            }
        }

        private void EditPanelViewCrosshairPicture_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
        }

        private void EditItemSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EditItemSelected.SelectedIndex != -1)
            {
                bmEdit = (Bitmap)Bitmap.FromFile(libraryImgPath + "/" + EditItemSelected.SelectedItem.ToString() + ".png");
                graphicsBMEdit = Graphics.FromImage(bmEdit);
                EditPanelViewCrosshairPicture.Image = bmEdit;
                EditPanelViewCrosshairPicture.Refresh();
                EditPanelViewConfigsFileName.Text = EditItemSelected.SelectedItem.ToString();
            }
        }

        private void EditRedNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!hexChanging)
            {
                rHEX = Int32.Parse(EditRedNumeric.Value.ToString()).ToString("X");
                gHEX = Int32.Parse(EditGreenNumeric.Value.ToString()).ToString("X");
                bHEX = Int32.Parse(EditBlueNumeric.Value.ToString()).ToString("X");
                EditHEXTextbox.Text = rHEX + gHEX + bHEX;
                paintingColorBase = Color.FromArgb(Int32.Parse(EditRedNumeric.Value.ToString()), Int32.Parse(EditGreenNumeric.Value.ToString()), Int32.Parse(EditBlueNumeric.Value.ToString()));
                paintAlpha = Int32.Parse(EditAlphaNumeric.Value.ToString());
                paintingColor = Color.FromArgb(paintAlpha, paintingColorBase);
                p.Color = paintingColor;
                SelectedColorPicture.BackColor = paintingColor;
            }
        }

        private void EditGreenNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!hexChanging)
            {
                rHEX = Int32.Parse(EditRedNumeric.Value.ToString()).ToString("X");
                gHEX = Int32.Parse(EditGreenNumeric.Value.ToString()).ToString("X");
                bHEX = Int32.Parse(EditBlueNumeric.Value.ToString()).ToString("X");
                EditHEXTextbox.Text = rHEX + gHEX + bHEX;
                paintingColorBase = Color.FromArgb(Int32.Parse(EditRedNumeric.Value.ToString()), Int32.Parse(EditGreenNumeric.Value.ToString()), Int32.Parse(EditBlueNumeric.Value.ToString()));
                paintAlpha = Int32.Parse(EditAlphaNumeric.Value.ToString());
                paintingColor = Color.FromArgb(paintAlpha, paintingColorBase);
                p.Color = paintingColor;
                SelectedColorPicture.BackColor = paintingColor;
            }
        }

        private void EditBlueNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!hexChanging)
            {
                rHEX = Int32.Parse(EditRedNumeric.Value.ToString()).ToString("X");
                gHEX = Int32.Parse(EditGreenNumeric.Value.ToString()).ToString("X");
                bHEX = Int32.Parse(EditBlueNumeric.Value.ToString()).ToString("X");
                EditHEXTextbox.Text = rHEX + gHEX + bHEX;
                paintingColorBase = Color.FromArgb(Int32.Parse(EditRedNumeric.Value.ToString()), Int32.Parse(EditGreenNumeric.Value.ToString()), Int32.Parse(EditBlueNumeric.Value.ToString()));
                paintAlpha = Int32.Parse(EditAlphaNumeric.Value.ToString());
                paintingColor = Color.FromArgb(paintAlpha, paintingColorBase);
                p.Color = paintingColor;
                SelectedColorPicture.BackColor = paintingColor;
            }
        }

        private void EditAlphaNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!hexChanging)
            {
                if (EditAlphaNumeric.Value == 0)
                {
                    p.Color = Color.Transparent;
                }
                else
                {
                    rHEX = Int32.Parse(EditRedNumeric.Value.ToString()).ToString("X");
                    gHEX = Int32.Parse(EditGreenNumeric.Value.ToString()).ToString("X");
                    bHEX = Int32.Parse(EditBlueNumeric.Value.ToString()).ToString("X");
                    EditHEXTextbox.Text = rHEX + gHEX + bHEX;
                    paintingColorBase = Color.FromArgb(Int32.Parse(EditRedNumeric.Value.ToString()), Int32.Parse(EditGreenNumeric.Value.ToString()), Int32.Parse(EditBlueNumeric.Value.ToString()));
                    paintAlpha = Int32.Parse(EditAlphaNumeric.Value.ToString());
                    paintingColor = Color.FromArgb(paintAlpha, paintingColorBase);
                    p.Color = paintingColor;
                    SelectedColorPicture.BackColor = paintingColor;
                }
            }
        }

        private void EditHEXTextbox_TextChanged(object sender, EventArgs e)
        {
            hexChanging = true;
            rgbValues.Clear();
            rgbValues.AddRange(EditHEXTextbox.Text);
            if (rgbValues.Count == 6)
            {
                try
                {
                    rHEX = rgbValues[0].ToString() + rgbValues[1].ToString();
                    gHEX = rgbValues[2].ToString() + rgbValues[3].ToString();
                    bHEX = rgbValues[4].ToString() + rgbValues[5].ToString();

                    EditRedNumeric.Value = int.Parse(rHEX, System.Globalization.NumberStyles.HexNumber);
                    EditGreenNumeric.Value = int.Parse(gHEX, System.Globalization.NumberStyles.HexNumber);
                    EditBlueNumeric.Value = int.Parse(bHEX, System.Globalization.NumberStyles.HexNumber);

                    paintingColorBase = Color.FromArgb(Int32.Parse(EditRedNumeric.Value.ToString()), Int32.Parse(EditGreenNumeric.Value.ToString()), Int32.Parse(EditBlueNumeric.Value.ToString()));
                    paintAlpha = Int32.Parse(EditAlphaNumeric.Value.ToString());
                    paintingColor = Color.FromArgb(paintAlpha, paintingColorBase);
                    p.Color = paintingColor;
                    SelectedColorPicture.BackColor = paintingColor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            hexChanging = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            GetLibraryItems();
        }

        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            if (!form2open)
            {
                try
                {
                    crosshairWindow = new CrosshairWindow();
                    crosshairWindow.Show();
                    form2open = true;
                    ShowHideButton.Text = "Close";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    crosshairWindow.Exit();
                    form2open = false;
                    ShowHideButton.Text = "Show";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
