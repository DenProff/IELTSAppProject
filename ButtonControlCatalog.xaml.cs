using DocumentFormat.OpenXml.VariantTypes;
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
    /// Логика взаимодействия для ButtonControlCatalog.xaml
    /// </summary>
    public partial class ButtonControlCatalog : UserControl
    {
        public ButtonControlCatalog(TaskCollection task)
        {
            InitializeComponent();

            this.DataContext = task;

            var multiBind = new MultiBinding();

            multiBind.StringFormat = "Вариант: {0} Название: {1} Дата публикации: {2}";

            multiBind.Bindings.Add(new Binding("VariantId"));
            multiBind.Bindings.Add(new Binding("VariantName"));
            multiBind.Bindings.Add(new Binding("DateOfAccess"));

            collection.SetBinding(Button.ContentProperty, multiBind);
        }

        private void collection_Click(object sender, RoutedEventArgs e)
        {
            //CollectionPage page = new CollectionPage(task);
        }

        private void convertCollection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
