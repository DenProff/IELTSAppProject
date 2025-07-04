﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для UserCollectionPage.xaml
    /// </summary>
    public partial class UserCollectionPage : Page
    {
        //локальные переменные для фильтрации
        private ICollectionView tasksView;
        private ObservableCollection<TaskCollection> allTasks;

        public UserCollectionPage()
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
                    //OpenChmHelp();
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
            allTasks = new ObservableCollection<TaskCollection>(JsonControl.UserCollectionsArray);
            List<ButtonControlCatalog> collections = new List<ButtonControlCatalog>();
            foreach (var task in allTasks) // Перебор подборок в массиве для добавления их на экран
            {
                ButtonControlCatalog userControl = new ButtonControlCatalog(task); // Создание UserControl-a на основе подборки
                userControl.NavigationRequested += (s, set) =>
                {
                    NavigationService.Navigate(new CollectionPage(set)); //подписка на событие для клика по user control
                };
                collections.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство

            }
            if (collections.Count == 0) //если нет подборок
                emptyMessage.Visibility = Visibility.Visible;
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

            //Для вывода надписи об отсутствии подборок
            if (isVisible || ((varOfExam.IsChecked == true && task.isVariants) &&
                (actualTasks.IsChecked == true && task.isFastRepeat)) ||
                 //когда все галочки не нажаты
                 (speakCheackBox.IsChecked == false &&
                  readingCheckBox.IsChecked == false &&
                  writingCheckBox.IsChecked == false &&
                  listeningCheckBox.IsChecked == false &&
                  actualTasks.IsChecked == false &&
                  varOfExam.IsChecked == false))
                emptyMessage.Visibility = Visibility.Hidden;
            else
                emptyMessage.Visibility = Visibility.Visible;

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


        //справка
        private void OpenChmHelp()
        {
            string chmPath = System.IO.Path.Combine(
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

        private void language_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LanguageSelectionPage());
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }

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
