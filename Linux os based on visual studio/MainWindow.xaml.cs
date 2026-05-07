using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var assetsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            var fallbackIconPath = Path.Combine(assetsDir, "browser.png");
            foreach (var button in DesktopAppsPanel.Children.OfType<Button>())
            {
                var image = (button.Content as StackPanel)?.Children.OfType<Image>().FirstOrDefault();
                var sourceText = image?.Source?.ToString() ?? string.Empty;
                if (image != null && string.IsNullOrWhiteSpace(sourceText))
                {
                    if (File.Exists(fallbackIconPath))
                    {
                        image.Source = new BitmapImage(new Uri(fallbackIconPath, UriKind.Absolute));
                    }
                }
            }

            var taskbarBg = (TaskbarGrid.Background as SolidColorBrush)?.Color.ToString() ?? "non-solid";
            var taskbarBorderBg = (TaskbarBorder.Background as SolidColorBrush)?.Color.ToString() ?? "non-solid";
            var taskbarBorderStroke = (TaskbarBorder.BorderBrush as SolidColorBrush)?.Color.ToString() ?? "non-solid";
            var blurRadius = (TaskbarBorder.Effect as BlurEffect)?.Radius ?? -1d;
            var clockColor = (ClockDisplay.Foreground as SolidColorBrush)?.Color.ToString() ?? "non-solid";

            // #region agent log
            DebugLog(
                "baseline-readability",
                "H7",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Taskbar color and opacity values",
                $"{{\"taskbarGrid\":\"{EscapeForJson(taskbarBg)}\",\"taskbarBorderBackground\":\"{EscapeForJson(taskbarBorderBg)}\",\"taskbarBorderBrush\":\"{EscapeForJson(taskbarBorderStroke)}\"}}");
            // #endregion

            // #region agent log
            DebugLog(
                "baseline-readability",
                "H8",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Taskbar blur and size values",
                $"{{\"blurRadius\":{blurRadius},\"taskbarActualHeight\":{TaskbarBorder.ActualHeight},\"windowHeight\":{ActualHeight}}}");
            // #endregion

            // #region agent log
            DebugLog(
                "baseline-readability",
                "H9",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Clock and menu readability values",
                $"{{\"clockForeground\":\"{EscapeForJson(clockColor)}\",\"clockFontSize\":{ClockDisplay.FontSize},\"menuButtonCount\":{TaskbarGrid.Children.OfType<StackPanel>().FirstOrDefault()?.Children.OfType<Button>().Count() ?? 0}}}");
            // #endregion

            var leftPanel = TaskbarGrid.Children.OfType<StackPanel>().FirstOrDefault();
            var menuButton = leftPanel?.Children.OfType<Button>().FirstOrDefault();
            var menuBg = ((menuButton?.Background as SolidColorBrush)?.Color.ToString()) ?? "non-solid";
            var menuFg = ((menuButton?.Foreground as SolidColorBrush)?.Color.ToString()) ?? "non-solid";
            var menuPadding = menuButton?.Padding.ToString() ?? "n/a";
            // #region agent log
            DebugLog(
                "baseline-readability",
                "H10",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Menu button contrast and sizing values",
                $"{{\"menuBackground\":\"{EscapeForJson(menuBg)}\",\"menuForeground\":\"{EscapeForJson(menuFg)}\",\"menuFontSize\":{menuButton?.FontSize ?? -1},\"menuPadding\":\"{EscapeForJson(menuPadding)}\"}}");
            // #endregion

            var trayPanel = TaskbarGrid.Children.OfType<StackPanel>().Skip(1).FirstOrDefault();
            var trayTextSizes = trayPanel == null
                ? new string[0]
                : trayPanel.Children.OfType<TextBlock>().Select(tb => tb.FontSize.ToString("0.##")).ToArray();
            // #region agent log
            DebugLog(
                "baseline-readability",
                "H11",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Tray text size distribution",
                $"{{\"trayItemCount\":{(trayPanel?.Children.Count ?? 0)},\"trayFontSizes\":[\"{string.Join("\",\"", trayTextSizes.Select(EscapeForJson))}\"]}}");
            // #endregion

            var appButtonWidths = DesktopAppsPanel.Children.OfType<Button>().Select(b => b.ActualWidth).ToArray();
            // #region agent log
            DebugLog(
                "baseline-readability",
                "H12",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Desktop app button rendered widths",
                $"{{\"buttonCount\":{appButtonWidths.Length},\"widths\":[{string.Join(",", appButtonWidths.Select(w => w.ToString("0.##")))}]}}");
            // #endregion
        }

        private static void DebugLog(string runId, string hypothesisId, string location, string message, string data)
        {
            var payload = $"{{\"sessionId\":\"96355c\",\"runId\":\"{EscapeForJson(runId)}\",\"hypothesisId\":\"{EscapeForJson(hypothesisId)}\",\"location\":\"{EscapeForJson(location)}\",\"message\":\"{EscapeForJson(message)}\",\"data\":{data},\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}";
            try
            {
                File.AppendAllText(@"c:\Users\Acer\Desktop\Linux os based on vusial studio\debug-96355c.log", payload + Environment.NewLine);
            }
            catch
            {
                // intentionally ignore logging failure
            }
        }

        private static string EscapeForJson(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
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