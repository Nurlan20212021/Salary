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
using System.Data.SqlClient;
using System.Data;

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для WindowPosition.xaml
    /// </summary>
    public partial class WindowPosition : Window
    {
        private SqlConnection SqlConnection = null;

        private SqlCommandBuilder SqlCommandBuilder = null;

        private SqlDataAdapter SqlDataAdapter = null;

        public WindowPosition()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
            Load();
        }

        private void Load()
        {
            SqlDataAdapter = new SqlDataAdapter(@"Select ID_Position AS 'ID', Position AS 'Должность' From Position", SqlConnection);
            DataTable vivod = new DataTable("Position");
            SqlDataAdapter.Fill(vivod);
            comboPos.ItemsSource = vivod.DefaultView;
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            if (Position.Text == "")
            {
                MessageBox.Show("Пустые поля");
            }
            else
            {
                SqlConnection.Open();
                string insert = @"insert into Position (Position)
                                values ('" + Position.Text + "');";
                SqlCommand cmd = new SqlCommand(insert, SqlConnection);
                cmd.ExecuteNonQuery();
                this.Close();
            }
        }
    }
}
