using DLinkTradeApp.HighLevelPages;
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

namespace DLinkTradeApp {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static MainWindow _instance { get; private set; }
        public MainWindow() {
            InitializeComponent();
            _instance = this;

            //TODO Заменить на LogPage.get!!!
            var _temp = WorkPage.get;
            screenFrame.Navigate(LogPage.get);
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e) {
            var obj = (e.Source as FrameworkElement)?.Tag ?? "__none";
            if ((obj.Equals("__none") || !obj.Equals("__handle_tab")) && e.Key == Key.Tab)
                e.Handled = true;
        }

        private void Window_Closed(object sender, EventArgs e) {
            //Закрытие соединения с БД
            Manager.GetConnection.Close();
        }
    }
}
