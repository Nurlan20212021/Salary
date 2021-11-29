using System;
using System.Collections.Generic;
using SD = System.Data;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Salary_software
{
    /// <summary>
    /// Логика взаимодействия для PayTable.xaml
    /// </summary>
    public partial class PayTable : Window
    {
        private SqlConnection SqlConnection = null;

        private SqlDataAdapter SqlDataAdapter = null;

        int DATAID;

        public PayTable()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            SqlConnection = new SqlConnection("Data Source=mssql;Initial Catalog=gr682_gnm;Integrated Security=True");
            SqlDataAdapter = new SqlDataAdapter(@"Select LastName_person as 'Фамилия', FirstName_person as 'Имя', MiddleName_person as 'Отчество', CountChildren as 'Количество детей', NDFL as 'НДФЛ', PFR as 'ПФР', FSS as 'ФСС', Premiya as 'Премия', NaRuki as 'Сотруднику', DayRate as 'Ставка', CountDay as 'Количесто дней', Nachisleno as 'Начислено', ZPTaxes as 'ЗП + налоги', Date as 'Дата'
            From Data inner join Payments on Data.ID_Data = DATAID", SqlConnection);
            SD.DataTable vivod = new SD.DataTable("Data");
            SqlDataAdapter.Fill(vivod);
            dtPayTable.ItemsSource = vivod.DefaultView;
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection.Open();
                SD.DataRowView red = (SD.DataRowView)dtPayTable.SelectedItems[0];
                string vivodID = $"Select DATAID from Payments inner join Data on Data.ID_Data = DATAID Where LastName_person = '{red["Фамилия"]}' and FirstName_person = '{red["Имя"]}' and MiddleName_person = '{red["Отчество"]}'";
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

                string upit = $"Delete from Payments Where DATAID = {DATAID} and ID_Payments = (Select ID_Payments From Payments Where DATAID = {DATAID} and Date = '{red["Дата"]}')";
                MessageBoxResult rezultat = MessageBox.Show("Вы хотите удалить эту строку ?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            Load();
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            
            Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dtPayTable.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dtPayTable.Columns[j].Header;
            }
            for (int i = 0; i < dtPayTable.Columns.Count; i++)
            { 
                for (int j = 0; j < dtPayTable.Items.Count; j++)
                {
                    TextBlock b = dtPayTable.Columns[i].GetCellContent(dtPayTable.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
            excel.Visible = true;
        }

        private void Poisk_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter = new SqlDataAdapter($"Select LastName_person as 'Фамилия', FirstName_person as 'Имя', MiddleName_person as 'Отчество', CountChildren as 'Количество детей', NDFL as 'НДФЛ', PFR as 'ПФР', FSS as 'ФСС', Premiya as 'Премия', NaRuki as 'Сотруднику', DayRate as 'Ставка', CountDay as 'Количесто дней', Nachisleno as 'Начислено', ZPTaxes as 'ЗП + налоги', Date as 'Дата' From Data inner join Payments on Data.ID_Data = DATAID Where Date = '{Poisk.SelectedDate}'", SqlConnection);
            SD.DataTable vivod = new SD.DataTable("Data");
            SqlDataAdapter.Fill(vivod);
            dtPayTable.ItemsSource = vivod.DefaultView;

            if(PLast.Text != "")
            {
                SqlDataAdapter = new SqlDataAdapter($"Select LastName_person as 'Фамилия', FirstName_person as 'Имя', MiddleName_person as 'Отчество', CountChildren as 'Количество детей', NDFL as 'НДФЛ', PFR as 'ПФР', FSS as 'ФСС', Premiya as 'Премия', NaRuki as 'Сотруднику', DayRate as 'Ставка', CountDay as 'Количесто дней', Nachisleno as 'Начислено', ZPTaxes as 'ЗП + налоги', Date as 'Дата' From Data inner join Payments on Data.ID_Data = DATAID Where LastName_person = '{PLast.Text}'", SqlConnection);
                SD.DataTable vivodLast = new SD.DataTable("Data");
                SqlDataAdapter.Fill(vivodLast);
                dtPayTable.ItemsSource = vivodLast.DefaultView;
                PLast.Clear();
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void sort_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter = new SqlDataAdapter($"Select LastName_person as 'Фамилия', FirstName_person as 'Имя', MiddleName_person as 'Отчество', CountChildren as 'Количество детей', NDFL as 'НДФЛ', PFR as 'ПФР', FSS as 'ФСС', Premiya as 'Премия', NaRuki as 'Сотруднику', DayRate as 'Ставка', CountDay as 'Количесто дней', Nachisleno as 'Начислено', ZPTaxes as 'ЗП + налоги', Date as 'Дата' From Data inner join Payments on Data.ID_Data = DATAID Order by LastName_person", SqlConnection);
            SD.DataTable vivod = new SD.DataTable("Data");
            SqlDataAdapter.Fill(vivod);
            dtPayTable.ItemsSource = vivod.DefaultView;
        }
    }
}