using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdoNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string host = "sql8.freesqldatabase.com"; // Имя хоста
        public static string database = "sql8594892"; // Имя базы данных
        public static string user = "sql8594892"; // Имя пользователя
        public static string password = "MKIgR3u8MQ"; // Пароль пользователя

        public static readonly string Connect = "Database=" + database + ";Datasource=" + host + ";User=" + user + ";Password=" + password + ";CharSet = utf8";
    }
}
