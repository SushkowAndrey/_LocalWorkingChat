using ModelData;
using static Logging.Logging;

namespace SerializationData
{
    /// <summary>
    /// Работа с БД
    /// </summary>
    public class DBConnect
    {
        
        
        /*
        /// <summary>
        /// Получение данных администратора
        /// </summary>
        /// <returns>Данные администратора</returns>
        public User GetAdmin()
        {
            User admin = new User();
            try
            {
                connection.Open();
                var sql = 
                    $"SELECT " +
                    $"id," +
                    $"name_user," +
                    $"is_active," +
                    $"is_admin " +
                    $"FROM table_users " +
                    $"WHERE is_admin = 'true';";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    admin.id = result.IsDBNull(0) ? null :  result.GetString(0);
                    admin.nameUser = result.IsDBNull(1) ? null :  result.GetString(1);
                    admin.isActive = !result.IsDBNull(2) && result.GetBoolean(2);
                    admin.isActive = !result.IsDBNull(3) && result.GetBoolean(3);
                }
                connection.Close();
                if (!string.IsNullOrEmpty(admin.id))
                {
                    return admin;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show("Ошибка чтения БД GetAdmin-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                                ". Трассировка стека: " + ex.StackTrace, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                LogError("Ошибка чтения БД GetAdmin-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }
            return null;
        }*/
        
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public void RegistrationUserOnline(User user)
        {
            /*try
            {
                connection.Open();
                string sql = $"INSERT INTO table_users_online (users_id, date_time) " +
                             $"VALUES ('{user.id}','{DateTime.Now:s}');";
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = sql
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine($"{DateTime.Now:u}-Ошибка чтения БД RegistrationUser-Исключение: "+ex.Message+". Метод: "+ex.TargetSite+". Трассировка стека: "+ex.StackTrace);
                LogError("Ошибка чтения БД RegistrationUser-Исключение: " + ex.Message + ". Метод: " + ex.TargetSite +
                         ". Трассировка стека: " + ex.StackTrace);
            }*/
        }
        
    }
}