using System;

namespace ModelData
{
    /// <summary>
    /// Модель данных сообщения
    /// </summary>
    public class Message
    {
        public string id { get; set; }
        public string textMessage { get; set; }
        public string sender { get; set; }
        public string recipient { get; set; }
        public string dateTime { get; set; }
    }
}