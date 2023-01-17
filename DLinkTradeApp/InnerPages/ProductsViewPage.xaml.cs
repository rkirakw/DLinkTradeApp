using DLinkTradeApp.HighLevelPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
                object typeName = Manager.ProductTypes.Table.Find(pt => pt.ID == (int)value).TypeName;
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

           
            var types = new List<ProductType>(Manager.ProductTypes.Table);
            types.Insert(0, new ProductType(-1, "Все типы"));
            typeBox.SelectedIndex = 0;

            typeBox.ItemsSource = types;
            FilterUpdate();
        }

        public void ForceUpdate() {
            typeBox.SelectedIndex = 0;
            searchBox.Text = string.Empty;
            FilterUpdate();
            listBox.Items.Refresh();
        }

        static ProductsViewPage() {
            get = new ProductsViewPage();
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e) {
            // TODO Открытие меню для выбора кол-ва товара и пункта выдачи

            Product pr = (Product)(sender as Button).DataContext;
            WorkPage.get.ChangePage(6, pr);
        }

        private void FilterUpdate() {
            var result = Manager.Products.Table;
            if (typeBox.SelectedIndex > 0) {
                var type = typeBox.SelectedItem as ProductType;
                result = result.Where(p => p.ProductType == type.ID).ToList();
            }

            result = result.Where(p => p.ProductName.ToLower().Contains(searchBox.Text.ToLower())).ToList();

            listBox.ItemsSource = result;
        }

        private void searchBox_Changed(object sender, TextChangedEventArgs e) {
            FilterUpdate();
        }

        private void typeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            FilterUpdate();
        }
    }
}
