
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

            double[] statisticsArray = JsonControl.StatisticsArray;

            // Переменные для хранения правильных ответов/полностью решенных заданий Listening и Reading
            double listeningCorrectAnswers = statisticsArray[0];
            double listeningCorrectTasks = statisticsArray[1];
            double listeningTasksCount = statisticsArray[2];

            double readingCorrectAnswers = statisticsArray[3];
            double readingCorrectTasks = statisticsArray[4];
            double readingTasksCount = statisticsArray[5];

            // Обновление текстовых полей на странице статистики
            listeningStats.Text = listeningTasksCount > 0
            ? $"{listeningCorrectTasks} / {listeningTasksCount} ({Math.Round(listeningCorrectAnswers / listeningTasksCount * 10)}%)"
            : $"{listeningCorrectTasks} / {listeningTasksCount} (0%)";

            readingStats.Text = readingTasksCount > 0
            ? $"{readingCorrectTasks} / {readingTasksCount} ({Math.Round(readingCorrectAnswers * 10 / readingTasksCount)}%)"
            : $"{readingCorrectTasks} / {readingTasksCount} (0%)";
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

        private void mistakes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MistakesPage());
        }

        private void collections_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserCollectionPage());
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
