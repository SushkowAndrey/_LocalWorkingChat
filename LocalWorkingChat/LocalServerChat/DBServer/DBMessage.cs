using System;
using System.IO;
using System.Threading.Tasks;
using ModelData;
using MySql.Data.MySqlClient;
using static Logging.Logging;

namespace LocalServerChat.DBServer
{
    /// <summary>
    /// Класс работы с сообщениями на сервере асинхронно
    /// </summary>
    public class DBMessage
    {
        /// <summary>
        /// Подключение к БД
        /// </summary>
        private static MySqlConnection connection;

        /// <summary>
        /// Асинхнонная операция записи сообщения на сервер
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connectionString">Строка подключения</param>
        public static async void RegistrationMessagesAsync(Message message, string connectionString)
        {
            try
            {
                await Task.Run(() => RegistrationMessages(message, connectionString));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка чтения БД RegistrationMessagesAsync-Исключение: " + ex.Message + ". Метод: " + 
                                  ex.TargetSite + ". Трассировка стека: " + ex.StackTrace);
                LogError("Ошибка чтения БД RegistrationMessagesAsync-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
        }
        /// <summary>
        /// Регистрация сообщений пользователя
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connectionString">Строка подключения</param>
        private static void RegistrationMessages(Message message, string connectionString)
        {
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string sql = $"INSERT INTO table_message (id,text_message, users_id_sender, users_id_recipient, date_time, type_message) " +
                             $"VALUES ('{message.id}'," +
                             $"'{message.textMessage}'," +
                             $"'{message.idSender}'," +
                             $"IF({message.idRecipient != null},'{message.idRecipient}',NULL)," +
                             $"'{DateTime.Now:s}','{message.typeMessage}');";
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
                Console.WriteLine("Ошибка чтения БД RegistrationUser-Исключение: "+ex.Message+". Метод: "
                                  +ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
                LogError("Ошибка чтения БД RegistrationUser-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
        }
    }
}