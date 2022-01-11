using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ModelData;
using static LocalServerChat.DBMessage;
using static SerializationData.WorkingJson;

namespace LocalServerChat
{ 
    /// <summary>
    /// Класс работы с клиентом
    /// </summary>
    public class ClientObject
    {
        /// <summary>
        /// Свойство класса - модель данных пользователя-уникальный идентификатор клиента
        /// </summary>
        internal User user = new User();
        /// <summary>
        /// Свойство класса - свойство Stream, хранящее поток для взаимодействия с клиентом
        /// </summary>
        internal NetworkStream Stream {get; set;}
        /// <summary>
        /// Свойство класса - создание клиента
        /// </summary>
        TcpClient client;
        /// <summary>
        /// Свойство класса - объект сервера
        /// </summary>
        ServerObject server = new ServerObject();
        /// <summary>
        /// Свойство класса - подключение к БД
        /// </summary>
        private DBConnect dbConnect = new DBConnect();
        /// <summary>
        /// Свойство класса - сообщение
        /// </summary>
        private Message getSetMessage = new Message();
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="tcpClient">Клиент</param>
        /// <param name="serverObject">Объект сервера</param>
        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            client = tcpClient;
            server = serverObject;
        }
        /// <summary>
        /// Процесс работы сервера - прием и направление ответа на сообщения
        /// </summary>
        public void ProcessClient()
        {
            try
            {
                //получаем поток
                Stream = client.GetStream();
                string message = GetMessage();
                user = DeserializationJson<User>(message);
                Console.ForegroundColor = ConsoleColor.Blue;
                message = $"{DateTime.Now:u}-{user.nameUser} вошел в чат";
                Console.WriteLine(message);
                dbConnect.RegistrationUserOnline(user);
                getSetMessage.sender = "Server";
                getSetMessage.recipient = "Общий чат";
                getSetMessage.textMessage = message;
                // посылаем сообщение о входе в чат всем подключенным пользователям
                RegistrationMessagesAsync(getSetMessage);
                server.SendMessage(getSetMessage);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        getSetMessage = DeserializationJson<Message>(message);
                        getSetMessage.recipient = "Общий чат";
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now:u}-Отправитель - {getSetMessage.sender}, сообщение - {getSetMessage.textMessage}");
                        //регистрация сообщения
                        RegistrationMessagesAsync(getSetMessage);
                        server.SendMessage(getSetMessage);
                    }
                    catch
                    {
                        dbConnect.DeleteData(user.nameUser);
                        message = String.Format($"{DateTime.Now:u}-{user.nameUser}: покинул чат");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(message);
                        getSetMessage.sender = "Server";
                        getSetMessage.recipient = "Общий чат";
                        getSetMessage.textMessage = message;
                        RegistrationMessagesAsync(getSetMessage);
                        server.SendMessage(getSetMessage);
                        server.RemoveConnection(user.nameUser);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{DateTime.Now:u}-Ошибка ProcessClient-{e.Message}");
            }
        }
        /// <summary>
        /// Чтение входящего сообщения и преобразование в строку
        /// </summary>
        /// <returns>Получаем сообщение</returns>
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);
            return builder.ToString();
        }
        /// <summary>
        /// Закрытие подключения
        /// </summary>
        public void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}