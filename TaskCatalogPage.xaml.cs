
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
    /// Логика взаимодействия для TaskCatalogPage.xaml
    /// </summary>
    public partial class TaskCatalogPage : Page
    {
        
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

            //НУЖЕН МАССИВ ДИСЕРИАЛИЗОВАННЫЙ ТИПА ТАСККОЛЛЕКШН

            GeneralizedTask[] taskArray = JsonControl.TaskArray; // Десериализация в список json-файла со всеми заданиями

            foreach (var task in taskArray) // Перебор подборок в массиве для добавления их на экран
            {
                ButtonControlCatalog userControl = new ButtonControlCatalog(null); // Создание UserControl-a на основе подборки
                TasksContainer.Items.Add(userControl); // Добавление UserControl-а в выделенное в xaml-е пространство
            }

            while (true)
            {
                foreach (ButtonControlCatalog item in TasksContainer.Items)
                {
                    IsVariantsExist(item);
                    IsFastRepeatExist(item);
                    IsReadingExist(item);
                    IsListeningExist(item);
                    IsWritingExist(item);
                    IsSpeakingExist(item);
                }
            }

        }

        //проверка на существование в подборке задания Reading
        private void IsReadingExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isReading && readingCheckBox.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isReading && readingCheckBox.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isReading && readingCheckBox.IsChecked == false)
                elem.IsEnabled = false;
            else //(((TaskCollection)elem.DataContext).isReading && speakCheackBox.IsChecked == true)
                elem.IsEnabled = true;
        }

        //проверка на существование в подборке задания Listening
        private void IsListeningExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isListening && listeningCheckBox.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isListening && listeningCheckBox.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isListening && listeningCheckBox.IsChecked == false)
                elem.IsEnabled = false;
            else 
                elem.IsEnabled = true;
        }

        //проверка на существование в подборке задания Writing
        private void IsWritingExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isWriting && writingCheckBox.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isWriting && writingCheckBox.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isWriting && writingCheckBox.IsChecked == false)
                elem.IsEnabled = false;
            else 
                elem.IsEnabled = true;
        }

        //проверка на существование в подборке задания Speaking
        private void IsSpeakingExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isSpeaking && speakCheackBox.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isSpeaking && speakCheackBox.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isSpeaking && speakCheackBox.IsChecked == false)
                elem.IsEnabled = false;
            else 
                elem.IsEnabled = true;
        }

        //проверка на существование в подборке целого варианта экзамена
        private void IsVariantsExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isVariants && varOfExam.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isVariants && varOfExam.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isVariants && varOfExam.IsChecked == false)
                elem.IsEnabled = false;
            else
            {
                elem.IsEnabled = true;
                readingCheckBox.IsChecked = true;
                listeningCheckBox.IsChecked = true;
                writingCheckBox.IsChecked = true;
                speakCheackBox.IsChecked = true;
            }
                
        }

        //проверка на существование в подборке заданий для быстрого повторения
        private void IsFastRepeatExist(ButtonControlCatalog elem)
        {
            if (((TaskCollection)elem.DataContext).isFastRepeat && actualTasks.IsChecked == false ||
                !((TaskCollection)elem.DataContext).isFastRepeat && actualTasks.IsChecked == true ||
                !((TaskCollection)elem.DataContext).isFastRepeat && actualTasks.IsChecked == false)
                elem.IsEnabled = false;
            else
                elem.IsEnabled = true;
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
