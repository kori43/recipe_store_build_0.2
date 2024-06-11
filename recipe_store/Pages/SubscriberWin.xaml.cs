using recipe_store.Classes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    public partial class SubscriberWin : Window
    {
        Database database = new Database();
        private ObservableCollection<Recipes> recipes = new ObservableCollection<Recipes>();
        private ObservableCollection<Subscriber> subscribers = new ObservableCollection<Subscriber>();
        public SubscriberWin()
        {
            InitializeComponent();
            DGRecipes.ItemsSource = recipes;
            DGSubscriber.ItemsSource = subscribers;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecipes();
            LoadSubscriber();
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

        private void LoadSubscriber()
        {
            try
            {
                subscribers.Clear();
                database.openConnection();
                string query = "SELECT * FROM Subscriber";

                SqlCommand command = new SqlCommand(query, database.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Subscriber subscriber = new Subscriber
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Author = reader["Author"].ToString(),
                        RecipeName = reader["RecipeName"].ToString()
                    };
                    subscribers.Add(subscriber);
                }
                database.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке сохраненных рецептов: " + ex.Message);
            }
        }

        private bool DeleteFromSubscriber(int Id)
        {
            try
            {
                var selectedSub = subscribers.FirstOrDefault(item => item.Id == Id);
                if (selectedSub != null)
                {
                    subscribers.Remove(selectedSub);

                    database.openConnection();

                    string query = "DELETE FROM Subscriber WHERE id = @id";

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
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                return false;
            }
        }

        private bool SaveToSubscriber(int Id)
        {
            try
            {
                Recipes selectedRecipe = DGRecipes.SelectedItem as Recipes;
                if (selectedRecipe != null)
                {
                    database.openConnection();

                    string query = "INSERT INTO Subscriber (Author, RecipeName) " +
                                   "VALUES (@Author, @RecipeName)";

                    SqlCommand command = new SqlCommand(query, database.getConnection());

                    command.Parameters.AddWithValue("@Author", selectedRecipe.Author);
                    command.Parameters.AddWithValue("@RecipeName", selectedRecipe.RecipeName);
                    command.ExecuteNonQuery();

                    database.closeConnection();

                    LoadSubscriber();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении записи: " + ex.Message);
                return false;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recipes selectedRecipe = DGRecipes.SelectedItem as Recipes;
                if (selectedRecipe == null)
                {
                    MessageBox.Show("Выберите рецепт для сохранения.");
                    return;
                }
                int id = selectedRecipe.Id;
                bool success = SaveToSubscriber(id);
                if (success)
                {
                    LoadSubscriber();
                    MessageBox.Show("Запись успешно удалена!");
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

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Subscriber selectedSub = DGSubscriber.SelectedItem as Subscriber;
                if (selectedSub == null)
                {
                    MessageBox.Show("Выберите рецепт для удаления.", "Внимание");
                    return;
                }
                int id = selectedSub.Id;
                bool success = DeleteFromSubscriber(id);
                if (success)
                {
                    LoadSubscriber();
                    MessageBox.Show("Запись успешно удалена!");
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

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
