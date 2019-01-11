using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace AstroBot.DB.Tasks
{
    class Tasks
    {
        private SqlConnection connection;

        public enum IdType
        {
            TGId,
            VKId
        }

        private enum UpdateOpt
        {
            CurrentTask,
            CurrentTaskAnswer,
            CurrentTaskCompleted
        }

        static public int MaxTasks{ get; private set; } = Convert.ToInt32(System.IO.File.ReadAllLines("../../../DB/Tasks/config.cfg")[0].Substring(6));

        public Tasks(ref SqlConnection connection)
        {
            this.connection = connection;
        }

        public Tuple<string, double> GetTask(IdType type, string id)
        {
            var n = getCurTaskNum(type, id);
            var compl = getCompleted(type, id);
            if (n == Tasks.MaxTasks && compl)
                throw new ArgumentException("Вы решили все задачи, приходите позже XD");
            else if(!compl)
                throw new ArgumentException("Вы ещё не решили предыдущую задачу");

            var tuple  = new Tuple<string, double>("Undefined", 0);
            var engine = Python.CreateEngine();
            var scope  = engine.CreateScope();
            var dic    = new Dictionary<string, object>
                {
                    { "task", tuple.Item1 },
                    { "answer", tuple.Item2 }
                };

            scope.SetVariable("params", dic);

            var source = engine.CreateScriptSourceFromFile($"../../../res/Scripts/Task{n}.py");
            source.Execute(scope);

            tuple = new Tuple<string, double>(scope.GetVariable<string>("task"), scope.GetVariable<double>("answer"));

            this.update<int>(type, id, UpdateOpt.CurrentTask, n + 1);
            this.update<double>(type, id, UpdateOpt.CurrentTaskAnswer, tuple.Item2);
            this.update<int>(type, id, UpdateOpt.CurrentTaskCompleted, 0);

            return tuple;
        }
        public bool CheckAnswer(IdType type, string id, double answer)
        {
            var ans = this.getAnswer(type, id);
            if (ans == 0)
                throw new ArgumentException("Вы еще не получали задание, используйте /task");

            if (ans == answer)
            {
                this.update<byte>(type, id, UpdateOpt.CurrentTaskCompleted, 1);

                return true;
            }

            return false;
        }
        private int getCurTaskNum(IdType type, string id)
        {
            string sql = "SELECT * FROM Students WHERE " + type.ToString() + " = @id";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = id;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    return reader.GetInt32(7);
                }
                else
                    return 0;
            }
        }

        private double getAnswer(IdType type, string id)
        {
            string sql = "SELECT * FROM Students WHERE " + type.ToString() + " = @id";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = id;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    return reader.GetDouble(8);
                }
                else
                    return 0;
            }
        }

        private bool getCompleted(IdType type, string id)
        {
            string sql = "SELECT * FROM Students WHERE " + type.ToString() + " = @id";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = id;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    return reader.GetInt32(9) != 0;
                }
                else
                    return false;
            }
        }

        private void update<T>(IdType idType, string id, UpdateOpt opt, T value)
        {
            string sql = "UPDATE Students SET " + opt.ToString() + " = @val WHERE " + idType.ToString() + " = @id";

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            System.Data.SqlDbType sqlDbType = System.Data.SqlDbType.Int;
            if (opt == UpdateOpt.CurrentTaskAnswer)
                sqlDbType = System.Data.SqlDbType.Float;

            cmd.Parameters.Add("@val", sqlDbType).Value = value;
            cmd.Parameters.Add("@id", sqlDbType).Value = id;

            cmd.ExecuteNonQuery();
        }
    }
}
