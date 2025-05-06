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

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для SpeakingPage.xaml
    /// </summary>
    public partial class SpeakingPage : Page
    {
        public SpeakingTask Task { get; set; }
        static private bool isRecordingInProgress = false;
        static private bool isRecordingDone = false;
        static private bool isPlayingUserAnswerInProgress = false;
        static private bool isPlayingRealAnswerInProgress = false;
        static string taskText = @"You should:
- Express your attitude toward the given problem.
- Analyze the problem and justify your position, providing at least 2 supporting arguments.
- Summarize your ideas.

Your speech should last no less than 3 minutes and no longer than 5 minutes.";

        public SpeakingPage(SpeakingTask task)
        {
            Task = task;
            InitializeComponent();
            taskTextBlock.Text = taskText;
            topicTextBlock.Text = task.TaskText;
            idTextBox.Text += (task.id).ToString();
            recommendedTimeTextBlock.Text += task.RecommendedTime.ToString() + "мин.";
        }

        private void StartRecord(object sender, RoutedEventArgs e)
        {
            if (!isRecordingInProgress && !isRecordingDone)
            {
                SoundControl.StartRecording();
                isRecordingInProgress = true;
                inputRecordingStatusTextBox.Text = "Запись идёт.";
                inputRecordingStatusTextBox.Background = Brushes.LightGreen;
            }
        }

        private void StopRecord(object sender, RoutedEventArgs e)
        {
            if (!isRecordingInProgress) return;

            try
            {
                SoundControl.StopRecording(); // Остановка записи
                Task.UserAnswer = File.ReadAllBytes(SoundControl.OutputFilePath);// Чтение файла
            }
            catch (Exception ex)
            {
                // Обновление интерфейса
                isRecordingInProgress = false;
                isRecordingDone = true;
                inputRecordingStatusTextBox.Text = "Ответ сохранён.";
                inputRecordingStatusTextBox.Background = Brushes.LightGreen;
            }
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
    }
}
