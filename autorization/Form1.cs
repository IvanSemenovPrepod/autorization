using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace autorization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            //првоеряем что логин и пароль введены
            if (string.IsNullOrEmpty(login)|| string.IsNullOrEmpty(password))
            {
                MessageBox.Show("логин или пароль не введен");
            }
            else
            {
                //строка подключения из файла app.config
                string con = ConfigurationManager.ConnectionStrings["DbDatabase"].ConnectionString;
                //подключение к mysql
                MySqlConnection connection = new MySqlConnection
                {
                    ConnectionString = con
                };
                //запрос sql для получения списка пользователей
                //с введеными логином и паролем
                //в нормальном случае должен быть только один
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", GetHash(password));
                command.CommandText = "SELECT COUNT(*) FROM user WHERE login=@login AND password=@password";
                //подключаемся и выполняем запрос
                //на случай, если сервер не работает, сделаем обработку исключения
                try
                {
                    connection.Open();
                    //в переменную countuser присвоится количество пользователей из базы данных
                    int countUser = Convert.ToInt32(command.ExecuteScalar());
                    //закрываем соединение
                    connection.Close();
                    //проверяем countUser
                    if (countUser == 1)
                    {
                        //MessageBox.Show("Пользователь авторизован");
                        Form2 frm = new Form2();
                        this.Hide();
                        if (frm.ShowDialog() != DialogResult.OK)
                            this.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не зарегистрирован");
                    }
                }
                //exception ex лучше поменять на более точный exception
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
