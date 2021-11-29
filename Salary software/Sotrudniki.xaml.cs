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
using SD = System.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для Sotrudniki.xaml
    /// </summary>
    public partial class Sotrudniki : Window
    {
        private SqlConnection SqlConnection = null;

        private SqlDataAdapter SqlDataAdapter = null;

        public int ID_POLP;

        public int ID_AddressP;

        public int ID_PositionP;

        public int DATAID;

        public Sotrudniki()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadedW();
        }

        private void LoadedW()
        {
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
            SqlDataAdapter = new SqlDataAdapter(@"Select POL From POL", SqlConnection);
            SD.DataTable dt = new SD.DataTable("Data");
            SqlDataAdapter.Fill(dt);
            Combo.ItemsSource = dt.DefaultView;

            SqlDataAdapter = new SqlDataAdapter(@"Select Position From Position", SqlConnection);
            SD.DataTable ComboD = new SD.DataTable("Data");
            SqlDataAdapter.Fill(ComboD);
            ComboDol.ItemsSource = ComboD.DefaultView;

            SqlDataAdapter = new SqlDataAdapter(@"Select CONCAT((LastName_person),' ', FirstName_person, ' ', MiddleName_person) as 'ФИО', POL as 'Пол', Position as 'Должность', CountChildren as 'Количество детей', PassportSeries as 'Серия паспорта', NumberPassport as 'Номер паспорта', Birthday as 'Дата рождения', NumberTelephone as 'Номер телефона', CONCAT((Street), ', ', Home) as 'Адрес'
            From Data inner join Pol on Pol.ID_POL = Data.POLID
            inner join AddressBD on Data.AddressID = AddressBD.ID_Address
            inner join Position on Data.PositionID = Position.ID_Position", SqlConnection);
            SD.DataTable vivod = new SD.DataTable("Data");
            SqlDataAdapter.Fill(vivod);
            dtSot.ItemsSource = vivod.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Address address = new Address();
            address.Show();
            this.Show();
        }

        public void Clear()
        {
            LastName.Clear();
            FirstName.Clear();
            MiddleName.Clear();
            Series.Clear();
            Number.Clear();
            NumberTelephone.Clear();
            CountChildren.Clear();
        }

        public void Perevod()
        {
            string vivodID = @"Select ID_POL from POL Where POL ='" + Combo.Text + "'";
            SqlCommand command = new SqlCommand(vivodID, SqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object id = reader.GetValue(0);
                    ID_POLP = Convert.ToInt32(id.ToString());
                }
            }
            reader.Close();

            string vivodIDAddress = @"Select ID_Address from AddressBD
                Where ID_Address = (select max(ID_Address) from AddressBD)";
            SqlCommand commandAddress = new SqlCommand(vivodIDAddress, SqlConnection);
            SqlDataReader readerAddress = commandAddress.ExecuteReader();

            if (readerAddress.HasRows) // если есть данные
            {
                while (readerAddress.Read()) // построчно считываем данные
                {
                    object id = readerAddress.GetValue(0);
                    ID_AddressP = Convert.ToInt32(id.ToString());
                }
            }
            readerAddress.Close();

            string vivodIDPosition = @"Select ID_Position from Position Where Position ='" + ComboDol.Text + "'";
            SqlCommand commandPosition = new SqlCommand(vivodIDPosition, SqlConnection);
            SqlDataReader readerPosition = commandPosition.ExecuteReader();

            if (readerPosition.HasRows) // если есть данные
            {
                while (readerPosition.Read()) // построчно считываем данные
                {
                    object id = readerPosition.GetValue(0);
                    ID_PositionP = Convert.ToInt32(id.ToString());
                }
            }
            readerPosition.Close();
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection.Open();
                Perevod();
                if (LastName.Text == "" || FirstName.Text == "" || MiddleName.Text == "" || Series.Text == "" || Number.Text == "")
                {
                    MessageBox.Show("Пустые поля");
                }

                string insert = @"insert into DATA (LastName_person, FirstName_person, MiddleName_person, PassportSeries, NumberPassport, CountChildren, Birthday, NumberTelephone, POLID, PositionID, AddressID)
                                values ('" + LastName.Text + "', '" + FirstName.Text + "', '" + MiddleName.Text + "', '" + Series.Text + "', '" + Number.Text + "', " + CountChildren.Text + ", '" + Birthday.SelectedDate + "', '" + NumberTelephone.Text + "', " + ID_POLP + "," + ID_PositionP + "," + ID_AddressP + ")";
                SqlCommand cmd = new SqlCommand(insert, SqlConnection);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                MessageBox.Show("Ввод некоторых значений не действителен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (SqlConnection != null)
                {
                    SqlConnection.Close();
                }
            }
            LoadedW();
            Clear();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection.Open();
                SD.DataRowView red = (SD.DataRowView)dtSot.SelectedItems[0];

                string vivodID = $"Select DATAID from Payments inner join Data on Data.ID_Data = DATAID where PassportSeries = " + red["Серия паспорта"] + "and NumberPassport = " + red["Номер паспорта"];
                SqlCommand command = new SqlCommand(vivodID, SqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        DATAID = Convert.ToInt32(id.ToString());
                    }
                }
                reader.Close();

                string upit = $"Delete from Payments where DATAID = {DATAID} Delete from Data where PassportSeries = " + red["Серия паспорта"] + "and NumberPassport = " + red["Номер паспорта"];
                MessageBoxResult rezultat = MessageBox.Show("Вы хотите удалить этого сотрудника ?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand(upit, SqlConnection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
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
            LoadedW();
            Clear();
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection.Open();         
                SD.DataRowView red = (SD.DataRowView)dtSot.SelectedItems[0];
                Perevod();
                int perevodChild = Convert.ToInt32(CountChildren.Text);
                string update = $"Update DATA set LastName_person = '{LastName.Text}', FirstName_person = '{FirstName.Text}', MiddleName_person = '{MiddleName.Text}', PassportSeries = '{Series.Text}', NumberPassport = '{Number.Text}', CountChildren = {perevodChild}, Birthday = '{Birthday.SelectedDate}', NumberTelephone = '{NumberTelephone.Text}', POLID = {ID_POLP}, PositionID = {ID_PositionP}, AddressID = {ID_AddressP} Where PassportSeries = '{red["Серия паспорта"]}' and NumberPassport = '{red["Номер паспорта"]}'";
                MessageBoxResult rezultat = MessageBox.Show("Вы хотите изменить данные этого сотрудника ?", "Изменение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand(update, SqlConnection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
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
            LoadedW();
            Clear();
        }

        private void Dol(object sender, RoutedEventArgs e)
        {
            WindowPosition windowPosition = new WindowPosition();
            windowPosition.Show();
        }
    }
}