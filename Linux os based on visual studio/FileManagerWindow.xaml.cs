using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LinuxMintOS
{
    public partial class FileManagerWindow : Window
    {
        private string currentPath;
        private ObservableCollection<string> files;

        public FileManagerWindow()
        {
            InitializeComponent();
            files = new ObservableCollection<string>();
            FileListBox.ItemsSource = files;
            currentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LoadFiles();
            Loaded += FileManagerWindow_Loaded;
        }

        private void FileManagerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LogToolbarReadability("baseline-filemanager", "H13", "Toolbar button render metrics");
            LogToolbarReadability("baseline-filemanager", "H14", "Toolbar foreground/background contrast values");
            LogToolbarReadability("baseline-filemanager", "H15", "Toolbar typography and container size values");
        }

        private void LogToolbarReadability(string runId, string hypothesisId, string message)
        {
            var buttons = new[] { HomeButton, NewFolderButton, RefreshButton };
            var metrics = string.Join(",", buttons.Select(b =>
            {
                var bg = (b.Background as SolidColorBrush)?.Color.ToString() ?? "non-solid";
                var fg = (b.Foreground as SolidColorBrush)?.Color.ToString() ?? "non-solid";
                var contentAreaHeight = b.ActualHeight - b.Padding.Top - b.Padding.Bottom;
                var styleSource = b.Style == null ? "local-or-default" : "explicit-style";
                return $"{{\"name\":\"{EscapeForJson(b.Name)}\",\"content\":\"{EscapeForJson(b.Content?.ToString() ?? string.Empty)}\",\"actualWidth\":{b.ActualWidth.ToString("0.##")},\"actualHeight\":{b.ActualHeight.ToString("0.##")},\"contentAreaHeight\":{contentAreaHeight.ToString("0.##")},\"fontSize\":{b.FontSize.ToString("0.##")},\"padding\":\"{EscapeForJson(b.Padding.ToString())}\",\"background\":\"{EscapeForJson(bg)}\",\"foreground\":\"{EscapeForJson(fg)}\",\"styleSource\":\"{EscapeForJson(styleSource)}\"}}";
            }));

            // #region agent log
            DebugLog(
                runId,
                hypothesisId,
                "FileManagerWindow.xaml.cs:FileManagerWindow_Loaded",
                message,
                $"{{\"toolbarWidth\":{ActualWidth.ToString("0.##")},\"toolbarHostHeight\":{((HomeButton.Parent as Panel)?.ActualHeight ?? -1).ToString("0.##")},\"buttons\":[{metrics}]}}");
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

        private void LoadFiles()
        {
            files.Clear();
            try
            {
                var dirs = Directory.GetDirectories(currentPath).Select(d => Path.GetFileName(d) + "/");
                var fils = Directory.GetFiles(currentPath).Select(f => Path.GetFileName(f));

                foreach (var dir in dirs)
                    files.Add(dir);
                foreach (var file in fils)
                    files.Add(file);

                StatusText.Text = $"Items: {files.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            currentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LoadFiles();
        }

        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderName = "NewFolder";
            string path = Path.Combine(currentPath, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LoadFiles();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadFiles();
        }
    }
}