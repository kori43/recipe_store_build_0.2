using recipe_store.Classes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    public partial class AdminWin : Window
    {
        Database database = new Database();
        private ObservableCollection<Recipes> recipes = new ObservableCollection<Recipes>();
        private ObservableCollection<Users> users = new ObservableCollection<Users>();
        public AdminWin()
        {
            InitializeComponent();

            DGRecipes.ItemsSource = recipes;
            DGUsers.ItemsSource = users;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecipes();
            LoadUsers();
        }

        private void LoadRecipes()
        {
            try
            {
                recipes.Clear();
                database.openConnection();

                string query = "SELECT * FROM Recipes";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Recipes recipe = new Recipes
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Author = reader["Author"].ToString(),
                        RecipeName = reader["RecipeName"].ToString()
                    };
                    recipes.Add(recipe);
                }
                database.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке рецептов: " + ex.Message);
            }
        }
        private void LoadUsers()
        {
            try
            {
                users.Clear();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке рецептов: " + ex.Message);
            }
        }

        private bool DeleteUserProfile(int Id)
        {
            try
            {
                var selectedUser = users.FirstOrDefault(item => item.Id == Id);
                if (selectedUser != null)
                {
                    users.Remove(selectedUser);

                    database.openConnection();

                    string query = "DELETE FROM Users WHERE id = @id";

                    SqlCommand command = new SqlCommand(query, database.getConnection());

                    command.Parameters.AddWithValue("@id", Id);
                    command.ExecuteNonQuery();

                    database.closeConnection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении аккаунта: " + ex.Message);
                return false;
            }
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            EditProfile editProfile = new EditProfile();
            editProfile.Show();
            this.Close();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Users selectedUser = DGUsers.SelectedItem as Users;
                if (selectedUser == null)
                {
                    MessageBox.Show("Выберите профиль для удаления.", "Внимание");
                    return;
                }
                int id = selectedUser.Id;
                bool success = DeleteUserProfile(id);
                if (success)
                {
                    LoadUsers();
                    MessageBox.Show("Аккаунт успешно удален!");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении аккаунта");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении аккаунта: " + ex.Message);
            }
        }
    }
}
