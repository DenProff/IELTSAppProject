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
using Newtonsoft.Json;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для ReadingPage.xaml
    /// </summary>
    public partial class ReadingPage : Page
    {
        public ReadingPage()
        {
            InitializeComponent();

            foreach (UIElement elem in readingPart.Children)
            {
                if (elem is Button)
                {
                    if (elem == turnBack)
                        ((Button)elem).Click += turnBack_Click;
                    if (elem == addToCollection)
                        ((Button)elem).Click += Button_Click;
                    if (elem == convertToDocx)
                        ((Button)elem).Click += convertToDocx_Click;
                }
            }


        }

        private void turnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convertToDocx_Click(object sender, RoutedEventArgs e)
        {

        }

        //public static string NumAndTask()
        //{

        //}
    }
}
