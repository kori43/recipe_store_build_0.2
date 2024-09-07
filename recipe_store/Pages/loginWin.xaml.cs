using recipe_store.Classes;
using recipe_store.Pages;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store
{
    public partial class loginWin : Window
    {
        Database database = new Database();
        public loginWin()
        {
            InitializeComponent();
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Login.Clear();
            PasswordBox_Password.Clear();
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            SignUpWin signUpWin = new SignUpWin();
            signUpWin.Show();
            this.Close();
        }

        private void Btn_Entry_Click(object sender, RoutedEventArgs e)
        {
            string userLogin = TextBox_Login.Text;
            string userPassword = PasswordBox_Password.Password;

            string query = "SELECT id, UserName, UserRole, UserStatus FROM Users WHERE UserLogin = @Userlogin AND UserPassword = @UserPassword";

            database.openConnection();

            SqlCommand command = new SqlCommand(query, database.getConnection());

            command.Parameters.AddWithValue("@Userlogin", userLogin);
            command.Parameters.AddWithValue("@UserPassword", userPassword);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int userId = reader.GetInt32(0);
                string userName = reader.GetString(1);
                int roleId = reader.GetInt32(2);
                int status = reader.GetInt32(3);
                try
                {
                    if (status == 2)
                    {
                        MessageBox.Show("Пользователь заблокирован!", "Ошибка");
                    }
                    else
                    {
                        database.openConnection();

                        switch (roleId)
                        {
                            case 1:
                                AdminWin adminWin = new AdminWin();
                                adminWin.Show();
                                this.Close();
                                break;
                            case 2:
                                CookWin cookWin = new CookWin();
                                cookWin.SetUserInfo(userId, userName);
                                cookWin.Show();
                                this.Close();
                                break;
                            case 3:
                                SubscriberWin subscriberWin = new SubscriberWin();
                                subscriberWin.Show();
                                this.Close();
                                break;
                            default:
                                MessageBox.Show("Некорректная роль пользователя!");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка авторизации: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Ошибка");
                database.closeConnection();
            }
        }

    }
}
