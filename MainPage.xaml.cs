﻿using System;
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
using NAudio.Wave;
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
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
                    Process.Start("hh.exe", $"{chmPath}::/applicationModes.htm");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskCatalogPage page = new TaskCatalogPage();
            page.actualTasks.IsChecked = true;
            page.speakCheackBox.IsChecked = true;
            page.readingCheckBox.IsChecked = true;
            page.writingCheckBox.IsChecked = true;
            page.listeningCheckBox.IsChecked = true;
            NavigationService?.Navigate(page);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TaskCatalogPage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReadingUserControl(null));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // NavigationService?.Navigate(new FirstTestPage());
            NavigationService?.Navigate(new SpeakingUserControl(new SpeakingTask(0, "Проблема фимоза человечества", 12, "SpeakingTask")));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CollectionPage(new TaskCollection(5, "zalupa variant", "date", new List<int>{ 0, 1, 2 })));
            //NavigationService?.Navigate(new LanguageSelectionPage());
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            OpenChmHelp();
        }
    }
}
