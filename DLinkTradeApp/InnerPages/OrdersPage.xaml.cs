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
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    /// 

    public class OrderConverter : IValueConverter {
        public object Convert(object value, Type targetType, object param, CultureInfo culture) {
            if (param != null && param.ToString() == "NAME")
                return Manager.Products._table.Select($"id={value}")[0]["ProductName"];
            else if (param != null && param.ToString() == "TYPE") {
                object typeID = Manager.Products._table.Select($"id={value}")[0]["ProductType"];
                object typeName = Manager.ProductTypes._table.Select($"id={typeID}")[0]["TypeName"];
                return typeName;
            }
            else if (param != null && param.ToString() == "COST")
                return Manager.Products._table.Select($"id={value}")[0]["Cost"];

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

            dataGrid.ItemsSource = Manager.Orders.Table;
        }

        public void ForceUpdate() {
            // Установить режим сортировки в режим по умолчанию
            for (int i = 0; i < dataGrid.Columns.Count; i++) {
                dataGrid.Columns[i].SortDirection = null;
            }
            // Очистить сортировку из представления DataGrid
            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).SortDescriptions.Clear();

            dataGrid.Items.Refresh();
        }

        static OrdersPage() {
            get = new OrdersPage();
        }
    }
}
