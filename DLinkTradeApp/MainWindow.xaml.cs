using DLinkTradeApp.HighLevelPages;
using System;
using System.Windows;
using System.Windows.Input;

namespace DLinkTradeApp {
    public partial class MainWindow : Window {
        public static MainWindow _instance { get; private set; }
        public MainWindow() {
            InitializeComponent();
            _instance = this;

            // Обращение к WorkPage.get для вызова статического конструктора
            var _temp = WorkPage.get;
            // Переход на страницу авторизации
            screenFrame.Navigate(LogPage.get);
        }

        // Подавить событие, если была нажата клавиша Tab
        private void Page_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Tab)
               e.Handled = true;
        }

        // Закрыть соединение с базой данных
        private void Window_Closed(object sender, EventArgs e) {
            Manager.GetConnection.Close();
        }
    }
}
