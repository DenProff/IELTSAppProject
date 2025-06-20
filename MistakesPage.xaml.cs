
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
    /// Логика взаимодействия для MistakesPage.xaml
    /// </summary>
    public partial class MistakesPage : Page
    {
        //локальные переменные для фильтрации
        private ICollectionView tasksView;
        private ObservableCollection<Tuple<int, string, string>> allId;
        public MistakesPage()
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
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);

            LoadTasks();
        }

        //загрузка всех UserControl-ов
        private void LoadTasks()
        {
            allId = new ObservableCollection<Tuple<int, string, string>>(JsonControl.MistakesArray);
            List<ButtonControlCatalog> collections = new List<ButtonControlCatalog>();
            foreach (Tuple<int, string, string> task in allId) // Перебор подборок в массиве для добавления их на экран
            {
                TaskCollection mistakeTask = null;
                if (task.Item2 == "ReadingTask")
                {
                    List<int> list = new List<int> { task.Item1 };
                    mistakeTask = new TaskCollection(task.Item1, $"{task.Item2} {task.Item1.ToString()}", task.Item3,
                         list, false, false, false, true, false, false );
                }
                else if (task.Item2 == "ListeningTask")
                {
                    List<int> list = new List<int> { task.Item1 };
                    mistakeTask = new TaskCollection(task.Item1, $"{task.Item2} {task.Item1.ToString()}", task.Item3,
                         list, true, false, false, false, false, false);
                }

                ButtonControlCatalog userControl = new ButtonControlCatalog(mistakeTask); // Создание UserControl-a на основе подборки
                userControl.NavigationRequested += (s, set) =>
                {
                    NavigationService.Navigate(new CollectionPage(set)); //подписка на событие для клика по user control
                };
                collections.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство

            }
            tasksView = CollectionViewSource.GetDefaultView(collections);
            tasksView.Filter = TaskFilter;

            // Инициализация ItemsControl
            TasksContainer.ItemsSource = tasksView;
        }

        //фильтр
        private bool TaskFilter(object item)
        {
            var task = (TaskCollection)((ButtonControlCatalog)item).DataContext;
            bool isVisible = true;

            // Фильтрация по типам заданий (OR-логика между чекбоксами одного типа)
            if (readingCheckBox.IsChecked == true ||
                listeningCheckBox.IsChecked == true)
            {
                isVisible = (readingCheckBox.IsChecked == true && task.isReading) ||
                            (listeningCheckBox.IsChecked == true && task.isListening);
            }

            return isVisible;
        }

        //событие для перехода на задания
        private void ButtonControlCatalog_NavigationRequested(object sender, TaskCollection task)
        {
            NavigationService?.Navigate(new CollectionPage(task));
        }

        //обновление данных
        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            tasksView.Refresh();
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
                    Process.Start("hh.exe", $"{chmPath}::/errors.htm");
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
            NavigationService?.Navigate(new StatisticPage());
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
        "prevPage",
        "taskType"
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
