using DLinkTradeApp.InnerPages;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Text.Json;

namespace DLinkTradeApp {

    public class Product {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int ProductType { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }

        public BitmapImage Image { get; set; }

        public Product(int id, string name, int type, string cost, string description) {
            ID = id;
            ProductName = name;
            ProductType = type;
            Cost = cost;
            Description = description;
        }
    }
    class Order {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int StorageID { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }

        public Order(int _id, int uid, int pid, int sid, int _amount) {
            ID = _id;
            UserID = uid;
            ProductID = pid;
            StorageID = sid;
            Amount = _amount;
        }
    }
    class ProductType {
        public int ID { get; set; }
        public string TypeName { get; set; }

        public ProductType(int iD, string typeName) {
            ID = iD;
            TypeName = typeName;
        }
    }

    class DataBaseTable<T> {
        public List<T> Table { get; set; }
        public DataTable _table;
        public MySqlDataAdapter dataAdapter;

        public DataBaseTable() {
            Table = new List<T>();
            _table = new DataTable();
        }

        public void Update() {
            _table.Clear();
            dataAdapter.Fill(_table);
        }
    }

    class DataBaseConnectionData {
        public string Server { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
    }

    class UserData {
        public int UID { get; private set; }
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Phone { get; private set; }

        public UserData(int uid, string email, string login, string phone) {
            UID = uid;
            Email = email;
            Login = login;
            Phone = phone;
        }
    }

    internal static class Manager {
        private static Dictionary<string, string> _pages;
        private static Frame _mainFrame;

        // Уникальный идентификатор пользователя
        public static UserData userData { get; private set; }

        public static MySqlConnection GetConnection { get; private set; }


        public static DataBaseTable<Product> Products;

        public static DataBaseTable<Order> Orders;

        public static DataBaseTable<ProductType> ProductTypes;



        public static void Init(Frame mainFrame) {
            _mainFrame = mainFrame;
            _pages = new Dictionary<string, string>() { { "Коммутаторы",       "ProductType=1" },
                                                        { "Маршрутизаторы",    "ProductType=2"},
                                                        { "Межсетевые экраны", "ProductType=3" },
                                                        { "Медиа конвертеры",  "ProductType=4" },
                                                        { "Все продукты",      ""              },};

            Products = new DataBaseTable<Product>();
            Orders = new DataBaseTable<Order>();
            ProductTypes = new DataBaseTable<ProductType>();

            // Установка соеденения с базой данных
            var data = JsonSerializer.Deserialize<DataBaseConnectionData>(File.ReadAllText("config.json"));
            string connStr = $"server={data.Server};user={data.User};database={data.Database};port={data.Port};password={data.Password};";
            try {
                GetConnection = new MySqlConnection(connStr);
                GetConnection.Open();
            }
            catch(MySqlException ex) {
                MessageBox.Show("Access denied");
                MainWindow._instance.Close();
            }


            LoadDataBase(Products, "select * from products");
            LoadDataBase(ProductTypes, "select * from producttypes");
            GetProductTypes();
        }

        private static void LoadDataBase<T>(DataBaseTable<T> data, string sql, bool updateCommand = false) {
            MySqlCommand sqlCmd = new MySqlCommand(sql, GetConnection);

            data.dataAdapter = new MySqlDataAdapter(sqlCmd);
            if (updateCommand) {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(data.dataAdapter);
                MySqlCommand updateCmd = builder.GetUpdateCommand();
                data.dataAdapter.UpdateCommand = updateCmd;
            }
            data._table = new DataTable();
            data.dataAdapter.Fill(data._table); //Запрос на получение выполняется в этой точке
        }


        public static void GetProductTypes() {
            ProductTypes.Table.Clear();
            var data = ProductTypes._table.Select();

            for(int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
                var prodType = new ProductType((int)       data[rowIndex]["ID"],
                                               (string)    data[rowIndex]["TypeName"]);
                ProductTypes.Table.Add(prodType);
            }
        }


        // Получение списка продуктов
        public static void GetProducts(string rowFilter = "") {
            Products.Table.Clear();
            var data = Products._table.Select(rowFilter);


            for(int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
                 var prod = new Product((int)     data[rowIndex]["ID"],
                                        (string)  data[rowIndex]["ProductName"],
                                        (int)     data[rowIndex]["ProductType"],
                                                  data[rowIndex]["Cost"].ToString(),
                                        (string)  data[rowIndex]["ProductDescription"]);
                 using(MemoryStream ms = new MemoryStream((byte[])data[rowIndex]["Image"])) {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    prod.Image = image;
                 }

                 Products.Table.Add(prod);
            }
        }

        // Получение списка заказов
        public static void GetOrders() {
            Orders.Table.Clear();
            var data = Orders._table.Select();


            for (int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
                var order = new Order( (int)     data[rowIndex]["ID"],
                                       (int)     data[rowIndex]["UserID"],
                                       (int)     data[rowIndex]["ProductID"],
                                       (int)     data[rowIndex]["StorageID"],
                                       (int)     data[rowIndex]["Amount"]);

                order.TotalPrice = order.Amount * (int)Products._table.Select($"ID={order.ProductID}")[0]["Cost"];
                Orders.Table.Add(order);
            }
        }

        public static void Insert(string table, string fields, string values) {

            MySqlCommand insertCommand = new MySqlCommand($"insert {table}({fields}) values({values})", GetConnection);
            insertCommand.ExecuteNonQuery();
        }

        public static void Delete(string table, int id) {
            MySqlCommand deleteCommand = new MySqlCommand($"delete from {table} where ID = {id}", GetConnection);
            deleteCommand.ExecuteNonQuery();
        }

        public static bool CheckUser(string login, string password) {
            string cmd = $"select * from users where login = '{login}' and pass = '{password}'";
            MySqlCommand select = new MySqlCommand(cmd, GetConnection);

            var reader = select.ExecuteReader();
            bool userAvailable = reader.HasRows;
            if (reader.Read())
                userData = new UserData((int)reader["ID"], reader["Email"].ToString(), login, reader["Phone"].ToString());
            reader.Close();

            if(userAvailable)
                LoadDataBase(Orders, $"select * from orders where UserID = {userData.UID}", true);


            return userAvailable;
        }

        // Сброс значения инкремента в БД
        public static void ResetAutoINC(string table) {
            MySqlCommand autoIncReset = new MySqlCommand($"call reset_auto_inc('{table}')", GetConnection);
            autoIncReset.ExecuteNonQuery();
        }

        public static string SetCurrentPage(int pageIndex, params object[] p) {
            if(pageIndex == 5) {
                //TODO ...
                GetOrders();
                _mainFrame.Navigate(OrdersPage.get);
                OrdersPage.get.ForceUpdate();
                return "Заказы";
            }
            else if(pageIndex == 6) {
                if (p[0] is Product)
                    _mainFrame.Navigate(new BuyPage(p[0] as Product));
                return "Оформление заказа";
            }

            
            var element = _pages.ElementAt(pageIndex);
            GetProducts(element.Value);

            _mainFrame.Navigate(ProductsViewPage.get);
            ProductsViewPage.get.ForceUpdate();

            return element.Key;
        }

        public static void GoBack() => _mainFrame.GoBack();
    }
}
