using recipe_store.Classes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    public partial class EditProfile : Window
    {
        Database database = new Database();
        private ObservableCollection<Users> users = new ObservableCollection<Users>();
        private ObservableCollection<Status> statuses = new ObservableCollection<Status>();
        public EditProfile()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBox_UserLogin();
            LoadComboBox_UserStatus();
        }

        private void LoadComboBox_UserLogin()
        {
            try
            {
                database.openConnection();

                string query = "SELECT * FROM Users";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Users user = new Users
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        UserName = reader["UserName"].ToString(),
                        UserLogin = reader["UserLogin"].ToString(),
                        UserRole = Convert.ToInt32(reader["UserRole"]),
                        UserStatus = Convert.ToInt32(reader["UserStatus"])
                    };
                    users.Add(user);
                }
                database.closeConnection();

                ComboBox_UserName.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке пользователей: " + ex.Message);
            }
        }

        private void LoadComboBox_UserStatus()
        {
            try
            {
                database.openConnection();

                string query = "SELECT * FROM UserStatus";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Status userStatus = new Status
                    {
                        id = Convert.ToInt32(reader["id"]),
                        status = reader["Status"].ToString()
                    };
                    statuses.Add(userStatus);
                }
                database.closeConnection();

                ComboBox_UserStatus.ItemsSource = statuses;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке статуса: " + ex.Message);
            }
        }

        private int GetStatus(int status)
        {
            if (ComboBox_UserStatus.Text == "Заблокирован")
            {
                return status = 2;
            }
            if (ComboBox_UserStatus.Text == "В системе")
            {
                return status = 1;
            }
            return 0;
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            AdminWin adminWin = new AdminWin();
            adminWin.Show();
            this.Close();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            int status = 0;
            status = GetStatus(status);
            try
            {
                database.openConnection();

                string query = $"UPDATE Users set UserStatus = '{status}' WHERE UserLogin = '{ComboBox_UserName.Text}'";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                command.ExecuteNonQuery();

                database.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить статус: " + ex.Message);
            }
        }
    }
}
