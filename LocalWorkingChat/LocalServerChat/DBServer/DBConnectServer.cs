using System;
using ModelData;
using MySql.Data.MySqlClient;
using static Logging.Logging;

namespace LocalServerChat.DBServer
{
    /// <summary>
    /// Класс подключениея к БД сервером
    /// </summary>
    public class DBConnectServer
    {
        /// <summary>
        /// Подключение к БД
        /// </summary>
        private MySqlConnection connection;
        /// <summary>
        /// Конструктор класса подключения
        /// </summary>
        public DBConnectServer(string connectionString)
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
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД RegistrationUserOnline-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
                LogError("Ошибка чтения БД RegistrationUserOnline-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
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
                LogError("Ошибка чтения БД DeleteUserOnline-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
        }
        /// <summary>
        /// Удаление онлайн конкретного пользователя
        /// </summary>
        /// <param name="idUser">Удаляемый пользователь</param>
        /// <returns>Результат удаления</returns>
        public void DeleteUserOnline(string idUser)
        {
            try
            {
                connection.Open();
                var sql = $"DELETE FROM table_users_online WHERE users_id = '{idUser}';";
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
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД DeleteData-Исключение: "+ex.Message+". Метод: "
                                  +ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
                LogError("Ошибка чтения БД DeleteData-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
        }
        /// <summary>
        /// Получение данных админа
        /// </summary>
        /// <returns>Админ</returns>
        public User GetAdmin()
        {
            User admin = new User();
            try
            {
                connection.Open();
                var sql = 
                    $"SELECT " +
                    $"id," +
                    $"name_user," +
                    $"is_active," +
                    $"is_admin " +
                    $"FROM table_users " +
                    $"WHERE is_admin = 'true';";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    admin.id = result.IsDBNull(0) ? null :  result.GetString(0);
                    admin.nameUser = result.IsDBNull(1) ? null :  result.GetString(1);
                    admin.isActive = !result.IsDBNull(2) && result.GetBoolean(2);
                    admin.isActive = !result.IsDBNull(3) && result.GetBoolean(3);
                }
                connection.Close();
                if (!string.IsNullOrEmpty(admin.id))
                {
                    return admin;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД GetAdmin-Исключение: "+ex.Message+". Метод: "
                                  +ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
                LogError("Ошибка чтения БД GetAdmin-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
            return null;
        }
    }
}