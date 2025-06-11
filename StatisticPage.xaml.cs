
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using DocumentFormat.OpenXml.Drawing;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        
        public StatisticPage()
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

            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            // Переменные для хранения правильных ответов/полностью решенных заданий Listening и Reading
            int listeningCorrectAnswers = 0;
            int listeningCorrectTasks = 0;

            int readingCorrectAnswers = 0;
            int readingCorrectTasks = 0;

            foreach (GeneralizedTask task in taskArray)
            {
                if (task is ListeningTask listeningTask)
                {
                    if (listeningTask.CorrectAnswers == 10)
                        listeningCorrectTasks++;
                    listeningCorrectAnswers += listeningTask.CorrectAnswers;
                }
                if (task is ReadingTask readingTask)
                {
                    if (readingTask.CorrectAnswers == 10)
                        readingCorrectTasks++;
                    readingCorrectAnswers += readingTask.CorrectAnswers;
                }
            }

            // Обновление текстовых полей на странице статистики
            listeningStats.Text = $"{listeningCorrectTasks} / 5 ({listeningCorrectAnswers * 2}%)";
            readingStats.Text = $"{readingCorrectTasks} / 5 ({listeningCorrectAnswers * 2}%)";
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
                    Process.Start("hh.exe", $"{chmPath}::/statistics.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void turnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void statistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mistakes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MistakesPage());
        }

        private void collections_Click(object sender, RoutedEventArgs e)
        {

        }

        private void language_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LanguageSelectionPage());
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
