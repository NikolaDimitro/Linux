using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace LinuxMintOS
{
    public partial class SplashScreen : Window
    {
        private DispatcherTimer bootTimer;
        private int bootStage = 0;
        private List<string> bootMessages;

        public SplashScreen()
        {
            InitializeComponent();
            bootMessages = new List<string>
            {
                "Checking system...",
                "Loading kernel...",
                "Initializing drivers...",
                "Setting up display...",
                "Loading desktop environment...",
                "Starting applications...",
                "Boot complete!"
            };

            StartBootAnimation();
        }

        private void StartBootAnimation()
        {
            bootTimer = new DispatcherTimer();
            bootTimer.Interval = TimeSpan.FromMilliseconds(400);
            bootTimer.Tick += BootTimer_Tick;
            bootTimer.Start();
        }

        private void BootTimer_Tick(object sender, EventArgs e)
        {
            if (bootStage < bootMessages.Count)
            {
                StatusText.Text = bootMessages[bootStage];
                BootMessages.Text += bootMessages[bootStage] + "\n";
                LoadingBar.Value = (bootStage + 1) * (100 / bootMessages.Count);
                bootStage++;
            }
            else
            {
                bootTimer.Stop();
                // Open main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}