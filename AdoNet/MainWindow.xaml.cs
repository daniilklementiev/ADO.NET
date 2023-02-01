using MySql.Data.MySqlClient;
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
using System.Data;
using System.Data.SqlClient;

namespace AdoNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {

        private MySqlConnection _connection;
        public MainWindow()
        {
            InitializeComponent();
            string host = "sql8.freesqldatabase.com"; // Имя хоста
            string database = "sql8594892"; // Имя базы данных
            string user = "sql8594892"; // Имя пользователя
            string password = "MKIgR3u8MQ"; // Пароль пользователя

            string Connect = "Database=" + database + ";Datasource=" + host + ";User=" + user + ";Password=" + password;
            _connection = new();
            _connection.ConnectionString = Connect;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                StatusConnection.Content = "Open";
                StatusConnection.Foreground = Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                StatusConnection.Content = "Unable to open";
                StatusConnection.Foreground = Brushes.Red;
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        #region Запити без повернення результатів
        private void CreateDepartments_Click(object sender, RoutedEventArgs e)
        {
            // команда - ресурс для передачі SQL запиту до СУБД
            MySqlCommand cmd = new();
            // Обов'язкові параметри команди:
            cmd.Connection = _connection;  // відкрите підключення
            cmd.CommandText =              // та текст SQL запиту
                @"CREATE TABLE Departments(
                 Id          CHAR(36) NOT NULL PRIMARY KEY,
                 Name        VARCHAR(50) NOT NULL
                )";
            /* MySql: CREATE TABLE Departments(
                 Id          CHAR(36) NOT NULL PRIMARY KEY,
                 Name        VARCHAR(50) NOT NULL
             )*/
            try
            {
                cmd.ExecuteNonQuery();  // NonQuery - без повернення рез-ту
                MessageBox.Show("Create OK");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Create error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
            }
            cmd.Dispose();  // команда - unmanaged, потрібно вивільняти ресурс
        }
        #endregion
        /* Д.З. Реалізувати заповнення таблиці Departments
        * (SQL команди - у доданому в Тімс файлі)
        * - додати кнопку <<Fill Departments>>
        * - реалізувати запит за натисненням цієї кнопки
        * - до звіту додати скріншот таблиці у БД
        */
        #region HW
        private void FillDepartments_Click(object sender, RoutedEventArgs e)
        {
            // команда - ресурс для передачі SQL запиту до СУБД
            MySqlCommand cmd = new();
            // Обов'язкові параметри команди:
            cmd.Connection = _connection;  // відкрите підключення
            cmd.CommandText =              // та текст SQL запиту
                @"INSERT INTO Departments 
                        ( Id, Name )
                  VALUES 
                    ( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT отдел'            ), 
                    ( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'         ), 
                    ( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности' ), 
                    ( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'        ),
                    ( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'          ), 
                    ( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Отдел продаж'        ), 
                    ( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба'  )";
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Departments filled");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Filling error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            cmd.Dispose(); 
        }

        #endregion
    }
}
