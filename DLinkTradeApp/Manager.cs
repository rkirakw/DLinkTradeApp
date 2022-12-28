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

namespace DLinkTradeApp {

    class Product {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }

        public BitmapImage Image { get; set; }

        public Product(int id, string name, string type, string cost, string description) {
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


    internal static class Manager {
        private static Dictionary<string, string> _pages;
        private static Frame _mainFrame;

        // Уникальный идентификатор пользователя
        public static int _uid { get; private set; }

        public static MySqlConnection GetConnection { get; private set; }


        public static DataBaseTable<Product> Products;

        public static DataBaseTable<Order> Orders;

        public static DataBaseTable<ProductType> ProductTypes;





        public static void Init(Frame mainFrame, int UID = 1) {
            _mainFrame = mainFrame;
            _uid = UID;
            _pages = new Dictionary<string, string>() { { "Коммутаторы",       "ProductType=1" },
                                                        { "Маршрутизаторы",    "ProductType=2"},
                                                        { "Межсетевые экраны", "ProductType=3" },
                                                        { "Медиа конвертеры",  "ProductType=4" } };

            Products = new DataBaseTable<Product>();
            Orders = new DataBaseTable<Order>();
            ProductTypes = new DataBaseTable<ProductType>();

            // Установка соеденения с базой данных
            string connStr = "server=localhost;user=root;database=productsdb;port=3306;password=pqek100T;";
            GetConnection = new MySqlConnection(connStr);
            GetConnection.Open();


            LoadDataBase(Products, "select * from products");
            LoadDataBase(ProductTypes, "select * from producttypes");
            LoadDataBase(Orders, $"select * from orders where UserID = {_uid}", true);
            
        }

        private static void LoadDataBase<T>(DataBaseTable<T> data, string sql, bool updateCommand = false) {
            MySqlCommand sqlCmd = new MySqlCommand(sql, GetConnection);
            sqlCmd.ExecuteNonQuery();


            data.dataAdapter = new MySqlDataAdapter(sqlCmd);
            if (updateCommand) {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(data.dataAdapter);
                MySqlCommand updateCmd = builder.GetUpdateCommand();
                data.dataAdapter.UpdateCommand = updateCmd;
            }
            data._table = new DataTable();
            data.dataAdapter.Fill(data._table);
        }


        // Получение списка продуктов
        public static void GetProducts(string rowFilter = "") {
            Products.Table.Clear();
            var data = Products._table.Select(rowFilter);


            for(int rowIndex = 0; rowIndex < data.Length; rowIndex++) {
                 var prod = new Product((int)     data[rowIndex]["ID"],
                                        (string)  data[rowIndex]["ProductName"],
                                                  data[rowIndex]["ProductType"].ToString(),
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
                Orders.Table.Add(order);
            }
        }

        public static void Insert(string table, string fields, string values) {

            MySqlCommand insertCommand = new MySqlCommand($"insert {table}({fields}) values({values})", GetConnection);
            insertCommand.ExecuteNonQuery();
        }

        public static string SetCurrentPage(int pageIndex) {
            if(pageIndex == 4) {
                //TODO ...
                GetOrders();
                _mainFrame.Navigate(OrdersPage.get);
                OrdersPage.get.ForceUpdate();
                return "Заказы";
            }

            
            var element = _pages.ElementAt(pageIndex);
            GetProducts(element.Value);

            _mainFrame.Navigate(ProductsViewPage.get);
            ProductsViewPage.get.ForceUpdate();

            return element.Key;
        }
    }
}
