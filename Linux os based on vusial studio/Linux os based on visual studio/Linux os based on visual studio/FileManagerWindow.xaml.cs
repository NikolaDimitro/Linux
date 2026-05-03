using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

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