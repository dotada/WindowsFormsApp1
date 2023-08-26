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
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
using System.Net;
using System.IO.Compression;

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
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{textBox3.Text}\\{sanitizedtitle}.{streamInfo.Container}");
                Form7 done = new Form7();
                done.ShowDialog();
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 readme = new Form3();
            readme.ShowDialog();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (Uri.IsWellFormedUriString(textBox2.Text, UriKind.Absolute))
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "DYVD");
                string ffmpegloc = Path.Combine(specificFolder, "ffmpeg-master-latest-win64-gpl-shared");
                string ffmpegclidir = Path.Combine(ffmpegloc, "bin");
                string ffmpegcli = Path.Combine(ffmpegclidir, "ffmpeg.exe");
                if (Directory.Exists(ffmpegclidir))
                {
                    var youtube = new YoutubeClient();
                    var videourl = textBox2.Text;
                    var video = await youtube.Videos.GetAsync(videourl);
                    string sanitizedtitle = SanitizeFileName(video.Title);
                    var filepath = $"{textBox3.Text}\\{sanitizedtitle}.mp4";
                    await youtube.Videos.DownloadAsync(videourl, filepath, o => o
                        .SetContainer("mp4")
                        .SetFFmpegPath(ffmpegcli)
                    );
                    Form7 done = new Form7();
                    done.ShowDialog();
                } else
                {
                    Form5 form5 = new Form5();
                    form5.ShowDialog();
                }
            } else {
                Form2 invalidurl = new Form2();
                invalidurl.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "DYVD");
            string ffmpegloc = Path.Combine(specificFolder, "ffmpeg-master-latest-win64-gpl-shared");
            string ffmpegdw = Path.Combine(specificFolder, "ffmpeg-master-latest-win64-gpl-shared.zip");
            if (Directory.Exists(ffmpegloc))
            {
                Form4 ffmpeginstalled = new Form4();
                ffmpeginstalled.ShowDialog();
            } else
            {
                Directory.CreateDirectory(specificFolder);
                WebClient webc = new WebClient();
                webc.DownloadFile("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl-shared.zip", ffmpegdw);
                ZipFile.ExtractToDirectory(ffmpegdw, specificFolder);
                Form6 form6 = new Form6();
                form6.ShowDialog();
            }
            
        }
    }
}
