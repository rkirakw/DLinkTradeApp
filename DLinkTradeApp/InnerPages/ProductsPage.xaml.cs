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

namespace DLinkTradeApp.InnerPages {
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page {
        public static ProductsPage get { get; private set; }
        private ProductsPage() {
            InitializeComponent();
            dataGrid.ItemsSource = Manager.Products.Table;
        }

        static ProductsPage() {
            get = new ProductsPage();
        }

        public void ForceUpdate() {
            // Установить режим сортировки в режим по умолчанию
            for(int i = 0; i < dataGrid.Columns.Count; i++) {
                dataGrid.Columns[i].SortDirection = null;
            }
            // Очистить сортировку из представления DataGrid
            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).SortDescriptions.Clear();

            dataGrid.Items.Refresh();
        }

        private void searchBox_TextInput(object sender, TextCompositionEventArgs e) {
            
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e) {
            // TODO Открытие меню для выбора кол-ва товара и пункта выдачи
            // TODO Сначала добавлять в корзину, а в заказы добавлять ТОЛЬКО ИЗ КОРЗИНЫ

            Product pr = (Product)dataGrid.SelectedItem;
            Manager.Insert("orders", "UserID, ProductID, StorageID, Amount", $"{Manager._uid}, {pr.ID}, 1, 1");
            Manager.Orders.Update();
        }
    }
}
