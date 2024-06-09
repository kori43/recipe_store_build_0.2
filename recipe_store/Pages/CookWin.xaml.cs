using recipe_store.AddPages;
using recipe_store.Classes;
using recipe_store.EditPages;
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

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            AddPage addpage = new AddPage();
            addpage.Show();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            EditRecipe editRecipe = new EditRecipe();
            editRecipe.Show();
        }
    }
}
