using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelData;
using MySql.Data.MySqlClient;

namespace LocalWorkingChat
{
    /// <summary>
    /// Класс для подключения к БД
    /// </summary>
    public class DBConnect : IDBConnect
    {
        string connectionString =
            "Server=mysql60.hostland.ru;Database=host1323541_itstep37;Uid=host1323541_itstep;Pwd=269f43dc;";

        private MySqlConnection connection;

        /// <summary>
        /// Конструктор подключения к БД-при инициализации класса
        /// </summary>
        public DBConnect()
        {
            //подключение к БД
            connection = new MySqlConnection(connectionString);
        }
        /// <summary>
        /// Проверка регистрации пользователя на сервере
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        /// <returns>Результат проверки-пользователь зарегистрирован (true) или нет (false)</returns>
        public bool CheckRegistration(User user)
        {
            bool flag = false;
            try
            {
                connection.Open();
                var sql = $"SELECT id FROM table_users_chat WHERE name_user = '{user.nameUser}';";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                var result = command.ExecuteReader();
                int id = 0;
                while (result.Read())
                {
                    id = Convert.ToInt32(result.GetString(0));
                }

                connection.Close();
                if (id > 0)
                {
                    flag = true;
                }

                return flag;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(
                    "Ошибка чтения БД CheckRegistration-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                    ". Трассировка стека: " + ex.StackTrace, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return flag;
            }
        }
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        public void RegistrationUser(User user)
        {
            try
            {
                connection.Open();
                string sql = $"INSERT INTO table_users_chat (name_user, password) " +
                             $"VALUES ('{user.nameUser}','{user.password}');";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(
                    "Ошибка чтения БД RegistrationUser-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                    ". Трассировка стека: " + ex.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Получение списка пользователей онлайн
        /// </summary>
        /// <returns>Список пользователей</returns>
        public ObservableCollection<User> GetListUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User> { };
            try
            {
                connection.Open();
                var sql = $"SELECT * FROM table_users_chat_online;";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            users.Add(new User()
                            {
                                id = reader.IsDBNull(0) ? null : reader.GetString(0),
                                nameUser = reader.IsDBNull(1) ? null : reader.GetString(1)
                            });
                        }
                    }
                }
                connection.Close();
                return users;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(
                    "Ошибка чтения БД GetUsersOnline-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite + ". Трассировка стека: " + ex.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return users;
            }
        }
        /// <summary>
        /// Получение истории сообщений общего чата
        /// </summary>
        /// <returns>Архив сообщений</returns>
        public ObservableCollection<Message> GetMessageHistory(string parametr)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message> { };
            try
            {
                connection.Open();
                var sql = $"SELECT * FROM table_message WHERE recipient = '{parametr}';";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            messages.Add(new Message()
                            {
                                id = reader.IsDBNull(0) ? null : reader.GetString(0),
                                textMessage = reader.IsDBNull(1) ? null : reader.GetString(1),
                                sender = reader.IsDBNull(2) ? null : reader.GetString(2),
                                recipient = reader.IsDBNull(3) ? null : reader.GetString(3),
                                dateTime = reader.IsDBNull(3) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
                connection.Close();
                return messages;
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(
                    "Ошибка чтения БД GetMessageHistory-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite + ". Трассировка стека: " + ex.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return messages;
            }
        }
    }
}