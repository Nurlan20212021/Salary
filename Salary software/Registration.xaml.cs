using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        gr682_gnmEntities db = new gr682_gnmEntities();
        private SqlConnection SqlConnection = null;

        private SqlCommandBuilder SqlCommandBuilder = null;

        private SqlDataAdapter SqlDataAdapter = null;

        private int ID_RoleP;

        public Registration()
        {
            InitializeComponent();
            Load();
        }

        private void AuthUserClick(object sender, RoutedEventArgs e)
        {
            Auth rw = new Auth();
            rw.Show();
            this.Close();
        }

        private void RegUserClick(object sender, RoutedEventArgs e)
        {
            SqlConnection.Open();
            string vivodID = @"Select ID_Role from Role Where Name ='" + comboRole.Text + "'";
            SqlCommand command = new SqlCommand(vivodID, SqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object id = reader.GetValue(0);
                    ID_RoleP = Convert.ToInt32(id.ToString());
                }
            }
            reader.Close();

            if (login.Text == "" || password.Password == "" || LastName.Text == "" || FirstName.Text == "" || MiddleName.Text == "")
            {
                MessageBox.Show("Пустые поля");
            }
            if (db.Reg.Select(item => item.Login).Contains(login.Text))
            {
                MessageBox.Show("Такой логин уже существует в системе");
            }
            Reg newUser = new Reg()
            {
                Login = login.Text,
                Password = password.Password,
            };
            Personbd BD = new Personbd()
            {
                FirstName_person = FirstName.Text,
                LastName_person = LastName.Text,
                MiddleName_person = MiddleName.Text, 
                IDRole = ID_RoleP
            };
            db.Reg.Add(newUser);
            db.Personbd.Add(BD);
            db.SaveChanges();
            MessageBox.Show("Вы успешно зарегестрировались");
            Auth aw = new Auth();
            aw.Show();
            this.Close();
        }

        public void Load()
        {
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
            SqlDataAdapter = new SqlDataAdapter(@"Select Name From Role", SqlConnection);
            DataTable ComboD = new DataTable("Data");
            SqlDataAdapter.Fill(ComboD);
            comboRole.ItemsSource = ComboD.DefaultView;
        }
    } 
}