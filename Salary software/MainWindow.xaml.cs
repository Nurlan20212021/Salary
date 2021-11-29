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

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection SqlConnection = null;

        private SqlDataAdapter SqlDataAdapter = null;

        gr682_gnmEntities db = new gr682_gnmEntities();

        int IDDATA;

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Address address = new Address();
            address.Show();
            this.Show();
        }

        private void Button_Dol(object sender, RoutedEventArgs e)
        {
            WindowPosition windowPosition = new WindowPosition();
            windowPosition.Show();
            this.Show();
        }

        private void Load()
        {
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
            SqlDataAdapter = new SqlDataAdapter($"Select LastName_person as 'Фамилия', FirstName_person as 'Имя', MiddleName_person as 'Отчество', Position as 'Должность' From Data inner join Position on Position.ID_Position = Data.PositionID", SqlConnection);
            DataTable vivod = new DataTable("Data");
            SqlDataAdapter.Fill(vivod);
            dtPay.ItemsSource = vivod.DefaultView;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            PayTable payTable = new PayTable();
            payTable.Show();
        }

        private void Clear()
        {
            DayRate.Clear();
            CountDay.Clear();
        }

        private void Raschet(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DayRate.Text == "" || CountDay.Text == "" || Date.Text == "")
                {
                    MessageBox.Show("Пустые поля");
                }

                SqlConnection.Open();
                DataRowView red = (DataRowView)dtPay.SelectedItems[0];
                string vivodID = $"Select ID_DATA from DATA Where LastName_person = '{red["Фамилия"]}' and FirstName_person = '{red["Имя"]}' and MiddleName_person = '{red["Отчество"]}'";
                SqlCommand command = new SqlCommand(vivodID, SqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        IDDATA = Convert.ToInt32(id.ToString());
                    }
                }
                reader.Close();

                string upit = $"EXEC AddPayments {DayRate.Text}, {CountDay.Text}, '{Date.SelectedDate}', {IDDATA}";
                SqlCommand cmd = new SqlCommand(upit, SqlConnection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Расчет выполнен");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Выберите сотрудника!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
            finally
            {
                if (SqlConnection != null)
                {
                    SqlConnection.Close();
                }
            }
            Clear();
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            WinCalc winCalc = new WinCalc();
            winCalc.Show();
        }
    }
}