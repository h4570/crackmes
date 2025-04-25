using Keygen.Properties;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Keygen
{
    public partial class Form1 : Form
    {
        private readonly string _musicFilePath = Path.Combine(Path.GetTempPath(), "dreamsound.mp3");
        private readonly WindowsMediaPlayer _player = new WindowsMediaPlayer();
        private readonly Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(_musicFilePath))
                {
                    File.WriteAllBytes(_musicFilePath, Resources.dreamsound);
                }

                _player.URL = _musicFilePath;
                _player.settings.volume = 6;
                _player.controls.play();
            }
            catch { }
        }

        private void Form1_Closing(object sender, EventArgs e)
        {
            try
            {
                _player.controls.stop();
                KeyTextBox.Text = "Disposing audio...";
            }
            catch { }
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            try
            {
                var task = Task.Run(() =>
                {
                    if (File.Exists(_musicFilePath))
                    {
                        File.Delete(_musicFilePath);
                    }
                });

                task.Wait(TimeSpan.FromMilliseconds(333));
            }
            catch { }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text.Length < 1)
            {
                KeyTextBox.Text = "Name should have at least 1 character!";
                return;
            }

            KeyTextBox.Text = NameTextBox.Text.Length.ToString();

            int key2 = 0;

            foreach (var letter in NameTextBox.Text)
            {
                key2 += letter * 1337;
            }

            Key2TextBox.Text = key2.ToString();
            int key3 = 0;

            foreach (var letter in NameTextBox.Text)
            {
                key3 += letter;
            }

            Key3TextBox.Text = key3.ToString();
            ButtonTextBox.Text = "Button -> X:0, Y:1";
        }

        private char RandomChar()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return chars[_random.Next(chars.Length)];
        }
    }
}
