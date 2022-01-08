using System.Net.Sockets;

namespace ModelData
{
    /// <summary>
    /// Интерфейс работы с сетью
    /// </summary>
    public interface INetwork
    {
        void Connect(User user, TcpClient client, NetworkStream stream);
        void SendMessage<T>(T message, NetworkStream stream);
        void ReceiveMessage(NetworkStream stream);
        void Disconnect(TcpClient client, NetworkStream stream);
    }
}