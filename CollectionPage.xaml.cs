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
        public CollectionPage(TaskCollection taskCollection)
        {
            InitializeComponent();
            foreach (int task in taskCollection)
            {
                // Обработка id-шек и подтягивание из json - не реализовано
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
