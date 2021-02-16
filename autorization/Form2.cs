using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace autorization
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //строка подключения из файла app.config
            string con = ConfigurationManager.ConnectionStrings["DbDatabase"].ConnectionString;
            //подключение к mysql
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = con
            };
            MySqlCommand command = new MySqlCommand
            {
                Connection = connection
            };
            command.CommandText = "SELECT * FROM tovar";
            //подключаемся и выполняем запрос
            //на случай, если сервер не работает, сделаем обработку исключения
            try
            {
                connection.Open();
                DataTable table = new DataTable();
                table.Load(command.ExecuteReader());
                dataGridView1.DataSource = table;
                //закрываем соединение
                connection.Close();
            }
            //exception ex лучше поменять на более точный exception
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var report = StiReport.CreateNewReport();

            

            report.Render();
            report.Show();
        }
    }
}
