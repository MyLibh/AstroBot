using System.Data.SqlClient;

namespace AstroBot.DB
{
    static class DataBase
    {
        public static Students.Students Students { set; get; }
        public static Tasks.Tasks Tasks { set; get; }

        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Aleksei\Documents\Github\AstroBot\AstroBot\DB\DB.mdf;Integrated Security=True";
        private static SqlConnection connection;
        private static bool isOpen = false;

        public static void OpenConnection()
        {
            if (!isOpen)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                Students = new Students.Students(ref connection);
                Tasks = new Tasks.Tasks(ref connection);

                isOpen = true;
            }
        }

        public static void CloseConnection()
        {
            if (isOpen)
            {
                connection.Close();

                isOpen = false;
            }
        }
    }
}
