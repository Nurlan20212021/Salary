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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private SqlConnection SqlConnection = null;

        private SqlCommandBuilder SqlCommandBuilder = null;

        private SqlDataAdapter SqlDataAdapter = null;

        int ID_PositionP;

        gr682_gnmEntities db = new gr682_gnmEntities();
        public Auth()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
        }

        private void AuthUserClick(object sender, RoutedEventArgs e)
        {
            string user_name = LoginBox.Text.Replace("'", "");
            user_name = user_name.Replace("\"", "");
            string password = PasswordBox.Password.Replace("'", "");
            password = password.Replace("\"", "");

            if (LoginBox.Text == "" || PasswordBox.Password == "")
            {
                MessageBox.Show("Пустые поля");
            }
            if (db.Reg.Select(item => item.Login + " " + item.Password).Contains(user_name + " " + password))
            {
                MessageBox.Show("Вы авторизованы");
            }
            else
            {
                MessageBox.Show("Ошибка логина или пароля");
            }
            // Определение роли авторизованного человека. пр
            SqlConnection.Open();
            string vivodID = $"Select IDRole From Personbd inner join Reg on Personbd.IDReg = Reg.ID Where Login = '{user_name}' and Password = '{password}'";
            SqlCommand command = new SqlCommand(vivodID, SqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object id = reader.GetValue(0);
                    ID_PositionP = Convert.ToInt32(id.ToString());
                }
            }
            reader.Close();
            SqlConnection.Close();

            if (ID_PositionP == 1)
            {
                Sotrudniki sotrudniki = new Sotrudniki();
                sotrudniki.Show();
                this.Close();
            }
            if (ID_PositionP == 2)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}