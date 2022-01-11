using System;
using System.Net.Sockets;

namespace ModelData
{
    /// <summary>
    /// Интерфейс работы с сетью
    /// </summary>
    public interface INetwork
    {
        void Connect(User user, NetworkStream stream);
        void SendMessage<T>(T message, NetworkStream stream);
        void ReceiveMessage(NetworkStream stream, Action getListUsers, Action <string> updatePanelMessage);
        void Disconnect(TcpClient client, NetworkStream stream);
    }
}