using Keygen.Properties;
using System;
using System.IO;
using System.Text;
using System.Threading;
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
            if (NameTextBox.Text.Length < 3)
            {
                KeyTextBox.Text = "Name should have at least 3 characters!";
                return;
            }

            var key = new char[28];
            for (int i = 0; i < 28; i++)
            {
                key[i] = RandomChar();
            }

            var begin = _random.Next(2, 13);
            var beginStr = begin.ToString().PadLeft(2, '0');

            key[0] = beginStr[0];
            key[1] = beginStr[1];

            var secondLetter = ((int)NameTextBox.Text[0]).ToString("X").PadLeft(2, '0');
            var thirdLetter = ((int)NameTextBox.Text[1]).ToString("X").PadLeft(2, '0');
            var lastLetter = ((int)NameTextBox.Text[NameTextBox.Text.Length - 1]).ToString("X").PadLeft(2, '0');

            key[begin] = secondLetter[0];
            key[begin + 1] = secondLetter[1];

            key[begin * 2] = thirdLetter[0];
            key[begin * 2 + 1] = thirdLetter[1];

            key[27] = lastLetter[1];
            key[26] = lastLetter[0];

            var sb = new StringBuilder();

            for (int i = 0; i < key.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    sb.Append('-');
                }
                sb.Append(key[i]);
            }

            KeyTextBox.Text = sb.ToString();
        }

        private char RandomChar()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return chars[_random.Next(chars.Length)];
        }
    }
}
