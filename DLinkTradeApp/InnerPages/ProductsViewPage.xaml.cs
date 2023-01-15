using DLinkTradeApp.HighLevelPages;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DLinkTradeApp.InnerPages {
    
    public class TypeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object param, CultureInfo culture) {
            if (param != null && param.ToString() == "TYPE") {
                object typeName = Manager.ProductTypes._table.Select($"id={value}")[0]["TypeName"];
                return typeName;
            }
            return "Undefined";
        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public partial class ProductsViewPage : Page {
        public static ProductsViewPage get { get; private set; }
        private ProductsViewPage() {
            InitializeComponent();

            listBox.ItemsSource = Manager.Products.Table;
        }

        public void ForceUpdate() {
            listBox.Items.Refresh();
        }

        static ProductsViewPage() {
            get = new ProductsViewPage();
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e) {
            // TODO Открытие меню для выбора кол-ва товара и пункта выдачи

            Product pr = (Product)(sender as Button).DataContext;
            WorkPage.get.ChangePage(5, pr);
        }
    }
}
