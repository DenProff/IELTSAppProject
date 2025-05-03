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
    /// Логика взаимодействия для FirstTestPage.xaml
    /// </summary>
    public partial class FirstTestPage : Page
    {
        public FirstTestPage()
        {
            InitializeComponent();

            foreach (UIElement elem in firstTest.Children)
            {
                if (elem is Button)
                {
                    if (elem == testVar)
                        ((Button)elem).Click += Button_Click;
                    if (elem == @continue)
                        ((Button)elem).Click += Button_Click_1;
                    if (elem == turnBack)
                        ((Button)elem).Click += Button_Click_2;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }
    }
}
