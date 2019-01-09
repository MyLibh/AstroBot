using System.Data.SqlClient;

namespace AstroBot.DB.Tasks
{
    class Tasks
    {
        private SqlConnection connection;
        public Tasks(ref SqlConnection connection)
        {
            this.connection = connection;
        }
    }
}
