﻿using System;
using System.Collections.Generic;
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
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Threading;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для SpeakingPage.xaml
    /// </summary>
    public partial class SpeakingUserControl : UserControl, ICheckable
    {
        public SpeakingTask Task { get; set; }
        static private bool isRecordingInProgress = false; // Сейчас идёт какая-то запись?
        private bool isRecordingDone = false; // В одной подборке мб несколько заданий speaking
        static private bool isPlayingSomeAnswerInProgress = false; // Сейчас воспроизводится какой-то ответ?
        static string taskText = @"You should:
- Express your attitude toward the given problem.
- Analyze the problem and justify your position, providing at least 2 supporting arguments.
- Summarize your ideas.

Your speech should last no less than 3 minutes and no longer than 5 minutes.";

        private DispatcherTimer recordingTimer; // Замеряет время каждую секунду
        private DateTime recordingStartTime; // Время начала записи
        private TimeSpan maxRecordingDuration = TimeSpan.FromMinutes(5); // Максимальное время записи - 5 минут

        public SpeakingUserControl(SpeakingTask task)
        {
            Task = task;
            InitializeComponent();
            // Загрузка сохранённого язык
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language)) // Дополнительная безопасность, чтобы если что не было исключений
            {
                SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);
            }

            // Подписка на смену языка - событие в классе LanguageChange
            LanguageChange.LanguageChanged += () => SetLanguageResources.SetLanguageResourcesMethod(Properties.Settings.Default.Language, this);

            // Установка текста в элементах, где он будет меняться по мере работы с заданием
            inputRecordingStatusTextBox.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordingNotStartedMessage");
            resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordingEnableOnlyAfterSettingAnswer");

            taskTextBlock.Text = taskText;
            topicTextBlock.Text += " " + task.TaskText;
            idTextBox.Text += " " + (task.id).ToString();
            recommendedTimeTextBlock.Text += " " + task.RecommendedTime.ToString() + " мин.";

            // Организация контроля времени
            recordingTimer = new DispatcherTimer();
            recordingTimer.Interval = TimeSpan.FromSeconds(1); // Проверка времени записи каждую секунду
            recordingTimer.Tick += CheckOfTimeEverySecond; // Каждую секунду будет вызываться обработчик события "Tick" - RecordingTimer_Tick

            DataContext = task; //контекстные данные

            //Подписка для конвертации
            speakingConvert.Click += (sender, e) => Conversion.ConvertSpeaking(task);
            speakingConvert.Click += (sender, e) => Conversion.ConvertMessage();
        }

        private void CheckOfTimeEverySecond(object sender, EventArgs e)
        {
            if (isRecordingInProgress)
            {
                TimeSpan elapsedTime = DateTime.Now - recordingStartTime; // Сколько времени прошло от начала записи
                
                if (elapsedTime >= maxRecordingDuration) // Если время записи превысило лимит
                {
                    StopRecord(this, new RoutedEventArgs()); // Принудительная остановка записи
                    recordingTimer.Stop(); // Остановка таймера

                    // Всплывающее окно, чтобы пользователь точно не пропустил сообщение и не тратил время
                    MessageBox.Show(SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordingForcedToStopped"));
                }
            }
        }

        private void StartRecord(object sender, RoutedEventArgs e) // Начало записи
        {
            if (!isRecordingInProgress && !isRecordingDone) // Если запись ещё не проводилась и запись не идёт прямо сейчас
            {
                SoundControl.StartRecording(Task.id, true);
                isRecordingInProgress = true;
                inputRecordingStatusTextBox.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordingInProgress");
                inputRecordingStatusTextBox.Background = Brushes.LightGreen; // При начале записи цвет квадрата с текстом меняется с красного на зелёный

                recordingStartTime = DateTime.Now; // Фиксация времени начала записи ответа
                recordingTimer.Start(); // Запуск таймера
            }
        }

        private void StopRecord(object sender, RoutedEventArgs e) // Окончание записи
        {
            if (!isRecordingInProgress) return;

            SoundControl.StopRecording(); // Остановка записи
            recordingTimer.Stop(); // Остановка таймера (прошло менее 5 минут, но пользователь сам остановил запись)

            //Обновление интерфейса
            isRecordingInProgress = false;
            isRecordingDone = true;
            inputRecordingStatusTextBox.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "answerIsSaved");
            inputRecordingStatusTextBox.Background = Brushes.LightGreen;

            resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "UserAndIdealAnswersAreEnable");
            resultTextBlock.Background = Brushes.LightGreen;
        }

        private void PlayUserAnswer(object sender, RoutedEventArgs e) // Воспроизведение ответа пользователя
        {
            if (!isRecordingDone)
            {
                resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordAnswerFirst");
                return;
            }

            StopPlaying(this, new RoutedEventArgs()); // Если уже что-то было запущено - оно выключается

            isPlayingSomeAnswerInProgress = true;

            // Получение пути к файлу
            string filePath = SoundControl.GetAnswerFilePath(Task.id, true);

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                MessageBox.Show("answerFileMistake");
                return;
            }

            SoundControl.AudioPath = filePath;
            SoundControl.PlayAudio(); // Воспроизведение аудио
        }

        private void PlayIdealAnswer(object sender, RoutedEventArgs e) // Воспроизведение примера ответа на максимальный балл
        {
            if (!isRecordingDone)
            {
                resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "recordAnswerFirst");
                return;
            }

            StopPlaying(this, new RoutedEventArgs()); // Если уже что-то было запущено - оно выключается

            isPlayingSomeAnswerInProgress = true;

            // Получение пути к файлу
            string filePath = SoundControl.GetAnswerFilePath(Task.id, false);

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                MessageBox.Show(SetLanguageResources.GetString(Properties.Settings.Default.Language, "answerFileMistake"));
                return;
            }

            SoundControl.AudioPath = filePath;
            SoundControl.PlayAudio(); // Воспроизведение аудио
        }

        private void StopPlaying(object sender, RoutedEventArgs e) // Остановка воспроизведения
        {
            if (!isPlayingSomeAnswerInProgress)
            {
                resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "nothingIsPlaying");
                return;
            }

            SoundControl.StopAudio(); // Остановка воспроизведения
            isPlayingSomeAnswerInProgress = false;
            resultTextBlock.Text = SetLanguageResources.GetString(Properties.Settings.Default.Language, "playingWasStopped");
        }

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
                    Process.Start("hh.exe", $"{chmPath}::/speaking.htm");
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

        public (bool, int) Check() => (true, 0); // Необходим для реализации интерфейса - не имеет функционала

        private void ShowEvaluationCriteria(object sender, RoutedEventArgs e) // Вывод критериев для самооценки
        {
            string pdfPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "EvaluationCriterias\\SpeakingEvaluationCriteria.pdf");
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
            string file = System.IO.Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            SpeakingTask data = (SpeakingTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, today,
                         idList, false, true, false, false, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            CollectionPage.AddMessage();
        }
    }
}
