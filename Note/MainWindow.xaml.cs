using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Note
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string activeFileName = null;
        string activeFilePath = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void New_MenuItem(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure??", "Warning", MessageBoxButton.YesNoCancel);
            if(dialogResult == MessageBoxResult.Yes)
            {
                txtDocument.Text = "";
                activeFileName = "Untitled";
                activeFilePath = null;
                UpdateTitle();
            }
        }

        private void Open_MenuItem(object sender, RoutedEventArgs e)
        {
            // Configure dialog box
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                FileName = activeFileName,
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            // Show open file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();

            //Process open file dialog box results
            if(result == true)
            {
                //Open document
                activeFilePath = openFileDialog.FileName;
                activeFileName = new FileInfo(activeFilePath).Name;
                using (TextReader textReader = new StreamReader(activeFilePath))
                {
                    txtDocument.Text = textReader.ReadToEnd();
                }
                UpdateTitle();
            }
        }

        private void Save_MenuItem(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(activeFilePath))
                Save_us_MenuItem(sender, e);
            else
                using (TextWriter textWriter = new StreamWriter(activeFilePath))
                    textWriter.Write(txtDocument.Text);
                
        }

        private void Save_us_MenuItem(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = activeFileName,
                DefaultExt = ".txt", // Default file extension
                Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };

            Nullable<bool> result = saveFileDialog.ShowDialog();

            if(result == true)
            {
                activeFilePath = saveFileDialog.FileName;
                using (TextWriter textWriter = new StreamWriter(activeFilePath))
                {
                    textWriter.Write(txtDocument.Text);
                }
                //UpdateTitle;
            }
        }

        private void Exit_MenuItem(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure??", "Warning", MessageBoxButton.YesNoCancel);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Application.Current.MainWindow.Close();
            }
        }

        private void txtDocument_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void txtDocument_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateTitle()
        {
            window.Title = activeFileName + " - " + "Notepad";
        }

        private void UpdateStatus()
        {
            int caret = txtDocument.CaretIndex;
            int row = txtDocument.GetLineIndexFromCharacterIndex(caret);
            int col = caret - txtDocument.GetFirstVisibleLineIndex();
            statusBar.Text = String.Format("Ln {0}, Col {1}", row, col);
        }

        private void Wrap_MenuItem(object sender, RoutedEventArgs e)
        {
                txtDocument.TextWrapping = TextWrapping.Wrap;
        }

        private void Wrap_MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            txtDocument.TextWrapping = TextWrapping.NoWrap;
        }

        private void Font_MenuItem(object sender, RoutedEventArgs e)
        {
            ///System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();
            System.Windows.Forms.FontDialog fd = new System.Windows.Forms.FontDialog();
            System.Windows.Forms.DialogResult dr = fd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.Cancel)
            {
                txtDocument.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                txtDocument.FontSize = fd.Font.Size * 96.0 / 72.0;
                txtDocument.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                txtDocument.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void About_MenuItemk(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void Blog_MenuItem(object sender, RoutedEventArgs e)
        {
            Process.Start("chrome.exe", "google.com");
        }
    }
}
