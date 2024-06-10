using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using recipe_store.Classes;
using recipe_store.Pages;

namespace recipe_store.AddPages
{  
    public partial class AddPage : Window
    {
        Database database = new Database();
        private string author;
        public AddPage(string author)
        {
            InitializeComponent();
            this.author = author;
        }

        private void CreateRecipe()
        {
            var recipe = TextBox_RecipeName.Text;

            if (recipe != null) 
            {
                try
                {
                    database.openConnection();
                    var query = $"INSERT INTO Recipes (Author, RecipeName) VALUES ('{author}', '{recipe}')";
                    var command = new SqlCommand(query, database.getConnection());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Рецепт успешно создан!");
                    database.closeConnection();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось создать рецепт: " + ex.Message);
                }
            }
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            CreateRecipe();
            
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_RecipeName.Clear();
        }
    }
}
