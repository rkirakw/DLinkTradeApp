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
    /// Логика взаимодействия для BuyPage.xaml
    /// </summary>

    partial class BuyPage : Page {
        private Product _pr;

        public int ProdAmount { get; set; } = 1;

        public BuyPage(Product prod) {
            InitializeComponent();
            _pr = prod;
            prodName.Text += prod.ProductName;
            prodCost.Text += prod.Cost;
            prodImg.Source = prod.Image;
        }

        private void buyButton_Click(object sender, RoutedEventArgs e) {
            Manager.Insert("orders", "UserID, ProductID, StorageID, Amount", $"{Manager.userData.UID}, {_pr.ID}, 1, {(int)prodAmount.Value}");
            Manager.Orders.Update();
            Manager.GoBack();
        }

        private void prodAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            (sender as Slider).SelectionEnd = e.NewValue;
        }
    }
}
