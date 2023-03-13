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
        /// Подключение к БД
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// Конструктор класса подключения
        /// </summary>
        public DBConnect(string connectionString)
        {
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
                string sql = $"INSERT INTO table_users_online (users_id, date_time) " +
                             $"VALUES ('{user.id}','{DateTime.Now:s}');";
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
                Console.WriteLine("{DateTime.Now:u}-Ошибка чтения БД RegistrationUser-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
            }
        }
        /// <summary>
        /// Очистка списка всех пользователей онлайн
        /// </summary>
        public void DeleteUserOnline()
        {
            try
            {
                connection.Open();
                var sql = $"DELETE FROM table_users_online;";
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
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД DeleteUserOnline-Исключение: "
                                  +ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
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
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД DeleteData-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
            }
        }
    }
}