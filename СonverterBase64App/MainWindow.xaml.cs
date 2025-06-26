using System.IO;
using System.Windows;

using System.Windows.Controls;
using Microsoft.Win32;

namespace СonverterBase64App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        public string? _selectedInputFilePath;
        public string? _selectedOutputFilePath;
        public void InputBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Диалоговое окно
                openFileDialog.Title = "Выберите файл";
                openFileDialog.Filter = "Все файлы (*.*)|*.*";

                // Проверка выбора файла
                if (openFileDialog.ShowDialog() == true)
                {
                    // Установить путь к файлу в TextBox
                    InputFilePathTextBox.Text = openFileDialog.FileName;
                    _selectedInputFilePath = InputFilePathTextBox.Text;
                }
            }

        }
        public void InputFilePathTextChanged(object sender, TextChangedEventArgs args)
        {
            _selectedInputFilePath = InputFilePathTextBox.Text;

        }

        public void OutputBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.Title = "Выберите файл";
                saveFileDialog.Filter = "DLL-файлы (*.dll)|*.dll";
                saveFileDialog.DefaultExt = ".dll";

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Установить путь к файлу в TextBox
                    OutputFilePathTextBox.Text = saveFileDialog.FileName;
                    _selectedOutputFilePath = OutputFilePathTextBox.Text;
                }
            }

        }
        public void OutputFilePathTextChanged(object sender, TextChangedEventArgs args)
        {
            _selectedOutputFilePath = OutputFilePathTextBox.Text;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_selectedInputFilePath))
                {
                    MessageBox.Show("Пожалуйста, выберите входной файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!File.Exists(_selectedInputFilePath))
                {
                    MessageBox.Show("Входного файл не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Проверка выходного файла
                if (string.IsNullOrWhiteSpace(_selectedOutputFilePath))
                {
                    MessageBox.Show("Пожалуйста, выберите выходной файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string outputFileExtension = System.IO.Path.GetExtension(_selectedOutputFilePath);
                if (!outputFileExtension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Выходной файл должен иметь расширение .dll", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string testPath = System.IO.Path.GetDirectoryName(_selectedOutputFilePath);
                if (!Directory.Exists(testPath))
                  {
                     MessageBox.Show("Указанный путь для сохранения не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                  }

                string base64String = File.ReadAllText(_selectedInputFilePath);

                byte[] decodedBytes;
                try
                {
                    decodedBytes = Convert.FromBase64String(base64String);

                    File.WriteAllBytes(_selectedOutputFilePath, decodedBytes);

                    MessageBox.Show("DLL-файл успешно сохранён!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                catch
                {
                    MessageBox.Show("Содержимое файла не является строкой формата base64", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}