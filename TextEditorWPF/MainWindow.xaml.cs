using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsSaved { get; set; }
        private string FilePath { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            IsSaved = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        textArea.FontStyle = FontStyles.Normal;
                        break;
                    case 1:
                        textArea.FontStyle = FontStyles.Oblique;
                        break;
                    case 2:
                        textArea.FontStyle = FontStyles.Italic;
                        break;
                }
            }
        }

        private void textArea_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                {
                    textArea.FontSize *= 1.1;
                }
                else if (e.Delta < 0)
                {
                    textArea.FontSize /= 1.1;
                }

                e.Handled = true;
            }
        }

        private void fontSizeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(fontSizeText.Text, out int fontSize))
            {
                if (fontSize > 0 && fontSize < 100)
                {
                    textArea.FontSize = fontSize;
                }
                else
                {
                    textArea.FontSize = 32;
                    fontSizeText.Text = "32";
                }
            }
            else if (string.IsNullOrWhiteSpace(fontSizeText.Text))
            {
                textArea.FontSize = 32;
            }
            else
            {
                textArea.FontSize = 32;
                fontSizeText.Text = "32";
            }
        }

        private void textArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(fontSizeText.Text))
                fontSizeText.Text = "32";
            if (autoSave.IsChecked == true)
            {
                IsSaved = true;
                Save(FilePath);
            }
            else
            {
                IsSaved = false;
            }
        }

        private void MenuItem_Cyberpunk_Click(object sender, RoutedEventArgs e)
        {
            textArea.Background = new SolidColorBrush(Color.FromRgb(11, 61, 145));
            textArea.Foreground = new SolidColorBrush(Color.FromRgb(57, 255, 20));
        }

        private void MenuItem_Normal_Click(object sender, RoutedEventArgs e)
        {
            textArea.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            textArea.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void MenuItem_Retro_Click(object sender, RoutedEventArgs e)
        {
            textArea.Background = new SolidColorBrush(Color.FromRgb(255, 225, 204));
            textArea.Foreground = new SolidColorBrush(Color.FromRgb(96, 96, 96));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            if (IsSaved)
            {
                textArea.Text = string.Empty;
                fontSizeText.Text = "32";
                textArea.FontStyle = FontStyles.Normal;
                FilePath = string.Empty;
                Title = "Text Editor";
            }
            else
            {
                var result = MessageBox.Show("File not saved, do you want to exit?", "Changes doesn't saved.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    IsSaved = true;
                    MenuItem_New_Click(sender, e);
                }
            }
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (IsSaved)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.FileName = "Document";
                dialog.DefaultExt = ".txt";
                dialog.Filter = "Text documents (.txt)|*.txt";
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    FilePath = dialog.FileName;
                    textArea.Text = File.ReadAllText(FilePath);
                    Title = $"@isaaholic Text Editor - {dialog.SafeFileName}";
                }
                else
                {
                    MessageBox.Show("File doesn't exist", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var result = MessageBox.Show("File not saved, do you want to exit?", "Changes doesn't saved.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    IsSaved = true;
                    MenuItem_Open_Click(sender, e);
                }
            }
        }

        private void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "Document";
                dialog.DefaultExt = ".txt";
                dialog.Filter = "Text documents (.txt)|*.txt";
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    FilePath = dialog.FileName;

                    IsSaved = true;
                    Title = $"@isaaholic Text Editor - {dialog.SafeFileName}";
                    File.WriteAllText(FilePath, textArea.Text);
                }
                else
                {
                    MessageBox.Show("Unknown error.", "Unknown error.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                File.WriteAllText(FilePath, textArea.Text);
            }
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            Save(FilePath);
        }

        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            Save(filePath);
        }

        private void MenuItem_DateTime_Click(object sender, RoutedEventArgs e)
        {
            textArea.Text += DateTime.Now.ToString();
        }
    }
}