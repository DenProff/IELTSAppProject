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
            NavigationService?.Navigate(new SpeakingPage());
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
