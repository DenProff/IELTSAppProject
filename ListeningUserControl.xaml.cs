using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Newtonsoft.Json;
using System;
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
using Path = System.IO.Path;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ListeningControl.xaml
    /// </summary>
    public partial class ListeningUserControl : UserControl, ICheckable
    {
        public ListeningUserControl(ListeningTask data)
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

            this.DataContext = data; // Запись в Binding информации из свойств объекта data
        }

        public bool Check() // Данный метод должен реализовывать проверку ответов пользователя
        {
            //rightAnswer0.Text = IsAnswerCorrect(task.Answer[0], answer61, answer62, answer63)
            //    ? "Правильный ответ!" : "Неправильный ответ!";

            //rightAnswer1.Text = IsAnswerCorrect(task.Answer[6], answer71, answer72, answer73)
            //    ? "Правильный ответ!" : "Неправильный ответ!";

            //rightAnswer2.Text = IsAnswerCorrect(task.Answer[7], answer81, answer82, answer83)
            //    ? "Правильный ответ!" : "Неправильный ответ!";

            //rightAnswer3.Text = IsAnswerCorrect(task.Answer[8], answer91, answer92, answer93)
            //    ? "Правильный ответ!" : "Неправильный ответ!";

            //rightAnswer4.Text = IsAnswerCorrect(task.Answer[9], answer01, answer02, answer03)
            //    ? "Правильный ответ!" : "Неправильный ответ!";
            throw new NotImplementedException();
        }

        private void SimpleAudioPlayer_Loaded(object sender, RoutedEventArgs e)
        {

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
                    Process.Start("hh.exe", $"{chmPath}::/listening.htm");
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

        // Метод для проверки правильности ответа в RadioButton
        private bool IsAnswerCorrect(char expected, params RadioButton[] options)
        {
            return options.Any(btn => btn.IsChecked == true && btn.Content?.ToString()[0] == expected);
        }
    }
}
