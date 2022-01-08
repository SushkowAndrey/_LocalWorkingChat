using System;
using System.Text.Json;

namespace SerializationData
{
    /// <summary>
    /// Класс работы с файлами Json
    /// </summary>
    public class WorkingJson
    {
        /// <summary>
        /// Метод класса - сериализация данных по шаблону
        /// </summary>
        /// <param name="data">Передаваемый класс</param>
        /// <typeparam name="T">Шаблон параметра</typeparam>
        /// <returns>Строка в виде json</returns>
        public static string SerializationJson <T> (T data)
        {
            return JsonSerializer.Serialize(data);
        }
        /*public static string DeserializationJson <T> (T data, string json)
        {
            //return JsonSerializer.Deserialize<>(json);
        }*/
    }
}