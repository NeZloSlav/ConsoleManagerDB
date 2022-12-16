using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleManagerDB
{
    internal class User
    {
        public User(int userId, string userName, int userAge)
        {
            Id = userId;
            Name = userName;
            Age = userAge;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

    }

    internal class PrintUsers
    {
        public PrintUsers(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand
            {
                CommandText = "SELECT * FROM Users",
                Connection = connection
            };

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetValue(1)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");
                }
            }
            reader.Close();
        }

        public PrintUsers(List<User> users)
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=.\SQLEXPRESS;Database=userdb;Trusted_Connection=True";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            Console.WriteLine("Подключение открыто");




            connection.Close();
            Console.WriteLine("Подключение закрыто...");

        }

        private static List<User> GetUsersList(SqlConnection connection)
        {
            List<User> users = new List<User>();

            SqlCommand command = new SqlCommand
            {
                CommandText = "SELECT * FROM Users",
                Connection = connection
            };

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string Name = reader.GetString(1);
                    int Age = reader.GetInt32(2);

                    User user = new User(Id, Name, Age);
                    users.Add(user);
                }
            }
            reader.Close();

            Console.WriteLine("Список пользователей создан.");

            return users;
        }
    }
}
