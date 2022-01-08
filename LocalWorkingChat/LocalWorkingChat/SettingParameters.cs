using System.IO;
using System.Text.Json;
using ModelData;

namespace LocalWorkingChat
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingParameters
    {
        /// <summary>
        /// Метод получения данных пользователя из файла json
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Получаем класс с текущими данными</returns>
        public User ImportUserData(string path)
        {
            var user = new User();
            try
            {
                using (var file = new FileStream(path, FileMode.Open))
                {
                    user = JsonSerializer.DeserializeAsync<User>(file).Result;
                }
                return user;
            }
            catch
            {
                return user;
            }
        }
        /// <summary>
        /// Метод записи/перезаписи параметров в файл
        /// </summary>
        /// <param name="user">Передаем класс с данными пользователя для записи</param>
        public void WritingFileUserData(User user)
        {
            using (StreamWriter file = new StreamWriter("User.json", false))
            {
                string json = JsonSerializer.Serialize(user);
                file.WriteLine(json);
            }
        }
    }
}