
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
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Windows.Threading;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для TaskCatalogPage.xaml
    /// </summary>
    public partial class TaskCatalogPage : Page
    {
        //локальные переменные для фильтрации
        private ICollectionView _tasksView;
        private ObservableCollection<TaskCollection> _allTasks;

        public TaskCatalogPage()
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

            LoadTasks();
        }

        //загрузка всех UserControl-ов
        private void LoadTasks()
        {
            _allTasks = new ObservableCollection<TaskCollection>(JsonControl.CollectionArray);
            List<ButtonControlCatalog> collections = new List<ButtonControlCatalog>();
            foreach (var task in _allTasks) // Перебор подборок в массиве для добавления их на экран
            {
                ButtonControlCatalog userControl = new ButtonControlCatalog(task); // Создание UserControl-a на основе подборки
                userControl.NavigationRequested += (s, set) =>
                {
                    NavigationService.Navigate(new CollectionPage(set)); //подписка на событие для клика по user control
                };
                collections.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство

            }
            _tasksView = CollectionViewSource.GetDefaultView(collections);
            _tasksView.Filter = TaskFilter;

            // Инициализация ItemsControl
            TasksContainer.ItemsSource = _tasksView;
        }

        //фильтр
        private bool TaskFilter(object item)
        {
            var task = (TaskCollection)((ButtonControlCatalog)item).DataContext;
            bool isVisible = true;

            // Фильтрация по типам заданий (OR-логика между чекбоксами одного типа)
            if (speakCheackBox.IsChecked == true ||
                readingCheckBox.IsChecked == true ||
                writingCheckBox.IsChecked == true ||
                listeningCheckBox.IsChecked == true)
            {
                isVisible = (speakCheackBox.IsChecked == true && task.isSpeaking) ||
                           (readingCheckBox.IsChecked == true && task.isReading) ||
                           (writingCheckBox.IsChecked == true && task.isWriting) ||
                           (listeningCheckBox.IsChecked == true && task.isListening);
            }

            // Дополнительные фильтры (AND-логика с основными)
            if (varOfExam.IsChecked == true)
                isVisible &= task.isVariants;

            if (actualTasks.IsChecked == true)
                isVisible &= task.isFastRepeat;

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
            _tasksView.Refresh();
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
                    Process.Start("hh.exe", $"{chmPath}::/taskСollections.htm");
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
