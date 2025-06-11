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
using System.Windows.Shapes;

namespace IELTSAppProject
{
    /// <summary>
    /// Логика взаимодействия для DialogInputWindow.xaml
    /// </summary>
    public partial class DialogInputWindow : Window
    {
        public  string UserInput { get; set; }

        public DialogInputWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput = InputTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
