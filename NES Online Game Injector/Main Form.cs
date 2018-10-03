using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace NES_Online_Game_Injector
{
    public partial class Form1 : MaterialForm
    {
        Nvntexpkg Nconvert;
        Zdrop Zconvert;

        public Form1()
        {
            InitializeComponent();
            Nconvert = new Nvntexpkg();
            Zconvert = new Zdrop();
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.Red500, Accent.Red100, TextShade.WHITE);
        }
        private void InjectCompleted()
        {
            MessageBox.Show("Inject!", "Have fun ;)", MessageBoxButtons.OK);
        }

        private void GameBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog GameBrowse = new OpenFileDialog();
            GameBrowse.Title = "Browse for NES Game";
            GameBrowse.Filter = "NES File (*.nes*)|*.nes*";
            GameBrowse.FilterIndex = 1;

            if (GameBrowse.ShowDialog() == DialogResult.OK)
                GamepathTextbox.Text = GameBrowse.FileName;
        }

        private void Coverpath1BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Coverpath1Browse = new OpenFileDialog();
            Coverpath1Browse.Title = "Browse for Cover";
            Coverpath1Browse.Filter = "TGA File (*.tga*)|*.tga*";
            Coverpath1Browse.FilterIndex = 1;

            if (Coverpath1Browse.ShowDialog() == DialogResult.OK)
                Coverpath1Textbox.Text = Coverpath1Browse.FileName;
        }

        private void Coverpath2BrowseButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog Coverpath2Browse = new OpenFileDialog();
            Coverpath2Browse.Title = "Browse for Cover";
            Coverpath2Browse.Filter = "TGA File (*.tga*)|*.tga*";
            Coverpath2Browse.FilterIndex = 1;

            if (Coverpath2Browse.ShowDialog() == DialogResult.OK)
                Coverpath2Textbox.Text = Coverpath2Browse.FileName;
        }

        private void TitledbBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog TitledbBrowse = new OpenFileDialog();
            TitledbBrowse.Title = "Browse for Title DB";
            TitledbBrowse.Filter = "titlesdb File (*.titlesdb*)|*.titlesdb*";
            TitledbBrowse.FilterIndex = 1;

            if (TitledbBrowse.ShowDialog() == DialogResult.OK)
                TitledbTextbox.Text = TitledbBrowse.FileName;
        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            if (SortTitleTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the SortTitle!", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (PublisherTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Publisher!", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (GamecodeTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Game Code!", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (GamecodeTextbox.Text.Length < 5 || GamecodeTextbox.Text.Length > 5)
            {
                MessageBox.Show("Enter the unique Game Code (AAAAA-ZZZZZ)", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (CopyrightTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Copyright!", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (GametitleTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Game Title!", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (OverscanTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Overscan", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (SimuTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Simultanus", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (FadeinTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Fade In", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (VolumeTextbox.Text.Length < 1 || VolumeTextbox.Text.Length > 2)
            {
                MessageBox.Show("Enter the Volume (1-99)", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (VolumeTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter the Volume", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (GamepathTextbox.Text == string.Empty)
            {
                MessageBox.Show("Invalid Game path", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (Coverpath1Textbox.Text == string.Empty)
            {
                MessageBox.Show("Invalid Cover path 400x300", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (Coverpath2Textbox.Text == string.Empty)
            {
                MessageBox.Show("Invalid Cover path 355x512", "Error.", MessageBoxButtons.OK);
                return;
            }
            else if (TitledbTextbox.Text == string.Empty)
            {
                MessageBox.Show("Invalid Title DB path", "Error.", MessageBoxButtons.OK);
                return;
            }

            File.Move(@Coverpath1Textbox.Text, "cover.tga");
            File.Move(@Coverpath2Textbox.Text, "screenshot.tga");

            Nconvert.RunCommand($"-i cover.tga -o cover.xtx --mip-filter box --minmip 5 -f rgba8");
            Nconvert.RunCommand($"-i screenshot.tga -o screenshot.xtx --mip-filter box --minmip 5 -f rgba8");
            Zconvert.RunCommand($"cover.xtx");

            Directory.CreateDirectory("NES_ONLINE_Mod");
            Directory.CreateDirectory("NES_ONLINE_Mod/romfs");
            Directory.CreateDirectory("NES_ONLINE_Mod/romfs/titles");
            Directory.CreateDirectory("NES_ONLINE_Mod/romfs/titles/" + GamecodeTextbox.Text);

            File.Move(@"cover.xtx.zlib", "NES_ONLINE_Mod/romfs/titles/" + GamecodeTextbox.Text + "\\" + GamecodeTextbox.Text + ".xtx.z");
            File.Delete(@"cover.xtx");
            File.Delete(@"cover.tga");

            Zconvert.RunCommand($"screenshot.xtx");
            File.Move(@"screenshot.xtx.zlib", "NES_ONLINE_Mod/romfs/titles/" + GamecodeTextbox.Text + "\\" + GamecodeTextbox.Text + "00.xtx.z");
            File.Delete(@"screenshot.xtx");
            File.Delete(@"screenshot.tga");

            File.Move(@GamepathTextbox.Text, "NES_ONLINE_Mod/romfs/titles/" + GamecodeTextbox.Text + "\\" + GamecodeTextbox.Text + ".nes");

            File.Move(@TitledbTextbox.Text, "NES_ONLINE_Mod/romfs/titles/lclassics.titlesdb");

            string filecheck = "NES_ONLINE_Mod/romfs/titles/lclassics.titlesdb";
            if (File.Exists(filecheck))
            {
                var lines = File.ReadAllLines("NES_ONLINE_Mod/romfs/titles/lclassics.titlesdb");
                File.WriteAllLines("NES_ONLINE_Mod/romfs/titles/lclassics.titlesdb", lines.Take(lines.Length - 2).ToArray());

                using (StreamWriter db = new FileInfo("NES_ONLINE_Mod/romfs/titles/lclassics.titlesdb").AppendText())
                {
                    db.WriteLine("        ,");
                    db.WriteLine("        {");
                    db.WriteLine("            \"sort_title\": \"" + SortTitleTextbox.Text + "\",");
                    db.WriteLine("            \"publisher\": \"" + PublisherTextbox.Text + "\",");
                    db.WriteLine("            \"code\": \"CLV-H-" + GamecodeTextbox.Text + "\",");
                    db.WriteLine("            \"rom\": \"/titles/CLV-H-" + GamecodeTextbox.Text + "/CLV-H-" + GamecodeTextbox.Text + ".nes\",");
                    db.WriteLine("            \"copyright\": \"" + CopyrightTextbox.Text + "\",");
                    db.WriteLine("            \"title\": \"" + GametitleTextbox.Text + "\",");
                    db.WriteLine("            \"volume\": " + VolumeTextbox.Text + ",");
                    db.WriteLine("            \"release_date\": \"1987-12-01\",");
                    db.WriteLine("            \"players_count\": 1,");
                    db.WriteLine("            \"cover\": \"/titles/CLV-H-" + GamecodeTextbox.Text + "/CLV-H-"+ GamecodeTextbox.Text + ".xtx.z,");
                    db.WriteLine("            \"overscan\": [" + OverscanTextbox.Text + "],");
                    db.WriteLine("            \"armet_version\": \"v1\",");
                    db.WriteLine("            \"lcla6_release_date\": \"2018-09-01\",");
                    db.WriteLine("            \"save_count\": 0,");
                    db.WriteLine("            \"simultaneous\":" + SimuTextbox.Text + ",");
                    db.WriteLine("            \"fadein\": [" + FadeinTextbox.Text + "],");
                    db.WriteLine("            \"details_screen\": \"\",");
                    db.WriteLine("            \"armet_threshold\": 80,");
                    db.WriteLine("            \"sort_publisher\": \"" + PublisherTextbox.Text + "\"");
                    db.WriteLine("        }");
                    db.WriteLine("    ]");
                    db.WriteLine("}");
                    db.Close();
                }
            }

            InjectCompleted();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/8mNFFcC");
        }
    }
}
