
using System.Data.SqlClient;

namespace recipe_store.Classes
{
    public class Database
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-2D1FGK3;Initial Catalog=recipe_store;Integrated Security=True");
        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
