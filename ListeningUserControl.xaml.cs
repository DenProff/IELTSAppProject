using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ListeningControl.xaml
    /// </summary>
    public partial class ListeningUserControl : UserControl, ICheckable
    {
        public ListeningUserControl(ListeningTask data)
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                this.Focus();
                this.Focusable = true;
                Keyboard.Focus(this);
            };

            this.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.F1)
                {
                    OpenChmHelp();
                    e.Handled = true;
                }
            };

            this.DataContext = data; // Запись в Binding информации из свойств объекта data

            listeningConvert.Click += (sender, e) => Conversion.ConvertListening(data);
            listeningConvert.Click += (sender, e) => MessageBox.Show("Файл/ы с заданием скачан и находится на вашем рабочем столе");
        }

        public bool Check() // Данный метод должен реализовывать проверку ответов пользователя
        {
            ListeningTask task = (ListeningTask)DataContext;
            bool hasErrors = false;

            // Создаем массивы для хранения элементов
            RadioButton[][] radioButtonsGroups = new[]
            {
                new[] { answer1A, answer1B, answer1C },
                new[] { answer2A, answer2B, answer2C },
                new[] { answer3A, answer3B, answer3C },
                new[] { answer4A, answer4B, answer4C },
                new[] { answer5A, answer5B, answer5C }
            };

            // Создаем массивы для хранения элементов
            TextBox[] textBoxesGroups = new[]
            {
                answer6, answer7, answer8, answer9, answer0
            };

            TextBlock[] resultTextBlocks = new[]
            {
                rightAnswer1, rightAnswer2, rightAnswer3, rightAnswer4, rightAnswer5, rightAnswer6, rightAnswer7, rightAnswer8, rightAnswer9, rightAnswer0
            };

            // Проверяем все группы RadioButton в цикле
            for (int i = 0; i < 5; i++)
            {
                bool isCorrect = IsAnswerCorrect(task.Answer[i], radioButtonsGroups[i]);
                resultTextBlocks[i].Text = isCorrect ? "Правильно!" : "Неправильно!";
                resultTextBlocks[i].Visibility = Visibility.Visible;
                if (!isCorrect) hasErrors = true;
            }

            // Проверяем все группы TextBox в цикле
            for (int i = 5; i < 10; i++)
            {
                bool isCorrect = task.Answer[i] == textBoxesGroups[i-5].Text.ToLower();
                resultTextBlocks[i].Text = isCorrect ? "Правильно!" : "Неправильно!";
                resultTextBlocks[i].Visibility = Visibility.Visible;
                if (!isCorrect) hasErrors = true;
            }

            return hasErrors; // true - если есть ошибки, false - если нет
        }

        private void SimpleAudioPlayer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OpenChmHelp()
        {
            string chmPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Help",
                "referenceData.chm"
            );

            if (File.Exists(chmPath))
            {
                try
                {
                    // Открыть страницу "settings.html" внутри CHM
                    Process.Start("hh.exe", $"{chmPath}::/listening.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            Check();
        }

        // Метод для проверки правильности ответа в RadioButton
        private bool IsAnswerCorrect(string expected, params RadioButton[] options)
        {
            return options.Any(btn => btn.IsChecked == true && (bool)btn.Content?.ToString().StartsWith(expected));
        }
    }
}
