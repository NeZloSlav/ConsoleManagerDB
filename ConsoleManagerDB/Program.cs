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

        static void AddUser(SqlConnection connection)
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите возраст: ");
            int age;
            if (int.TryParse(Console.ReadLine(), out age) == false) Console.WriteLine("Введите числовое значение");


            SqlCommand command = new SqlCommand
            {
                CommandText = "INSERT INTO Users (Name, Age) VALUES (@name, @age)",
                Connection = connection
            };

            SqlParameter[] parameters =
            {
                new SqlParameter("@name", name),
                new SqlParameter("@age", age)
            };

            command.Parameters.Add(parameters);
            int lines = command.ExecuteNonQuery();

            if (lines > 0)
            {
                Console.WriteLine("Успешно добавлено! Затронуто ({0}) строк", lines);
            }
        }
    }


}
