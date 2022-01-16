using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using ModelData;
using MySql.Data.MySqlClient;

namespace LocalServerChat
{
    /// <summary>
    /// Класс подключениея к БД сервером
    /// </summary>
    public class DBConnect
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        string connectionString;
        /// <summary>
        /// Подключение к БД
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// Конструктор класса подключения
        /// </summary>
        public DBConnect()
        {
            StreamReader sr = new StreamReader("dbconnect.txt");
            connectionString = sr.ReadToEnd();
            sr.Close();
            connection = new MySqlConnection(connectionString);
        }
        /// <summary>
        /// Регистрация пользователя онлайн
        /// </summary>
        /// <param name="user">Авторизованный пользователь</param>
        public void RegistrationUserOnline(User user)
        {
            try
            {
                connection.Open();
                string sql = $"INSERT INTO table_users_chat_online (name_user, date_time) " +
                             $"VALUES ('{user.nameUser}','{DateTime.Now:u}');";
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
                Console.WriteLine("Ошибка чтения БД RegistrationUser-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
            }
        }
        /// <summary>
        /// Очистка списка всех пользователей онлайн
        /// </summary>
        public void DeleteData()
        {
            try
            {
                connection.Open();
                var sql = $"DELETE FROM table_users_chat_online;";
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
                Console.WriteLine("Ошибка чтения БД DeleteData-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
            }
        }
        /// <summary>
        /// Удаление онлайн конкретного пользователя
        /// </summary>
        /// <param name="idUser">Удаляемый пользователь</param>
        /// <param name="nameUser">Имя текущего удаляемого пользователя</param>
        /// <returns>Результат удаления</returns>
        public void DeleteData(string nameUser)
        {
            try
            {
                connection.Open();
                var sql = $"DELETE FROM table_users_chat_online WHERE name_user = '{nameUser}';";
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
                Console.WriteLine("Ошибка чтения БД DeleteData-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
            }
        }
    }
}