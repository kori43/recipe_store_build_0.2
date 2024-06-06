using recipe_store.Classes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace recipe_store.Pages
{
    /// <summary>
    /// Логика взаимодействия для CookWin.xaml
    /// </summary>
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
    }
}
