using System;
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

        public SpeakingUserControl(SpeakingTask task)
        {
            Task = task;
            InitializeComponent();

            taskTextBlock.Text = taskText;
            topicTextBlock.Text = task.TaskText;
            idTextBox.Text += (task.id).ToString();
            recommendedTimeTextBlock.Text += task.RecommendedTime.ToString() + "мин.";

            //Подписка для конвертации
            speakingConvert.Click += (sender, e) => Conversion.ConvertSpeaking(task);
            speakingConvert.Click += (sender, e) => MessageBox.Show("Файл/ы с заданием скачан и находится на вашем рабочем столе");
        }

        private void StartRecord(object sender, RoutedEventArgs e) // Начало записи
        {
            if (!isRecordingInProgress && !isRecordingDone) // Если запись ещё не проводилась и запись не идёт прямо сейчас
            {
                SoundControl.StartRecording(Task.id, true);
                isRecordingInProgress = true;
                inputRecordingStatusTextBox.Text = "Запись идёт.";
                inputRecordingStatusTextBox.Background = Brushes.LightGreen; // При начале записи цвет квадрата с текстом меняется с красного на зелёный
            }
        }

        private void StopRecord(object sender, RoutedEventArgs e)
        {
            if (!isRecordingInProgress) return;

            SoundControl.StopRecording(); // Остановка записи

            //Обновление интерфейса
            isRecordingInProgress = false;
            isRecordingDone = true;
            inputRecordingStatusTextBox.Text = "Ответ сохранён.";
            inputRecordingStatusTextBox.Background = Brushes.LightGreen;

            resultTextBlock.Text = "Можно слушать свой и пример идеального варианты ответа.";
            resultTextBlock.Background = Brushes.LightGreen;
        }

        private void PlayUserAnswer(object sender, RoutedEventArgs e)
        {
            if (!isRecordingDone)
            {
                resultTextBlock.Text = "Сначала запишите свой ответ.";
                return;
            }

            isPlayingSomeAnswerInProgress = true;

            // Получение пути к файлу
            string filePath = SoundControl.GetAnswerFilePath(Task.id, true);

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл записи не найден, это ошибка, приносим свои извинения.");
                return;
            }

            SoundControl.AudioPath = filePath;
            SoundControl.PlayAudio(); // Воспроизведение аудио
        }

        private void PlayIdealAnswer(object sender, RoutedEventArgs e)
        {
            if (!isRecordingDone)
            {
                resultTextBlock.Text = "Сначала запишите свой ответ.";
                return;
            }

            isPlayingSomeAnswerInProgress = true;

            // Получение пути к файлу
            string filePath = SoundControl.GetAnswerFilePath(Task.id, false);

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл записи не найден, это ошибка, приносим свои извинения.");
                return;
            }

            SoundControl.AudioPath = filePath;
            SoundControl.PlayAudio(); // Воспроизведение аудио
        }

        private void StopPlaying(object sender, RoutedEventArgs e)
        {
            if (!isPlayingSomeAnswerInProgress)
            {
                resultTextBlock.Text = "Сейчас ничего не проигрывается.";
                return;
            }

            SoundControl.StopAudio(); // Остановка воспроизведения
            isPlayingSomeAnswerInProgress = false;
            resultTextBlock.Text = "Воспроизведение остановлено.";
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

        public bool Check() => true; // Необходим для реализации интерфейса - не имеет функционала

        private void ShowEvaluationCriteria(object sender, RoutedEventArgs e) // Вывод критериев для самооценки
        {
            string pdfPath = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "..\\..\\EvaluationCriterias\\SpeakingEvaluationCriteria.pdf");
            pdfPath = System.IO.Path.GetFullPath(pdfPath); // Путь к файлу с критериями

            try
            {
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть PDF с критериями.");
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
            string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string file = System.IO.Path.Combine(projectDir, "resourcesTask", "Collections", "userCollections.json");
            string jsonData = File.ReadAllText(file);

            SpeakingTask data = (SpeakingTask)this.DataContext;

            List<int> idList = new List<int>() { data.id };

            //создание экземпляра TaskCollection
            TaskCollection userTask = new TaskCollection(data.id, name, "",
                         idList, false, false, false, true, false, false);

            List<TaskCollection> list = JsonConvert.DeserializeObject<List<TaskCollection>>(jsonData) ?? new List<TaskCollection>();

            list.Add(userTask);

            string updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, updatedJson);

            MessageBox.Show("Данная подбока добавлена в раздел \"Мои подборки заданий\"");
        }
    }
}
