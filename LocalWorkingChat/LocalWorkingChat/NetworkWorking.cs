using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using ModelData;
using static SerializationData.WorkingJson;
using Tulpep.NotificationWindow;

namespace LocalWorkingChat
{
    /// <summary>
    /// Работа клиента с сетью
    /// </summary>
    public class NetworkWorking : INetwork
    {
        /// <summary>
        /// Метод подключения к БД
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="stream">Поток</param>
        public void Connect(User user, NetworkStream stream)
        {
            //данные пользователя сериализуем в json
            string messageUser = SerializationJson(user);
            //перекодируем наше сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(messageUser);
            //отправка данных у потока
            stream.Write(data, 0, data.Length);
        }
        /// <summary>
        /// Метод отправки сообщений
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="stream">Поток</param>
        /// <typeparam name="T">Тип класса сообщений</typeparam>
        public void SendMessage<T>(T message, NetworkStream stream)
        {
            try
            {
                //сериализация сообщения
                string messageSerialization = SerializationJson(message);
                //получение байтового массива сообщения и отправка
                byte[] data = Encoding.Unicode.GetBytes(messageSerialization);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения БД DeleteData-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace, 
                    "Ошибка отправки сообщения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Метод получения сообщений от сервера
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="getListUsers">Делегат обновления списка пользователей</param>
        /// <param name="updatePanelMessage">Делегат панели сообщений</param>
        public void ReceiveMessage(NetworkStream stream, Action getListUsers, Action <string> updatePanelMessage)
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes;
                    //получение входящих данных-возвращает значение true, если в потоке есть данные. Если их нет, возвращается false.
                    do
                    {
                        //чтение получаемых данных
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    string message = builder.ToString();
                    if (message.IndexOf("Авторизация")!=0) 
                    { 
                        getListUsers(); 
                    } 
                    updatePanelMessage(message);
                }
                catch
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Отключение от сервера и закрытие потока
        /// </summary>
        /// <param name="client"></param>
        /// <param name="stream"></param>
        public void Disconnect(TcpClient client, NetworkStream stream)
        {
            if (stream != null)
            {
                stream.Close(); //отключение потока
            }
            if (client != null)
            {
                client.Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }

    }
}