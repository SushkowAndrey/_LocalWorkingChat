using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ModelData;

namespace LocalServerChat
{
    /// <summary>
    /// Работа сервера
    /// </summary>
    public class ServerObject
    {
        /// <summary>
        /// Свойство класса - сервер для прослушивания
        /// </summary>
        static TcpListener tcpListener;
        /// <summary>
        /// Свойство класса - все подключения клиентов
        /// </summary>
        List <ClientObject> clients = new List<ClientObject>(); 
        /// <summary>
        /// Метод класса - прослушивание входящих подключений
        /// </summary>
        public void ListenConnect()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8008);//прослушивание всех входящих подключений для порта
                tcpListener.Start();//запуск прослушивания
                Console.WriteLine($"{DateTime.Now:u}-Ожидание подключений...");
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);//создание экземпляра клиента
                    AddConnection(clientObject);
                    Thread clientThread = new Thread(clientObject.ProcessClient);//создание потока для клиента
                    Console.WriteLine($"{DateTime.Now:u}-Создание потока для клиента-{clientThread.GetHashCode()}");
                    clientThread.Start();//запуск клиента
                    ShowListUser($"Подключение клиента {clientObject.user.nameUser}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTime.Now:u}-Ошибка ListenConnect-{ex.Message}");
            }
        }
        /// <summary>
        /// Метод класса - трансляция сообщения подключенным клиентам-общая рассылка
        /// </summary>
        public void SendMessage(Message message)
        {
            string sengMessage = $"{DateTime.Now:u}-{message.sender}: {message.textMessage}";
            byte[] data = Encoding.Unicode.GetBytes(sengMessage);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Stream.Write(data, 0, data.Length); //передача данных всем
            }
        }
        //TODO
        /// <summary>
        /// Метод класса - трансляция сообщения подключенным клиентам-персональная рассылка
        /// </summary>
        private void BroadcastMessage(string message, string recipientId)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            //ищем клиента с таким же именем
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].user.nameUser == recipientId) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
        /// <summary>
        /// Метод класса - добавление соединения при подключении в список
        /// </summary>
        private void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        /// <summary>
        /// Метод класса - удаление соединения при отключении из списка
        /// </summary>
        public void RemoveConnection(string nameUser)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.user.nameUser == nameUser);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
            ShowListUser($"Отключение клиента {nameUser}");
        }
        /// <summary>
        /// Отображение списка пользователей при изменении
        /// </summary>
        private void ShowListUser(string currentProccess)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now:u}-Обновлен список пользователей (операция-{currentProccess})");
            foreach (var client in clients)
            {
                Console.WriteLine($"Клиент онлайн-{client.user.nameUser}");
            }
        }
        /// <summary>
        /// Метод класса - отключение всех клиентов
        /// </summary>
        public void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}