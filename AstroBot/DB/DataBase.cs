using System.Data.SqlClient;

using AstroBot.Util;

namespace AstroBot.DB
{
    static class DataBase
    {
        public static Students.Students Students { private set; get; }
        public static Tasks.Tasks Tasks { private set; get; }

        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Aleksei\Documents\Github\AstroBot\AstroBot\DB\DB.mdf;Integrated Security=True";
        private static SqlConnection connection;
        public static bool IsOpen = false;

        public static void OpenConnection()
        {
            if (!IsOpen)
            {
                Logger.Log(Logger.Module.Core, Logger.Type.Info, "Openning DataBase connection...");

                connection = new SqlConnection(connectionString);
                connection.Open();

                Students = new Students.Students(ref connection);
                Tasks = new Tasks.Tasks(ref connection);

                IsOpen = true;

                Logger.Log(Logger.Module.Core, Logger.Type.Info, "DataBase connection opened");
            }
        }

        public static void CloseConnection()
        {
            if (IsOpen)
            {
                Logger.Log(Logger.Module.Core, Logger.Type.Info, "Closing DataBase connection...");

                connection.Close();

                IsOpen = false;

                Logger.Log(Logger.Module.Core, Logger.Type.Info, "DataBase connection closed");
            }
        }
    }
}
