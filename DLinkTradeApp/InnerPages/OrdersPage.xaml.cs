using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DLinkTradeApp.InnerPages {
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    /// 

    public class OrderConverter : IValueConverter {
        public object Convert(object value, Type targetType, object param, CultureInfo culture) {
            if (param != null && param.ToString() == "NAME")
                return Manager.Products._table.Select($"id={value}")[0]["ProductName"];
            else if (param != null && param.ToString() == "TYPE") {
                int typeID = (int)Manager.Products._table.Select($"id={value}")[0]["ProductType"];
                object typeName = Manager.ProductTypes.Table.Find(pt => pt.ID == typeID).TypeName;
                return typeName;
            }
            else if (param != null && param.ToString() == "COST")
                return Manager.Products._table.Select($"id={value}")[0]["Cost"];
            else if (param != null && param.ToString() == "IMAGE") {
                byte[] data = (byte[])Manager.Products._table.Select($"id={value}")[0]["Image"];
                using(MemoryStream ms = new MemoryStream(data)) {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = ms;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    return img;
                }
            }

            return "Undefined";
        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public partial class OrdersPage : Page {
        public static OrdersPage get { get; private set; }
        private OrdersPage() {
            InitializeComponent();

            listBox.ItemsSource = Manager.Orders.Table;
        }

        public void ForceUpdate() {
            listBox.Items.Refresh();
        }

        static OrdersPage() {
            get = new OrdersPage();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) {
            Order order = (Order)(sender as Button).DataContext;
            Manager.Delete("orders", order.ID);
            Manager.Orders.Update();
            Manager.GetOrders();
            ForceUpdate();
            Manager.ResetAutoINC("orders");

        }
    }
}
