using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace DLinkTradeApp.HighLevelPages {

    public class LogData : IDataErrorInfo, INotifyPropertyChanged {
        public static bool _logError = false;
        Dictionary<string, string> errors = new Dictionary<string, string>();
        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;


        private string login;
        public string Login {
            get => login;
            set {
                login = value;
                OnPropertyChanged("Login");
                if (login.Length == 0) {
                    errors["Login"] = "Поле не может быть пустым";
                }
                else if (_logError)
                    errors["Login"] = "Неправильный логин или пароль";
                else
                    errors["Login"] = null;
            }
        }

        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    public partial class LogPage : Page {
        public static LogPage get { get; private set; }
        public static LogData logData { get; private set; }

        private LogPage() {
            InitializeComponent();
            logData = new LogData();
            fieldPanel.DataContext = logData;
        }

        static LogPage() {
            get = new LogPage();
        }

        private void LogButtonClick(object sender, RoutedEventArgs e) {
            LogIn();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter)
                LogIn();
        }

        private void LogIn() {
            bool userAvailable = Manager.CheckUser(loginText.Text, pass.Password);
            if (userAvailable)
                MainWindow._instance.screenFrame.Navigate(WorkPage.get);
            else
                LogData._logError = true;
        }
    }
}
