using recipe_store.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    public partial class SignUpWin : Window
    {
        Database database = new Database();
        public SignUpWin()
        {
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            if (checkUser())
            {
                return;
            }
            string userName = TextBox_UserName.Text;
            string userLogin = TextBox_Login.Text;
            string userPassword = TextBox_Password.Text;
            string confirmPassword = TextBox_Confirm_Password.Text;
            if (userPassword == confirmPassword)
            {
                string query = $"INSERT INTO Users(UserName, UserLogin, UserPassword) VALUES ('{userName}', '{userLogin}', '{userPassword}')";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                database.openConnection();

                if (userLogin == "" && userPassword == "")
                {
                    MessageBox.Show("Не удалось создать аккаунт!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Аккаунт создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось создать аккаунт!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                database.closeConnection();
            }
            else
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка");
            }
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            loginWin loginWin = new loginWin();
            loginWin.Show();
            this.Close();
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_UserName.Clear();
            TextBox_Login.Clear();
            TextBox_Password.Clear();
            TextBox_Confirm_Password.Clear();
        }

        private bool checkUser()
        {
            string userLogin = TextBox_Login.Text;
            string userPassword = TextBox_Password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string query = $"SELECT * from USers WHERE UserLogin = '{userLogin}' AND UserPassword = '{userPassword}'";

            SqlCommand command = new SqlCommand(query, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой аккаунт уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
