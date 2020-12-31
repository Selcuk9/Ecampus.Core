using BotEcampus.Core.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
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
        public bool AddUser(Authorization authData)
        {
            lock (this)
            {
                List<string> auth = new();
                if (auth != null)
                {
                    connection.Open();
                    string login = authData.Login;
                    string password = authData.Password;
                    var userId = authData.UserId;
                    var command = connection.CreateCommand();
                    command.Parameters.AddWithValue("$userId",userId);
                    command.Parameters.AddWithValue("$password",password);
                    command.Parameters.AddWithValue("$login",login);

                    command.CommandText =
                    @"
                    INSERT INTO Users(UserId,Login,Password)
                    VALUES($userId,$login,$password)
                    ";
                    var result = command.ExecuteReader();
                    return true;
                }
                return false;
            }
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
