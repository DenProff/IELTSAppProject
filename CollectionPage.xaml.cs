using System;
using System.Collections.Generic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для CollectionPage.xaml
    /// </summary>
    public partial class CollectionPage : Page
    {
        public CollectionPage()
        {
            InitializeComponent();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\.."));
            string audio = Path.Combine(projectRoot, "audio", "cocoJambo.mp3");
            byte[] data = SoundControl.ConvertMp3ToWavBytes(audio);
            SpeakingTask task = new SpeakingTask("Is it worth buying an apartment on the first floor?", data, 5);
            // Загружаем три разных типа заданий
            SpeakingFrame.Navigate(new SpeakingPage(task));
            ListeningFrame.Navigate(new ListeningPage());
            WritingFrame.Navigate(new WritingPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FirstTestPage());
        }
    }
}
