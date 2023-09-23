using Keygen.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
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
            var encodedUserName = EncodeUsername(UsernameTextBox.Text);

            byte[] key;
            try
            {
                key = BruteForceKey(encodedUserName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var decodedKey = DecodeKey(key);

            var isGeneratedKeyValid = AreEqual(encodedUserName, decodedKey);

            if (!isGeneratedKeyValid)
            {
                MessageBox.Show("Generated key is not valid. Please try other username (don't use numbers).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "NFO file (*.nfo)|*.nfo",
                Title = "Save NFO file",
                FileName = "file.nfo",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            File.WriteAllBytes(saveFileDialog.FileName, key);
        }

        private byte[] EncodeUsername(string username)
        {
            var usernameBytes = username.ToCharArray().Select(c => (byte)c).ToArray();
            var result = new byte[usernameBytes.Length];

            for (int i = 0; i < usernameBytes.Length; i++)
            {
                result[i] = (byte)((usernameBytes[i] << 2 | usernameBytes[i] >> 6) + 0x32 ^ 3);
            }

            return result;
        }

        private byte[] BruteForceKey(byte[] encryptedUsername)
        {
            var bruteForceFrom = short.MinValue;
            var bruteForceTo = short.MaxValue;
            var key = new byte[encryptedUsername.Length];

            for (int i = 0; i < encryptedUsername.Length; i++)
            {
                var characterToFind = encryptedUsername[i];
                var found = false;

                for (int j = bruteForceFrom; j <= bruteForceTo; j++)
                {
                    var p1 = (j ^ 3) - 0x32;
                    var p2 = (byte)(p1 >> 2 | p1 * '@');

                    if (p2 == characterToFind)
                    {
                        found = true;
                        key[i] = (byte)j;
                        break;
                    }
                }

                if (!found)
                {
                    throw new ArgumentException($"Didn't found valid key character for given character with index: {i}. Please try different username.");
                }
            }

            return key;
        }

        private byte[] DecodeKey(byte[] key)
        {
            var result = new byte[key.Count()];

            for (int i = 0; i < key.Count(); i++)
            {
                var part1 = (byte)((key[i] ^ 3) - 0x32);
                result[i] = (byte)(part1 >> 2 | part1 * '@');
            }

            return result;
        }

        private bool AreEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Count(); i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
