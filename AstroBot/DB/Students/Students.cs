using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace AstroBot.DB.Students
{
    class Students
    {
        private SqlConnection connection;

        public enum IdType
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

        public Student GetByID(IdType opt, string value)
        {
            string sql = "SELECT * FROM Students WHERE " + opt.ToString() + " = @value";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@value", System.Data.SqlDbType.NVarChar).Value = value;

            Student student = new Student();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    student.Name    = Convert.ToString(reader.GetValue(2));
                    student.Surname = Convert.ToString(reader.GetValue(3));
                    student.Class   = Convert.ToString(reader.GetValue(4));
                    student.CurrentTask = Convert.ToInt32(reader.GetValue(7));
                }
            }

            return student;
        }

        public async void Add(Student student)
        {
            string sql = "INSERT INTO Students VALUES (@Role, @Name, @Surname, @Class, @TGId, @VKId, @CurrentTask, @CurrentAnswer, @CurrentTaskCompleted)";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = student.Name;
            cmd.Parameters.Add("@Role", System.Data.SqlDbType.NVarChar).Value = student.Role.ToString();
            cmd.Parameters.Add("@Surname", System.Data.SqlDbType.NVarChar).Value = student.Surname;
            cmd.Parameters.Add("@Class", System.Data.SqlDbType.NVarChar).Value = student.Class;
            cmd.Parameters.Add("@TGId", System.Data.SqlDbType.NVarChar).Value = student.TGId;
            cmd.Parameters.Add("@VKId", System.Data.SqlDbType.NVarChar).Value = student.VKId;
            cmd.Parameters.Add("@CurrentTask", System.Data.SqlDbType.Int).Value = student.CurrentTask;
            cmd.Parameters.Add("@CurrentAnswer", System.Data.SqlDbType.Float).Value = student.CurrentAnswer;
            cmd.Parameters.Add("@CurrentTaskCompleted", System.Data.SqlDbType.Int).Value = student.CurrentTaskCompleted;

            await cmd.ExecuteNonQueryAsync();
        }

        public async void Update(IdType opt, string surname, string newMessengerId)
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

        public bool Exist(ExistOption opt, string value)
        {
            string sql = "SELECT * FROM Students WHERE " + opt.ToString() + " = @value";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@value", System.Data.SqlDbType.NVarChar).Value = value;

            return Convert.ToInt32(cmd.ExecuteScalar()) != 0;
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
                    printLine();
                    Console.WriteLine($"| {{0, 2}} | {{1, {Student.RoleMaxLength}}} | {{2, {Student.NameMaxLength}}} | {{3, {Student.SurnameMaxLength}}} | {{4, 5}} | {{5, {Student.TGIdMaxLength}}} | {{6, {Student.VKIdMaxLength}}} |" +
                        $" {{7, {Student.CurrentTaskMaxLength}}} | {{8, {Student.CurrentAnswerMaxLength}}} | {{9, {Student.CurrentTaskCompletedMaxLength}}} |",
                        "Id",
                        "Role",
                        "Name",
                        "Surname",
                        "Class", 
                        "TGId",
                        "VKId",
                        "Task",
                        "Ans",
                        "Compl");
                    printLine();

                    while (reader.Read())
                        Console.WriteLine($"| {{0, 2}} | {{1, {Student.RoleMaxLength}}} | {{2, {Student.NameMaxLength}}} | {{3, {Student.SurnameMaxLength}}} | {{4, 5}} | {{5, {Student.TGIdMaxLength}}} | {{6, {Student.VKIdMaxLength}}} |" +
                            $" {{7, {Student.CurrentTaskMaxLength}}} | {{8, {Student.CurrentAnswerMaxLength}}} | {{9, {Student.CurrentTaskCompletedMaxLength}}} |",
                            reader.GetValue(0),
                            reader.GetValue(1),
                            reader.GetValue(2),
                            reader.GetValue(3),
                            reader.GetValue(4),
                            reader.GetValue(5),
                            reader.GetValue(6),
                            reader.GetValue(7),
                            reader.GetValue(8),
                            reader.GetValue(9));

                    printLine();
                }
                else
                {
                    Console.WriteLine("Students are empty");
                }
            }
        }

        private void printLine()
        {
            Console.WriteLine("+-{0}-+-{1}-+-{2}-+-{3}-+-{4}-+-{5}-+-{6}-+-{7}-+-{8}-+-{9}-+",
                        new string('-', 2),
                        new string('-', Student.RoleMaxLength),
                        new string('-', Student.NameMaxLength),
                        new string('-', Student.SurnameMaxLength),
                        new string('-', 5),
                        new string('-', Student.TGIdMaxLength),
                        new string('-', Student.VKIdMaxLength),
                        new string('-', Student.CurrentTaskMaxLength),
                        new string('-', Student.CurrentAnswerMaxLength),
                        new string('-', Student.CurrentTaskCompletedMaxLength));
        }
    }
}
