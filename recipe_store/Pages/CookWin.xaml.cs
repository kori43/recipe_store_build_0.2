using recipe_store.AddPages;
using recipe_store.Classes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    public partial class CookWin : Window
    {
        Database database = new Database();
        private ObservableCollection<Recipes> recipes = new ObservableCollection<Recipes>();
        public CookWin()
        {
            InitializeComponent();
            DGRecipes.ItemsSource = recipes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecipes();
        }
        public void SetUserInfo(int id, string name)
        {
            TextBox_UserId.Text = Convert.ToString(id);
            TextBox_UserName.Text = name;
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

        private bool CanToDeleteRecipe(Recipes selectedRecipe)
        {
            try
            {
                if (selectedRecipe != null)
                {
                    database.openConnection();

                    string query = "SELECT Author FROM Recipes WHERE id = @id";

                    SqlCommand command = new SqlCommand(query, database.getConnection());

                    command.Parameters.AddWithValue("@id", selectedRecipe.Id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string author = reader.GetString(0);

                        database.closeConnection();

                        if (author != TextBox_UserName.Text)
                        {
                            MessageBox.Show("Вы не можете удалить чужую запись!");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка чтения!");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Выберите запись для удаления!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                return false;
            }
        }

        private bool DeleteRecipe(int Id)
        {
            try
            {
                var selectedRecipe = recipes.FirstOrDefault(item => item.Id == Id);
                if (CanToDeleteRecipe(selectedRecipe))
                {
                    if (selectedRecipe != null)
                    {
                        recipes.Remove(selectedRecipe);

                        database.openConnection();

                        string query = "DELETE FROM Recipes WHERE id = @id";

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
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                return false;
            }
        }

        private void AddPage_Closed(object sender, System.EventArgs e)
        {
            LoadRecipes();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            string author = TextBox_UserName.Text;
            AddPage addpage = new AddPage(author);
            addpage.Closed += AddPage_Closed;
            addpage.Show();

            LoadRecipes();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recipes selectedRecipe = DGRecipes.SelectedItem as Recipes;
                if (selectedRecipe == null)
                {
                    MessageBox.Show("Выберите рецепт для удаления.", "Внимание");
                    return;
                }
                int id = selectedRecipe.Id;
                bool success = DeleteRecipe(id);
                if (success)
                {
                    LoadRecipes();
                    MessageBox.Show("Рецепт успешно удален!");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении записи");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
            }
        }
    }
}
