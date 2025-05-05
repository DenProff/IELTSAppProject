using System;
using System.Collections.Generic;
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

            foreach (UIElement elem in mainScreen.Children)
            {
                if (elem is Button)
                {
                    if (elem == lessTime)
                        ((Button)elem).Click += Button_Click;
                    if (elem == teacherMode)
                        ((Button)elem).Click += Button_Click_1;
                    if (elem == developerMode)
                        ((Button)elem).Click += Button_Click_2;
                    if (elem == continueWork)
                        ((Button)elem).Click += Button_Click_3;
                    if (elem == chooseLanguage)
                        ((Button)elem).Click += Button_Click_4;
                }
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Получаем относительный путь к файлу в проекте
            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string mp3Path = Path.Combine(projectDir, "audio", "cocoJambo.mp3");

            // 2. Конвертируем MP3 в WAV (временный файл)
            string wavPath = Path.Combine(Path.GetTempPath(), "converted.wav");

            using (var mp3Reader = new Mp3FileReader(mp3Path))
            using (var waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader))
            {
                WaveFileWriter.CreateWaveFile(wavPath, waveStream);
            }

            // 3. Читаем WAV файл в byte[]
            byte[] wavBytes = File.ReadAllBytes(wavPath);

            Console.WriteLine($"Размер WAV файла: {wavBytes.Length} байт");

            // Удаляем временный файл (опционально)
            File.Delete(wavPath);


            SpeakingTask task = new SpeakingTask("Проблема фимоза в человеческом обществе", wavBytes, 5);
            NavigationService?.Navigate(new SpeakingPage(task));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FirstTestPage());

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
