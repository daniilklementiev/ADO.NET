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
            string Connect = App.Connect;
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
            ShowMonitorDepartments();
            ShowDepartmentsView();
            ShowManagersView();
            ShowProductsView();
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
                ) ENGINE = INNODB DEFAULT CHARSET = UTF8";
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

        #region Запити з одним (скалярним) результатом
        private void ShowMonitorDepartments()
        {
            using MySqlCommand cmd = new("SELECT COUNT(*) FROM Departments", _connection);
            using MySqlCommand cmd_prod = new("SELECT COUNT(*) FROM Products", _connection);
            using MySqlCommand cmd_man = new("SELECT COUNT(*) FROM Managers", _connection);
            try
            {
                object res = cmd.ExecuteScalar();
                object res_pr = cmd_prod.ExecuteScalar();
                object res_man = cmd_man.ExecuteScalar();
                int cnt = Convert.ToInt32(res);
                int cnt_prod = Convert.ToInt32(res_pr);
                int cnt_man = Convert.ToInt32(res_man);
                StatusDepartments.Content = cnt.ToString();
                StatusProducts.Content = cnt_prod.ToString();
                StatusManagers.Content = cnt_man.ToString();
            }
            catch (MySqlException sql)
            {
                MessageBox.Show(sql.Message, "SQL Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                StatusDepartments.Content = "--------";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Type Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                StatusDepartments.Content = "--------";
            }
        }
        #endregion

        private void CreateProducts_Click(object sender, RoutedEventArgs e)
        {
            string sql = @"CREATE TABLE Products(
                 Id          CHAR(36) NOT NULL PRIMARY KEY,
                 Name        VARCHAR(50) NOT NULL,
                 Price       FLOAT NOT NULL
                ) ENGINE = INNODB DEFAULT CHARSET = UTF8";
            using MySqlCommand cmd = new(sql, _connection);
            try
            {
                cmd.ExecuteNonQuery();
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
            cmd.Dispose();
        }

        private void FillProducts_Click(object sender, RoutedEventArgs e)
        {
            // команда - ресурс для передачі SQL запиту до СУБД
            MySqlCommand cmd = new();
            // Обов'язкові параметри команди:
            cmd.Connection = _connection;  // відкрите підключення
            cmd.CommandText =              // та текст SQL запиту
                @"INSERT INTO Products
	                    ( Id, Name,	Price	)
                  VALUES
                      ( 'DA1E17BB-A90D-4C79-B801-5462FB070F57', N'Гвоздь 100мм',			10.50	),
                      ( 'A8E6BE17-5447-4804-AB61-F31ABF5A76D3', N'Шуруп 4х35',			4.25	),
                      ( '21B0F444-2E4F-47D8-80C1-E69BF1C34CA8', N'Гайка М4',				6.50	),
                      ( '2DCA5E44-B06D-4613-BB6A-D3BC91430BFE', N'Гровер М4',			    5.99	),
                      ( '64A4DF8A-0733-4BE9-AABA-C01B4EC3612A', N'Болт 4х60',			    9.98	),
                      ( 'B6D20749-B495-4B1A-BA1C-80B88E78B7CD', N'Гвоздь 80мм',			19.98	),
                      ( '7B08197B-C55F-4389-891F-BF12A575DFFB', N'Отвертка PZ2',			35.50	),
                      ( '870DA1A9-44F4-4018-B7FC-727A2058FAF0', N'Шуруповерт',			799		),
                      ( '8FF90E21-DCDB-4D55-A557-7C6D57DBB029', N'Молоток',				216.50	),
                      ( 'F7F1E576-AF8D-4749-869E-4A794FE69D42', N'Набор ""Новосел""',		52.40	),
                      ( 'BB29F63D-1261-41F2-89E8-88F44D5EC409', N'Сверло 6х80',			39.98	),
                      ( 'D17A4442-0A71-4673-B450-36929048ADEF', N'Шуруп 5х45',			5.98	),
                      ( '69B125D7-99CC-42D6-A6FA-46687F333749', N'Винт ""потай"" 3х16',		3.98	),
                      ( '94BC671A-A6B6-417A-BC9F-8AE4871A58EC', N'Дюбель 6х60',			5.50	),
                      ( 'EFC6578A-00B7-4766-A7E3-79CDBA8C294B', N'Органайзер для шурупов',199		),
                      ( '9654271B-AB52-4225-A30C-D75054B1733F', N'Лазерный дальномер',	1950	),
                      ( 'F2585221-1ACA-4EFE-A5E8-C2F4534D1F92', N'Дрель электрическая',	990		),
                      ( '4A550D3B-D1F2-40EF-AE4E-963612C6713A', N'Сварочный аппарат',		2099	),
                      ( '17DB11D1-F50E-4CF4-9C54-CF1BD45802EA', N'Электроды 3мм',			49.98	),
                      ( '7264D33A-16B9-4E22-B3F1-63D6DAE60078', N'Паяльник 40 Вт',		199.98	)";
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Products filled");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Filling error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            cmd.Dispose();
        }

        private void CreateManagers_Click(object sender, RoutedEventArgs e)
        {
            string sql = @"CREATE TABLE Managers (
	                        Id			CHAR(36) NOT NULL PRIMARY KEY,
	                        Surname		VARCHAR(50) NOT NULL,
	                        Name		VARCHAR(50) NOT NULL,
	                        Secname		VARCHAR(50) NOT NULL,
	                        Id_main_dep CHAR(36) NOT NULL ,
	                        Id_sec_dep	CHAR(36) ,
	                        Id_chief	CHAR(36),

	                        FOREIGN KEY( Id_main_dep ) REFERENCES Departments( Id ),
	                        FOREIGN KEY( Id_sec_dep ) REFERENCES Departments( Id )
                          )  ENGINE = INNODB DEFAULT CHARSET = UTF8 ;";
            using MySqlCommand cmd = new(sql, _connection);
            try
            {
                cmd.ExecuteNonQuery();
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
            cmd.Dispose();
        }

        private void FillManagers_Click(object sender, RoutedEventArgs e)
        {
            // команда - ресурс для передачі SQL запиту до СУБД
            MySqlCommand cmd = new();
            // Обов'язкові параметри команди:
            cmd.Connection = _connection;  // відкрите підключення
            cmd.CommandText =              // та текст SQL запиту
                @"INSERT INTO Managers 
	( Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chief )
VALUES 
	( '743C93F2-4717-4E81-A093-69903476E176',  N'Носков',	N'Орест',		N'Ярославович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										null	),
	( '63531753-4D76-4A93-AD15-C727FFECA6AB',  N'Никитин',	N'Станислав',	N'Брониславович',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'3618D1D1-32DE-40B5-B823-9F82924A3CAF'		),
	( 'CDE086E1-D25C-4251-A234-10727818EE28',  N'Воронов',	N'Александр',	N'Леонидович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null	),
	( '0B2BE83A-7FB4-403B-8CE8-37BE257B038C',  N'Евдокимов',N'Клим',		N'Викторович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										null	),
	( '7585D790-6E5A-4F73-A85C-4F9BD883D811',  N'Жуков',	N'Влад',		N'Виталиевич',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	),
	( '45489FE7-86C8-4FA1-9D79-A82197566BF3',  N'Кулагин',	N'Максим',		N'Вадимович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null	),
	( '0017AAAE-3E22-462D-9031-4276A9788D51',  N'Журавлёв',	N'Зигмунд',		N'Владимирович',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'FEA65EE4-A8A0-425B-8F11-3896C1E2197E'		),
	( '521C07BE-6FBD-411F-BCCB-93E2672BD50E',  N'Соболев',	N'Нестор',		N'Юхимович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										null	),
	( '381C2888-1CB0-41FA-9650-48B953F31EF6',  N'Беляков',	N'Олег',		N'Грегориевич',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'663C3142-1C9D-4957-800D-F6C6824B9C88'		),
	( 'E1AC29AD-122E-474D-926A-F93AC636F605', N'Моисеев',	N'Конрад',		N'Леонидович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
	( '39D57DFB-8DA7-49C9-AE8D-464509618F02', N'Гуляев',	N'Семён',		N'Юхимович',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										null	),
	( '542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9', N'Назаров',	N'Сергей',		N'Платонович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
	( 'FE7E578E-5FC8-4D80-AD6B-500DDF2506C4', N'Рожков',	N'Радислав',	N'Дмитриевич',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'7A88B1B9-0216-4259-8BA6-C123ABB3C6A8'		),
	( '7B8219FC-9FD2-431E-985C-7CAA6E9BD013', N'Герасимов',	N'Лука',		N'Грегориевич',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
	( '23D52416-D994-4564-A106-1FDF5FECEF25', N'Куликов',	N'Заур',		N'Иванович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'23DBE38C-0ED4-4E90-8BC7-F168134E8674'		),
	( 'EE860EE3-6CCA-4EA3-A2F1-FB79F4FC823A', N'Корнилов',	N'Ярослав',		N'Романович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'676D8ED4-8307-4196-9776-107C40C1DF84'		),
	( 'DD860E7E-C2F0-47A6-BA29-165BE015E5A2', N'Князев',	N'Клим',		N'Эдуардович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
	( '267F7528-2D4B-4063-A2C8-98E8F19FB6EE', N'Кириллов',	N'Герасим',		N'Анатолиевич',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'207CDCF2-89AD-49A5-A669-A082FA9CCCBA'		),
	( 'FEA65EE4-A8A0-425B-8F11-3896C1E2197E', N'Галкин',	N'Пётр',		N'Максимович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
	( 'D13F3CCA-B9F8-4BC1-96F4-C80583928E55', N'Бородай',	N'Люций',		N'Львович',			'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										'DC268B00-1727-4381-9878-6DA1BFEF2701'		),
	( '5FE63A0F-C1AE-44BE-9397-0F7DB0B95C1F', N'Спивак',	N'Оливер',		N'Иванович',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'29219DB8-16A0-4046-A7E1-6E455B0559CD'		),
	( 'DC268B00-1727-4381-9878-6DA1BFEF2701', N'Ершов',		N'Владлен',		N'Богданович',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'868F6394-3CA3-4700-90BB-6B73EC6719A7'		),
	( '2FA70965-5BCE-44F0-B6DD-2AF6072EB8B0', N'Комаров',	N'Адриан',		N'Петрович',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										null	),
	( '1166ECDD-63C8-42FC-A68A-C292176A7B04', N'Веселов',	N'Роберт',		N'Евгеньевич',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'C5F771FB-A645-4BA1-8155-F3F5002B2B89'		),
	( '0989E3A2-3D6D-4BC3-A538-C4055F9A09DD', N'Данилов',	N'Добрыня',		N'Львович',			'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'23DBE38C-0ED4-4E90-8BC7-F168134E8674'		),
	( '6CBEA09E-E3E4-4DD3-A6C5-ED9CCD986BC0', N'Журавлёв',	N'Аким',		N'Петрович',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	),
	( '676D8ED4-8307-4196-9776-107C40C1DF84', N'Ерёменко',	N'Кристиан',	N'Евгеньевич',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'7B8219FC-9FD2-431E-985C-7CAA6E9BD013'		),
	( 'FF559AE5-64B6-459E-9771-CB36130B3B75', N'Туров',		N'Станислав',	N'Михайлович',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										'435EEE28-E5EA-4EC9-9F01-DE884DFD6292'		),
	( '1A930DE7-647B-4A32-AD3B-0CAF4528B356', N'Шумейко',	N'Абрам',		N'Романович',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
	( '3618D1D1-32DE-40B5-B823-9F82924A3CAF', N'Бобылёв',	N'Всеволод',	N'Ярославович',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
	( '66034616-24E5-4E90-815F-476EB0CBB6B1', N'Гурьева',	N'Антонина',	N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'FEA65EE4-A8A0-425B-8F11-3896C1E2197E'		),
	( 'C5F771FB-A645-4BA1-8155-F3F5002B2B89', N'Павлик',	N'Ника',		N'Эдуардовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		'8939ED0C-BBDB-435E-923E-68158D2153C6'		),
	( '15F36ECC-EF25-495F-ADFF-169DB3339B88', N'Копылова',	N'Екатерина',	N'Дмитриевна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										'05E31241-7274-43B5-8B59-9A62D725E54F'		),
	( '101BE2B1-C0AF-493E-BBF2-C8D8E4EB826C', N'Корнейчук',	N'Нина',		N'Платоновна',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'2B3170C4-3063-43E6-985D-A38D9E45AF09'		),
	( '868F6394-3CA3-4700-90BB-6B73EC6719A7', N'Гордеева',	N'Капитолина',	N'Станиславовна',	'1EF7268C-43A8-488C-B761-90982B31DF4E',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null	),
	( '05E31241-7274-43B5-8B59-9A62D725E54F', N'Майборода',	N'Алёна',		N'Александровна',	'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'E1AC29AD-122E-474D-926A-F93AC636F605'		),
	( '1ADC048C-E346-47C3-8C35-7AD4FDAA6EB7', N'Шубина',	N'Екатерина',	N'Викторовна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null	),
	( '435EEE28-E5EA-4EC9-9F01-DE884DFD6292', N'Лазарева',	N'Вера',		N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
	( '0889C51E-7728-4ABD-9987-3588D48B54A9', N'Кобзар',	N'Полина',		N'Львовна',			'131EF84B-F06E-494B-848F-BB4BC0604266',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9'		),
	( '46D73A48-3906-44F4-A4B4-E29F1CC40B4F', N'Милославска',N'Инна',		N'Эдуардовна',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'435EEE28-E5EA-4EC9-9F01-DE884DFD6292'		),
	( 'EFEF5433-7E26-43A3-A737-3BB032D7D88A', N'Степанова',	N'Нина',		N'Михайловна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										'63531753-4D76-4A93-AD15-C727FFECA6AB'		),
	( '55FF549E-1489-4B4A-9482-B843CD70C546', N'Ялова',		N'Любовь',		N'Ивановна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null	),
	( '79679ED4-0CCD-480A-8D5B-4A68287DE6C4', N'Макарова',	N'Полина',		N'Васильевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'0B2BE83A-7FB4-403B-8CE8-37BE257B038C'		),
	( '29219DB8-16A0-4046-A7E1-6E455B0559CD', N'Дементьева',N'Альбина',		N'Ивановна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null	),
	( '13DED219-A580-4FF8-8211-90A408B0AFA6', N'Егорова',	N'Ярослава',	N'Романовна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										'1166ECDD-63C8-42FC-A68A-C292176A7B04'		),
	( '2B3170C4-3063-43E6-985D-A38D9E45AF09', N'Коваленко',	N'Ольга',		N'Владимировна',	'131EF84B-F06E-494B-848F-BB4BC0604266',		'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null	),
	( '3E229EB8-E99A-455F-8AF3-5871337A092C', N'Белоусова',	N'Валерия',		N'Петровна',		'131EF84B-F06E-494B-848F-BB4BC0604266',		null,										null	),
	( '5319FD22-9BDE-48E5-819D-FE884B70AFD8', N'Бердник',	N'Ирина',		N'Ивановна',		'D2469412-0E4B-46F7-80EC-8C522364D099',		null,										'39D57DFB-8DA7-49C9-AE8D-464509618F02'		),
	( '8939ED0C-BBDB-435E-923E-68158D2153C6', N'Красинец',	N'Нелли',		N'Ярославовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'743C93F2-4717-4E81-A093-69903476E176'		),
	( '663C3142-1C9D-4957-800D-F6C6824B9C88', N'Баранова',	N'Флорентина',	N'Брониславовна',	'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',		null,										'0017AAAE-3E22-462D-9031-4276A9788D51'		),
	( '239450EB-A92F-4093-A74F-EAA38F8ADBE2', N'Толочко',	N'Анжелика',	N'Борисовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										'23D52416-D994-4564-A106-1FDF5FECEF25'		),
	( '23DBE38C-0ED4-4E90-8BC7-F168134E8674', N'Родионова',	N'Эльвира',		N'Фёдоровна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'3E229EB8-E99A-455F-8AF3-5871337A092C'		),
	( '7A88B1B9-0216-4259-8BA6-C123ABB3C6A8', N'Трясило',	N'Инга',		N'Артёмовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null	),
	( '789A53AB-A54D-4AF7-94A5-DD288428A37C', N'Гуляева',	N'Клара',		N'Даниловна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		null,										'DC268B00-1727-4381-9878-6DA1BFEF2701'		),
	( 'A93A1B20-155A-43BD-ACEE-87A6088C969E', N'Исаева',	N'Марта',		N'Борисовна',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										null	),
	( 'E56F5DE6-A1D3-4C3E-A09A-A9B9FA96C9B3', N'Одинцова',	N'Зинаида',		N'Евгеньевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'D2469412-0E4B-46F7-80EC-8C522364D099',		'DD860E7E-C2F0-47A6-BA29-165BE015E5A2'		),
	( '207CDCF2-89AD-49A5-A669-A082FA9CCCBA', N'Соловьёва',	N'Флорентина',	N'Виталиевна',		'1EF7268C-43A8-488C-B761-90982B31DF4E',		null,										null	),
	( 'C5EE780A-4D53-40FB-A592-C35CFC9455F2', N'Мирна',		N'Рада',		N'Сергеевна',		'8DCC3969-1D93-47A9-8B79-A30C738DB9B4',		null,										null	),
	( 'D3FCC76B-09A2-4578-A72C-34468DA36C45', N'Одинцова',	N'Мальвина',	N'Дмитриевна',		'624B3BB5-0F2C-42B6-A416-099AAB799546',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		'1A930DE7-647B-4A32-AD3B-0CAF4528B356'		),
	( '6FB5BCA3-2CAE-4450-AAB5-E0184FD45BE9', N'Ткаченко',	N'Альбина',		N'Викторовна',		'415B36D9-2D82-4A92-A313-48312F8E18C6',		null,										null	)";
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Managers filled");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Filling error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            cmd.Dispose();
        }
        #region Запити із табличними результатами
        private void ShowDepartmentsView()
        {
            using MySqlCommand cmd = new("SELECT * FROM Departments", _connection);
            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                String str = String.Empty;
                // Передача даних відбувається по одному рядку
                while (reader.Read())  // зчитує рядок, якщо немає - false
                {
                    // рядок зчитується у сам reader, дані з нього можна дістати
                    // а) через гет-тери
                    // б) через індексатори
                    str += reader.GetGuid(0)    // типізований Get-тер: рекомендовано
                        + "  "                  // 
                        + reader[1]             // індексатор - object
                        + "\n";                 // відлік від 0 по порядку полів у результаті
                                                // TODO: реалізувати скорочене відображення id типу a8f2...2c
                }
                ViewDepartments.Text = str;
                reader.Close();   // !! Незакритий reader блокує інші команди до БД
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region HW view
        private void ShowProductsView()
        {
            using MySqlCommand cmd = new("SELECT * FROM Products", _connection);
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                StringBuilder result = new StringBuilder();
                while (reader.Read())
                {
                    var guid = reader.GetGuid(0).ToString();
                    var start = guid.Substring(0, 4);
                    var end = guid.Substring(guid.Length - 4, 4);
                    var name = reader.GetString(1);
                    var price = reader.GetDouble(2);
                    result.AppendLine(start + "..." + end + " " + name + " " + price);
                }
                ViewProducts.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ShowManagersView()
        {
            using MySqlCommand cmd = new("SELECT * FROM Managers JOIN Departments ON Id_main_dep = Departments.Id", _connection);
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();
                StringBuilder result = new StringBuilder();
                while (reader.Read())
                {
                    var guid = reader.GetGuid(0).ToString();
                    var start = guid.Substring(0, 4);
                    var end = guid.Substring(guid.Length - 4, 4);
                    var surname = reader.GetString(1);
                    var name = reader.GetString(2);
                    var lastname = reader.GetString(3);
                    var department = reader.GetString(8);

                    result.AppendLine(start + "..." + end + " " + surname + " " + name[0] + ". " + lastname[0] + ". " + department);
                }
                ViewManagers.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }


}
