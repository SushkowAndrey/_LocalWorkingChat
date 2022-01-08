using System;
using System.Threading;
using ModelData;

namespace LocalServerChat
{
    /// <summary>
    /// Основной класс сервера
    /// </summary>
    class Program
    {
        /// <summary>
        /// Класс подключения к БД
        /// </summary>
        private static DBConnect dbConnect = new DBConnect();
        /// <summary>
        /// Свойство класса - модель данных пользователя
        /// </summary>
        private static User user = new User();
        /// <summary>
        /// Свойство класса - сервер
        /// </summary>
        private static ServerObject server;
        /// <summary>
        /// Свойство класса - поток для прослушивания
        /// </summary>
        private static Thread listenThread;
        /// <summary>
        /// Исходная точка программы сервера
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                dbConnect.DeleteData();
                //запись сервета в таблицу онлайн
                user.id = "01";
                user.nameUser = "Server";
                dbConnect.RegistrationUserOnline(user);
                //запуск сервера
                server = new ServerObject();
                listenThread = new Thread(server.Listen);
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine($"Ошибка сервера-{ex.Message}");
            }
        }
    }
}