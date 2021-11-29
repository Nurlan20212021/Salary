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
    /// Логика взаимодействия для Address.xaml
    /// </summary>
    public partial class Address : Window
    {
        private SqlConnection SqlConnection = null;

        public Address()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            if (Country.Text == "" || City.Text == "" || Street.Text == "" || Home.Text == "" || Flat.Text == "" || Index.Text == "")
            {
                MessageBox.Show("Пустые поля");
            }
            else
            {
                SqlConnection.Open();
                string insert = @"insert into AddressBD (Country, City, Street, Home, Flat, Indexx)
                                values ('" + Country.Text + "', '" + City.Text + "', '" + Street.Text + "', '" + Home.Text + "', '" + Flat.Text + "','" + Index.Text + "');";
                SqlCommand cmd = new SqlCommand(insert, SqlConnection);
                cmd.ExecuteNonQuery();
                this.Close();
            }
        }
    }
}
