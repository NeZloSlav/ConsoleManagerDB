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

        public static List<User> GetUsersList(SqlConnection connection)
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

    internal class PrintUsers
    {
        private static SqlConnection connect;
        public static void Print(SqlConnection connection)
        {
            connect = connection;

            bool isExit = false;

            while(isExit != true)
            {
                Console.WriteLine("Вывести:" +
               "\n1) Напрямую из БД" +
               "\n2) Из БД с созданием списка" +
               "\n3) Назад");

                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        FromDB();
                        isExit = true;
                        break;
                    case "2":
                        FromList();
                        isExit = true;
                        break;
                    case "3":
                        isExit = true;
                        break;

                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        private static void FromDB()
        {
            SqlCommand command = new SqlCommand
            {
                CommandText = "SELECT * FROM Users",
                Connection = connect
            };

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");
                }
            }
            reader.Close();
        }

        private static void FromList()
        {
            List<User> users = User.GetUsersList(connect);

            Console.WriteLine("Id\tName\tAge");

            foreach (User user in users)
            {
                Console.WriteLine($"{user.Id}\t{user.Name}\t{user.Age}");
            }
        }
    }

    internal class InputUserData
    {
        public static int UserID()
        {
            int id;

            while (true)
            {
                Console.Write("Введите id: ");
                bool isNum = int.TryParse(Console.ReadLine(), out id);

                if (isNum)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Недопустимое значение, попробуйте ещё раз");
                }
            }

            return id;
        }

        public static string UserName()
        {
            string name;

            bool isSuitableName = true;

            while (true)
            {
                Console.Write("Введите имя: ");
                name = Console.ReadLine();

                for (int i = 0; i < name.Length; i++)
                {
                    if (Char.IsNumber(name[i]))
                    {
                        isSuitableName = false;
                        break;
                    }
                }

                if (isSuitableName)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Недопустимое значение, попробуйте ещё раз");
                    isSuitableName = true;
                }
            }

            return name;
        }

        public static int UserAge()
        {
            int age;

            while (true)
            {
                Console.Write("Введите возраст: ");
                bool isNum = int.TryParse(Console.ReadLine(), out age);

                if (isNum)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Недопустимое значение, попробуйте ещё раз");
                }
            }

            return age;
        }

    }

    internal class DataActoins
    {
        public class ViaQuery
        {
            public static void AddUser(SqlConnection connection)
            {
                string name = InputUserData.UserName();

                int age = InputUserData.UserAge();

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

            public static void DeleteUser(SqlConnection connection)
            {
                int id = InputUserData.UserID();

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
                    Console.WriteLine("Пользователя с id = {0} не существует", id);
                }

            }

            public class UpdateUser
            {
                private static readonly int ID = InputUserData.UserID();

                public static void ForName(SqlConnection connection)
                {
                    string Name = InputUserData.UserName();

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

                public static void ForAge(SqlConnection connection)
                {
                    int Age = InputUserData.UserAge();

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

        public class ViaProcedure
        {
            public static void AddUser(SqlConnection connection)
            {
                string name = InputUserData.UserName();

                int age = InputUserData.UserAge();

                SqlCommand commandProc = new SqlCommand
                {
                    CommandText = @"CREATE OR ALTER PROCEDURE [dbo].[sp_InsertUser]
                                        @name nvarchar(50),
                                        @age int
                                    AS
                                        INSERT INTO Users
                                        ([Name], Age)
                                        VALUES
                                        (@name, @age)

                                    SELECT SCOPE_IDENTITY();
                                    GO",
                    Connection = connection
                };

                commandProc.ExecuteNonQuery();

                SqlCommand command = new SqlCommand
                {
                    CommandText = "sp_InsertUser",
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlParameter[] parameters =
                {
                    new SqlParameter("@name", name),
                    new SqlParameter("@age", age)
                };

                command.Parameters.Add(parameters);

                var id = command.ExecuteScalar();

                Console.WriteLine("Успешно добавлено! Id пользователя: {0} ", id);
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
            Console.WriteLine("Добро пожаловать! Подключение открыто...");

            bool isExit = false;

            while (isExit != true)
            {
                Console.WriteLine("\nЧто вы хотите сделать?" +
                "\n1) Вывести данные о пользователях. " +
                "\n2) Добавить пользователя. " +
                "\n3) Обновить пользователя. " +
                "\n4) Удалить пользователя. " +
                "\n5) Выйти...");
                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        PrintUsers.Print(connection);
                        break;
                    case "2":
                        //DataActoins;
                        break;
                    case "3":
                        //DataActoins;
                        break;
                    case "4":
                        //DataActoins;
                        break;
                    case "5":
                        isExit = true;
                        break;
                    default:
                        Console.WriteLine("Введите номер действия.");
                        break;
                }
            }

            connection.Close();
            Console.WriteLine("Подключение закрыто...");

        }
    }


}
