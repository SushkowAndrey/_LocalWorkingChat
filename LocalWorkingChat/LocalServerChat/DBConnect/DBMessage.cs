﻿using System;
using System.IO;
using System.Threading.Tasks;
using ModelData;
using MySql.Data.MySqlClient;

namespace LocalServerChat
{
    /// <summary>
    /// Класс работы с сообщениями на сервере асинхронно
    /// </summary>
    public class DBMessage
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        static string connectionString;
        /// <summary>
        /// Подключение к БД
        /// </summary>
        private static MySqlConnection connection = new MySqlConnection(connectionString);
        /// <summary>
        /// Подключение к БД
        /// </summary>
        public static void ConnectDB()
        {
            StreamReader sr = new StreamReader("dbconnect.txt");
            connectionString = sr.ReadToEnd();
            sr.Close();
            connection = new MySqlConnection(connectionString);
        }
        /// <summary>
        /// Асинхнонная операция записи сообщения на сервер
        /// </summary>
        /// <param name="message"></param>
        public static async void RegistrationMessagesAsync(Message message)
        {
            try
            {
                await Task.Run(() => RegistrationMessages(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка чтения БД RegistrationMessagesAsync-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite + ". Трассировка стека: " + ex.StackTrace);
            }
        }
        /// <summary>
        /// Регистрация сообщений пользователя
        /// </summary>
        /// <param name="message"></param>
        private static void RegistrationMessages(Message message)
        {
            ConnectDB();
            try
            {
                connection.Open();
                string sql = $"INSERT INTO table_message (text_message, sender, recipient, date_time) " +
                             $"VALUES ('{message.textMessage}','{message.idSender}','{message.idRecipient}','{DateTime.Now:u}');";
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
    }
}