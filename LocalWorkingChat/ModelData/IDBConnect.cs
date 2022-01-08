using System.Collections.ObjectModel;

namespace ModelData
{
    public interface IDBConnect
    {
        bool CheckRegistration(User user);
        void RegistrationUser(User user);
        ObservableCollection <User> GetListUsers();
        ObservableCollection <Message> GetMessageHistory(string parametr);
    }
}