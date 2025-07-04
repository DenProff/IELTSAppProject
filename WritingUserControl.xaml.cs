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
using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для WritingPage.xaml
    /// </summary>
    public partial class WritingUserControl : UserControl, ICheckable
    {
        static string TaskText = "WRITE AN ESSAY ON ONE OF THE FOLLOWING TOPICS:";
        public WritingTask TaskData; // Задание на основе которого генерируется данный UserControl

        public WritingUserControl(WritingTask task)
        {
            InitializeComponent();
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);

            answeField.IsEnabled = false; // До начала написания эссе должна быть выбрана тема, поэтому изначально поле для ввода недоступно
            showIdealEssay.IsEnabled = false; // Пока не сохранён ответ нельзя показывать другие эссе
            showUsersEssay.IsEnabled = false; // Пока не сохранён ответ нельзя показывать другие эссе
            saveAnswer.IsEnabled = false; // Пока не выбрана тема нельзя сохранить ответ

            taskTextBlock.Text = TaskText; // Установка общей формулировки задания

            TaskData = task;

            DataContext = task; //контекстные данные

            foreach (string topic in task.Topics)
            {
                topicsComboBox.Items.Add(topic); // Добавление тем в выпадающий список
            }

            idTextBox.Text += $" {task.id}"; // Установка id
            recommendedTimeTextBlock.Text += $" {task.RecommendedTime}"; // Установка рекомендованного времени выполнения

            //Подписка для конвертации
            writingConvert.Click += (sender, e) => Conversion.ConvertWriting(task);
            writingConvert.Click += (sender, e) => Conversion.ConvertMessage();
        }

        public WritingUserControl() { }

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
                    Process.Start("hh.exe", $"{chmPath}::/writing.htm");
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

        public (bool, int) Check() => (true, 0); // Метод необходим для реализации интерфейса - не имеет функционала для данного класса

        private void SaveTopic(object sender, RoutedEventArgs e) // Фиксирует выбранную тему
        {
            if (topicsComboBox.SelectedIndex != -1) // Если тема была выбрана
            {
                topicsComboBox.IsEnabled = false; // Делает выбор темы более недоступным
                saveTopicBtn.IsEnabled = false;   // Делает выбор темы более недоступным
                UnblockAnswerField();
            }
            else
            {
                ShowMessageWindow(SetLanguageResources.GetString(Properties.Settings.Default.Language, "remindToChooseTopicMessageBox"));
            }
        }

        private void SaveAnswer(object sender, RoutedEventArgs e) // Сохранение ответа пользователя
        {
            answeField.IsReadOnly = true; // После сохранения ответа нельзя вносить изменения
            showIdealEssay.IsEnabled = true;
            showUsersEssay.IsEnabled = true;

            TaskData.UserAnswer = answeField.Text; // Сохранение ответа пользователя

            // Изменение интерфейса
            infoTextBox.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "answerIsSaved");
            infoTextBox.Background = Brushes.LightGreen;
        }

        private void UnblockAnswerField() // Разблокировка поля для ввода эссе пользователя после выбора темы
        {
            answeField.IsEnabled = true;
            saveAnswer.IsEnabled = true;
            answeField.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "EnterAnswerHere");
        }

        private void ShowIdealEssay(object sender, RoutedEventArgs e) // Вывод примера идеального эссе по выбранной теме
        {
            int index = topicsComboBox.SelectedIndex; // Номер выбранной темы (индекс в массиве)
            if (index != -1)
            {
                answeField.Text = TaskData.Answer[index]; // Вывод в поле для ответа
            }
            else
            {
                ShowMessageWindow(SetLanguageResources.GetString(Properties.Settings.Default.Language, "UserDidNotChooseTopic")); // На всякий случай вывод сообщения - предполагается,
                                                          // что оно не должно быть показано никогда
            }
        }

        private void ShowUsersEssay(object sender, RoutedEventArgs e) // Вывод эссе пользователя по выбранной теме
        {
            answeField.Text = TaskData.UserAnswer; // Вывод в поле для ответа
        }

        private void ShowEvaluationCriteria(object sender, RoutedEventArgs e) // Вывод критериев для само-оценки
        {
            string pdfPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "EvaluationCriterias\\WritingEvaluationCriterias.pdf");
            pdfPath = System.IO.Path.GetFullPath(pdfPath); // Путь к файлу с критериями

            try
            {
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(SetLanguageResources.GetString(Properties.Settings.Default.Language, "PDFmistake"));
            }
        }

        private void ShowMessageWindow(string message) // Всплывающее окно с сообщением
        {
            MessageBox.Show(message);
        }

        private void AddToCollection_Click(object sender, RoutedEventArgs e)
        {
            string name = ""; //название подборки

            DialogInputWindow window = new DialogInputWindow();

            window.ShowDialog(); //показ диалогового окна для ввода названия подборки

            if (window.DialogResult != true) //если пользователь нажал отмена
                return;

            name = window.UserInput;

            DateTime now = DateTime.Today; //берем сегодняшнюю дату

            string today = now.ToString("dd.MM.yyyy"); //преводим ее в строку

            //работа с json
            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            string file = Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            WritingTask data = (WritingTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, today,
                         idList, false, false, true, false, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            CollectionPage.AddMessage();
        }
    }
}
