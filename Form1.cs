using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.Text.RegularExpressions;
using AngleSharp.Common;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        string SanitizeFileName(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (Uri.IsWellFormedUriString(textBox2.Text, UriKind.Absolute)) {
                var youtube = new YoutubeClient();
                var videoUrl = textBox2.Text;
                var video = await youtube.Videos.GetAsync(videoUrl);    
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
                _ = await youtube.Videos.Streams.GetAsync(streamInfo);
                string sanitizedtitle = SanitizeFileName(video.Title);
                Console.WriteLine($"{textBox3.Text}\\{sanitizedtitle}.{streamInfo.Container}");
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{textBox3.Text}\\{sanitizedtitle}.{streamInfo.Container}");
            } else {
                Form2 invalidurl = new Form2();
                invalidurl.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox3.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
