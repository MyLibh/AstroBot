using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace AstroBot.DB.Students
{
    class Students
    {
        private SqlConnection connection;

        public enum UpdateOption
        {
            TGId,
            VKId
        }

        public enum ExistOption
        {
            Surname,
            TGId,
            VKId
        }

        public Students(ref SqlConnection connection)
        {
            this.connection = connection;
        }

        public async void Add(Student student)
        {
            string sql = "INSERT INTO Students VALUES (@Name, @Surname, @Class, @TGId, @VKId)";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = student.Name;
            cmd.Parameters.Add("@Surname", System.Data.SqlDbType.NVarChar).Value = student.Surname;
            cmd.Parameters.Add("@Class", System.Data.SqlDbType.NVarChar).Value = student.Class;
            cmd.Parameters.Add("@TGId", System.Data.SqlDbType.NVarChar).Value = student.TGId;
            cmd.Parameters.Add("@VKId", System.Data.SqlDbType.NVarChar).Value = student.VKId;

            await cmd.ExecuteNonQueryAsync();
        }

        public async void Update(UpdateOption opt, string surname, string newMessengerId)
        {
            string sql = "UPDATE Students SET " + opt.ToString() + " = @messengerId WHERE Surname = @Surname";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@Surname", System.Data.SqlDbType.NVarChar).Value = surname;
            cmd.Parameters.Add("@messengerId", System.Data.SqlDbType.NVarChar).Value = newMessengerId;
            
           await cmd.ExecuteNonQueryAsync();
        }

        public async void Remove(int id)
        {
            string sql = "DELETE FROM Students WHERE Id = @Id";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

            await cmd.ExecuteNonQueryAsync();
        }

        public int Exist(ExistOption opt, string value)
        {
            string sql = "SELECT COUNT(*) FROM Students WHERE " + opt.ToString() + " = @value";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@value", System.Data.SqlDbType.NVarChar).Value = value;

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async void Execute(string sql)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            await cmd.ExecuteNonQueryAsync();
        }

        public void Print()
        {
            string sql = "SELECT * FROM Students";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("+----+-----------------+----------------------+-------+-----------------+-----------------+");
                    Console.WriteLine($"| {{0, 2}} | {{1, {Student.NameMaxLength}}} | {{2, {Student.SurnameMaxLength}}} | {{3, 5}} | {{4, {Student.TGIdMaxLength}}} | {{5, {Student.VKIdMaxLength}}} |",
                        "Id",
                        "Name",
                        "Surname",
                        "Class", "TGId",
                        "VKId");
                    Console.WriteLine("+----+-----------------+----------------------+-------+-----------------+-----------------+");

                    while (reader.Read())
                        Console.WriteLine($"| {{0, 2}} | {{1, {Student.NameMaxLength}}} | {{2, {Student.SurnameMaxLength}}} | {{3, 5}} | {{4, {Student.TGIdMaxLength}}} | {{5, {Student.VKIdMaxLength}}} |",
                            reader.GetValue(0),
                            reader.GetValue(1),
                            reader.GetValue(2),
                            reader.GetValue(3),
                            reader.GetValue(4),
                            reader.GetValue(5));

                    Console.WriteLine("+----+-----------------+----------------------+-------+-----------------+-----------------+");
                }
                else
                {
                    Console.WriteLine("Students are empty");
                }
            }
        }
    }
}
