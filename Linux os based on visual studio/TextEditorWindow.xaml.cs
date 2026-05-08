using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace LinuxMintOS
{
    public partial class TextEditorWindow : Window
    {
        private string currentFile;

        public TextEditorWindow()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            EditorText.Clear();
            currentFile = null;
            Title = "Text Editor - Untitled";
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                currentFile = dialog.FileName;
                EditorText.Text = File.ReadAllText(currentFile);
                Title = $"Text Editor - {Path.GetFileName(currentFile)}";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (currentFile == null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    currentFile = dialog.FileName;
                }
                else
                {
                    return;
                }
            }

            File.WriteAllText(currentFile, EditorText.Text);
            Title = $"Text Editor - {Path.GetFileName(currentFile)}";
            MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}