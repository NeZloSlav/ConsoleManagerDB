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
        public static void FromDB(SqlConnection connection)
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

        public static void FromList(List<User> users)
        {
            Console.WriteLine("Id\tName\tAge");

            foreach(User user in users) 
            {
                Console.WriteLine($"{user.Id}\t{user.Name}\t{user.Age}");
            }
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

        static void DeleteUser(SqlConnection connection)
        {
            Console.Write("Введите id: ");
            int id = int.Parse(Console.ReadLine());


            SqlCommand command = new SqlCommand
            {
                CommandText = "DELETE Users WHERE Id = @id",
                Connection = connection
            };

            SqlParameter idParam = new SqlParameter("@id", id);
            command.Parameters.Add(idParam);

            int lines = command.ExecuteNonQuery();

            if (lines > 0)
            {
                Console.WriteLine("Успешно! Затронуто строк ({0})", lines);
            }
            else
            {
                Console.WriteLine("Пользователя с id = {0} не существует",id);
            }

        }

        class UpdateUser
        {
            private static int ID;

            private static void InputUserID()
            {
                Console.Write("Введите id: ");
                int id = int.Parse(Console.ReadLine());

                ID = id;
            }


            private static void ForName(SqlConnection connection)
            {
                InputUserID();

                Console.Write("Введите новое имя: ");
                string Name = Console.ReadLine();

                SqlCommand command = new SqlCommand
                {
                    CommandText = "UPDATE Users SET Name = @name WHERE Id = @id",
                    Connection = connection
                };

                SqlParameter[] parameters =
                {
                    new SqlParameter("@name", Name),
                    new SqlParameter("@id", ID)
                };

                command.Parameters.Add(parameters);

                int lines = command.ExecuteNonQuery();

                if (lines > 0)
                {
                    Console.WriteLine("Данные успешно обновлены!");
                }
                else
                {
                    Console.WriteLine("Данные не обновлены. Пользователя с данным id не существует...");
                }
            }

            private static void ForAge(SqlConnection connection)
            {
                InputUserID();

                Console.WriteLine("Введите возраст: ");
                int Age = int.Parse(Console.ReadLine());

                SqlCommand command = new SqlCommand
                {
                    CommandText = "UPDATE Users SET Age = @age WHERE Id = @id",
                    Connection = connection
                };

                SqlParameter[] parameters =
                {
                    new SqlParameter("@age", Age),
                    new SqlParameter("@id", ID)
                };

                command.Parameters.Add(parameters);

                int lines = command.ExecuteNonQuery();

                if (lines > 0)
                {
                    Console.WriteLine("Данные успешно обновлены!");
                }
                else
                {
                    Console.WriteLine("Данные не обновлены. Пользователя с данным id не существует.");
                }
            }

        }

    }


}
