
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для TaskCatalogPage.xaml
    /// </summary>
    public partial class TaskCatalogPage : Page
    {
        
        public TaskCatalogPage()
        {
            InitializeComponent();

            foreach (UIElement elem in taskCatalog.Children)
            {
                if (elem is Button)
                {
                    if (elem == turnBack)
                        ((Button)elem).Click += turnBack_Click;
                    if (elem == statistic)
                        ((Button)elem).Click += statistic_Click;
                    if (elem == mistakes)
                        ((Button)elem).Click += mistakes_Click;
                    if (elem == collections)
                        ((Button)elem).Click += collections_Click;
                    if (elem == language)
                        ((Button)elem).Click += language_Click;
                    //if (elem == convert)
                    //    ((Button)elem).Click += varOfExam_Click;
                    //if (elem == newCollections)
                    //    ((Button)elem).Click += newCollections_Click;
                    //if (elem == testVar)
                    //    ((Button)elem).Click += testVar_Click;


                }
            }
        }

        private void turnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChooseModulePage());
        }

        private void statistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mistakes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void collections_Click(object sender, RoutedEventArgs e)
        {

        }

        private void language_Click(object sender, RoutedEventArgs e)
        {

        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addToCollection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            Hel
        }
    }
}
