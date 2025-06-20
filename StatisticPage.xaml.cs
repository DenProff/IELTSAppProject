
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

            // Расчеты статистики
            int listeningPercentage = listeningTasksCount > 0
            ? (int)Math.Round(listeningCorrectAnswers / listeningTasksCount * 10)
            : 0;

            int readingPercentage = readingTasksCount > 0
            ? (int)Math.Round(readingCorrectAnswers * 10 / readingTasksCount)
            : 0;

            // Обновление текстовых полей на странице статистики
            listeningStats.Text = $"{listeningCorrectTasks} / {listeningTasksCount} ({listeningPercentage}%)";
            readingStats.Text = $"{readingCorrectTasks} / {readingTasksCount} ({readingPercentage}%)";

            // Смена цветов
            listeningStats.Foreground = GetPercentageColorBrush(listeningPercentage);
            readingStats.Foreground = GetPercentageColorBrush(readingPercentage);

            // Формирование рекомендации
            if (listeningPercentage < readingPercentage)
            {
                recomendation.Text = "Listening";
            }
            else if (listeningPercentage > readingPercentage)
            {
                recomendation.Text = "Reading";
            }
            else
            {
                recomendation.Text = "Listening и Reading";
            }
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
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

        // Метод для получения цвета
        private Brush GetPercentageColorBrush(int percentage)
        {
            if (percentage < 1) return Brushes.LightGray;         // 0% - серый
            if (percentage <= 30) return Brushes.Red;             // 0-30% - красный
            if (percentage <= 60) return Brushes.Orange;          // 31-60% - оранжевый
            if (percentage <= 80) return Brushes.YellowGreen;     // 61-80% - желто-зеленый
            return Brushes.LawnGreen;                             // 81-100% - зеленый
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

        public static string[] resourcesKeysArray =
{
        "myStatistics",
        "myMistakes",
        "myCollections",
        "chooseLanguage",
        "help",
        "prevPage"
        }; // Массив с ключами для ресурсов - необходимо для реализации многоязычности

        private void home_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void baseCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TaskCatalogPage());
        }
    }
}
