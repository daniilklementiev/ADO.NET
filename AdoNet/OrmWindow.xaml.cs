using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace AdoNet
{
    /// <summary>
    /// Interaction logic for OrmWindow.xaml
    /// </summary>
    public partial class OrmWindow : Window
    {
        public ObservableCollection<Entity.Department> Departments { get; set; }
         public ObservableCollection<Entity.Products> Products { get; set; }

        private readonly MySqlConnection _connection;
        public OrmWindow()
        {
            InitializeComponent();
            Departments = new();
            Products = new();
            this.DataContext = this;
            _connection = new(App.Connect);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                MySqlCommand cmd = new() { Connection = _connection };

                #region Load Departments
                cmd.CommandText = "SELECT D.Id, D.Name FROM Departments D";
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Departments.Add(
                    new Entity.Department
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)
                    });
                }
                reader.Close();
                #endregion

                #region Load Products
                cmd.CommandText = "SELECT P.Id, P.Name, P.Price FROM Products P";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(
                    new Entity.Products
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Price = reader.GetFloat(2)
                    });
                }
                reader.Close();
                #endregion

                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Window will be closed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                this.Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListViewItem item)
            {
                if(item.Content is Entity.Department department)
                {
                    MessageBox.Show(department.Name);
                }
            }
        }
    }
}
