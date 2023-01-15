using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Page {
        public static WorkPage get { get; private set; }
        private WorkPage() {
            InitializeComponent();

            Manager.Init(mainFrame);
        }

        static WorkPage() {
            get = new WorkPage();
        }

        private void ChangePageButton(object sender, RoutedEventArgs e) {
            int index = int.Parse((e.OriginalSource as Button).Tag as string);
            ChangePage(index);
        }

        // Переключает страницу и устанавливает заголовок
        public void ChangePage(int page, params object[] param) {
            pageName.Content = Manager.SetCurrentPage(page, param);
        }
    }
}
