using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using ModelData;
using static SerializationData.WorkingJson;

namespace LocalWorkingChat
{
    /// <summary>
    /// Работа клиента с сетью
    /// </summary>
    public class NetworkWorking : INetwork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="client"></param>
        /// <param name="stream"></param>
        public void Connect(User user, TcpClient client, NetworkStream stream)
        {
            //данные пользователя сериализуем в json
            string messageUser = SerializationJson(user);
            //перекодируем наше сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(messageUser);
            //отправка данных у потока
            stream.Write(data, 0, data.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stream"></param>
        /// <typeparam name="T"></typeparam>
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
        //TODO
        public void ReceiveMessage(NetworkStream stream)
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
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
                        //TODO
                        //GetListUsers();
                    }
                    //TODO
                    //UpdatePanelMessage(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("3 "+ex.Message);
                    break;
                }
            }
        }
        /// <summary>
        /// 
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