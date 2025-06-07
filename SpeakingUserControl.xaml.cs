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
        }

        private void StartRecord(object sender, RoutedEventArgs e) // Начало записи
        {
            if (!isRecordingInProgress && !isRecordingDone) // Если запись ещё не проводилась и запись не идёт прямо сейчас
            {
                SoundControl.StartRecording(Task.id);
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
            //try
            //{
            //    // Строчка ниже из первой версии класса
            //    //Task.AudioPathUserAnswer = File.ReadAllBytes(SoundControl.OutputFilePath);// Чтение файла
            //}
            //catch (Exception ex)
            //{
            //    
                  // Тут было обновление интерфейса
            //}
        }

        private void PlayUserAnswer(object sender, RoutedEventArgs e)
        {

        }

        private void PlayRealAnswer(object sender, RoutedEventArgs e)
        {

        }

        private void StopPlaying(object sender, RoutedEventArgs e)
        {

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

        public bool Check() => true;
    }
}
