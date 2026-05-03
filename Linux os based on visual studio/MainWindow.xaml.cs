using System;
using System.Windows;
using System.Windows.Threading;

namespace LinuxMintOS
{
    public partial class MainWindow : Window
    {
        private FileManagerWindow fileManager;
        private BrowserWindow browser;
        private TerminalWindow terminal;
        private SettingsWindow settings;
        private TextEditorWindow textEditor;
        private DispatcherTimer clockTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += (s, e) => ClockDisplay.Text = DateTime.Now.ToString("HH:mm");
            clockTimer.Start();
        }

        private void FileManager_Click(object sender, RoutedEventArgs e)
        {
            if (fileManager == null || !fileManager.IsVisible)
            {
                fileManager = new FileManagerWindow();
                fileManager.Show();
            }
            else
            {
                fileManager.Focus();
            }
        }

        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            if (browser == null || !browser.IsVisible)
            {
                browser = new BrowserWindow();
                browser.Show();
            }
            else
            {
                browser.Focus();
            }
        }

        private void Terminal_Click(object sender, RoutedEventArgs e)
        {
            if (terminal == null || !terminal.IsVisible)
            {
                terminal = new TerminalWindow();
                terminal.Show();
            }
            else
            {
                terminal.Focus();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (settings == null || !settings.IsVisible)
            {
                settings = new SettingsWindow();
                settings.Show();
            }
            else
            {
                settings.Focus();
            }
        }

        private void TextEditor_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor == null || !textEditor.IsVisible)
            {
                textEditor = new TextEditorWindow();
                textEditor.Show();
            }
            else
            {
                textEditor.Focus();
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to Linux Mint OS Simulator!\n\nAvailable Applications:\n- Files\n- Browser\n- Terminal\n- Settings\n- Text Editor",
                "System Menu", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}