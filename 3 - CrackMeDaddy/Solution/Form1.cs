using Keygen.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            RefreshButton_Click(null, null);
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
            if (ProcessComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var process = Process.GetProcessesByName(ProcessComboBox.Text).FirstOrDefault();
            var processHandle = Memory.OpenProcess(Memory.PROCESS_WM_READ, false, process.Id);

            var buffer = new byte[13];
            int baseAddress = process.MainModule.BaseAddress.ToInt32();

            var offset = 0x21FEFF; // 0x61FEFF - baseAddress
            var addr = (IntPtr)(baseAddress + offset);
            Memory.ReadProcessMemory(processHandle, addr, buffer, buffer.Length, out var bytesRead);

            var key = UTF8Encoding.UTF8.GetString(buffer);
            KeyTextBox.Text = key;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ProcessComboBox.Items.Clear();
            var targetFound = false;

            foreach (Process process in Process.GetProcesses())
            {
                var skipNames = new string[] { "svchost", "conhost", "msedge", "chrome" };
                if (!skipNames.Contains(process.ProcessName))
                {
                    ProcessComboBox.Items.Add(process.ProcessName);
                    if (process.ProcessName == "crackme")
                        targetFound = true;
                }
            }

            if (targetFound)
            {
                var index = ProcessComboBox.Items.IndexOf("crackme");
                ProcessComboBox.SelectedIndex = index;
            }
        }
    }
}
