using Microsoft.Data.Sqlite;
using System.IO;

namespace BotEcampus.UI.DataBase
{
    public class SQLiteDb : IDataBase
    {
        private string connectionString;
        private SqliteConnection connection;
        private object obj = new object();
        public SQLiteDb(string pathDb)
        {
            if (!File.Exists(pathDb))
            {
                File.Create(pathDb);
            }
            connection = new SqliteConnection($"Data source = {pathDb}");
        }
        public void AddUser()
        {


        }


        public string GetUserById(long? id)
        {
            lock (obj)
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                @"
                SELECT Login,Password
                FROM Users
                WHERE UserId = $id
                ";

                command.Parameters.AddWithValue("$id", id);


                using (var reader = command.ExecuteReader())
                {
                    var user = "";
                    while (reader.Read())
                    {
                        user = reader.GetValue(0) + ":" + reader.GetValue(1);
                        return user;
                    }
                }
            }
            return null;
        }

        public void RemoveById(long? id)
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Users
                WHERE UserId = @$id
            ";
            command.Parameters.AddWithValue("$id", id);
            var result = command.ExecuteReader();
            result.Close();
        }
    }
}
