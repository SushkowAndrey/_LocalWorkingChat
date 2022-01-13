using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ModelData;
using Tulpep.NotificationWindow;
using static SerializationData.WorkingJson;
using Color = System.Drawing.Color;

namespace LocalWorkingChat
{
    /// <summary>
    /// Основное окно пользователя
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Свойство класса - ip адрес сервера
        /// </summary>
        private string ipAddress = "192.168.1.34";
        /// <summary>
        /// Свойство класса - порт сервера
        /// </summary>
        private int port = 8008;
        /// <summary>
        /// Свойство класса - класс для создания клиентской программы, работающей по протоколу TCP
        /// </summary>
        private static TcpClient client;
        /// <summary>
        /// Свойство класса - взаимодействие с сервером-через данный объект можно передавать сообщения серверу или, наоборот, получать данные с сервера
        /// </summary>
        private static NetworkStream stream;
        /// <summary>
        /// Свойство класса - модель данных пользователя
        /// </summary>
        private User user = new User();
        /// <summary>
        /// Свойство класса - сообщение
        /// </summary>
        private Message message = new Message();
        /// <summary>
        /// Класс подключения к БД
        /// </summary>
        private DBConnect dbConnect = new DBConnect();
        /// <summary>
        /// Параметры сохранения текущего пользователя
        /// </summary>
        private SettingParameters settingParameters = new SettingParameters();
        /// <summary>
        /// Класс работы с сетью
        /// </summary>
        private NetworkWorking networkWorking = new NetworkWorking();
        /// <summary>
        /// Конструктор класса инициализации окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Closing += OnClosing;
        }
        //СОБЫТИЯ ФОРМЫ
        /// <summary>
        /// События при загрузке окна
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GetUserData();
            Button_send.IsEnabled = false;
            TextBox_message.IsEnabled = false;
        }
        /// <summary>
        /// События при выходе
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            networkWorking.Disconnect(client, stream);
        }
        //МЕТОДЫ ФОРМЫ
        /// <summary>
        /// Получение полной истории
        /// </summary>
        private void GetMessageHistory()
        {
            var Messages = dbConnect.GetMessageHistory("Общий чат");
            foreach (var message in Messages)
            {
                TextBlock_messages.Text += $"{message.dateTime}-{message.sender}: {message.textMessage}\n";
            }
            ScrollViewer_messages.ScrollToEnd();
        }
        /// <summary>
        /// Получение имени пользователя и пароля из файла при загрузке программы
        /// </summary>
        private void GetUserData()
        {
            try
            {
                user = settingParameters.ImportUserData("User.json");
                TextBox_nameUser.Text = user.nameUser;
                TextBox_passwordUser.Text = user.password;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка GetUserData", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Получение списка пользователей из БД в режиме онлайн
        /// </summary>
        private void GetListUsers()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    DataGrid_usersTable.ItemsSource = null;
                    DataGrid_usersTable.ItemsSource = dbConnect.GetListUsers();
                }
            );
        }
        /// <summary>
        /// Обновление панели сообщений
        /// </summary>
        /// <param name="message">Новое сообщений</param>
        void UpdatePanelMessage(string message)
        {
            // Получить диспетчер от текущего окна и использовать его для вызова кода обновления
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    PopupNotifier popup = new PopupNotifier();
                    popup.TitleText = "Новое уведомление";
                    popup.ContentText = message;
                    popup.HeaderColor = Color.Brown;
                    popup.TitleColor = Color.Maroon;
                    popup.BodyColor = Color.Aqua;
                    popup.Popup(); 
                    TextBlock_messages.Text += message + '\n';
                }
            );
        }
        //КНОПКИ ФОРМЫ
        /// <summary>
        /// Кнопка регистрации пользователя в БД
        /// </summary>
        private void Button_registration_OnClick(object sender, RoutedEventArgs e)
        {
            user.nameUser = TextBox_nameUser.Text;
            user.password = TextBox_passwordUser.Text;
            var resultCheckDb = dbConnect.CheckRegistration(user);
            if (user.nameUser==String.Empty||user.password==String.Empty)
            {
                MessageBox.Show("Проверьте наличие данных", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(!resultCheckDb)
            {
                dbConnect.RegistrationUser(user);
                MessageBox.Show("Пользователь зарегистрирован", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                settingParameters.WritingFileUserData(user);
            }
            else
            {
                MessageBox.Show("Пользователь уже зарегистрирован", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                settingParameters.WritingFileUserData(user);
            }
        }
        /// <summary>
        /// Кнопка авторизации пользователя на сервере онлайн
        /// </summary>
        private void Button_checkConnect_OnClick(object sender, RoutedEventArgs e)
        {
            if (user.nameUser==String.Empty||user.password==String.Empty)
            {
                MessageBox.Show("Проверьте наличие данных", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                message.sender = user.nameUser;
                client = new TcpClient();
                try
                { 
                    client.Connect(ipAddress, port); //подключение клиента к ip и порту-в данном случае к серверу
                    stream = client.GetStream(); // получаем поток
                    networkWorking.Connect(user,stream);
                    // запускаем новый поток для получения данных
                    Thread receiveThread = new Thread(() =>
                    {
                        networkWorking.ReceiveMessage(stream, GetListUsers, UpdatePanelMessage);
                    });
                    receiveThread.Start(); //старт потока
                    Button_registration.IsEnabled = false;
                    Button_checkConnect.IsEnabled = false;
                    Button_send.IsEnabled = true;
                    TextBox_message.IsEnabled = true;
                    GetListUsers();
                    GetMessageHistory();
                    TextBlock_messages.Text += $"Добро пожаловать, {user.nameUser}\n";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка подключения к серверу", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// Кнопка отправки сообщения в общий чат через сервер
        /// </summary>
        private void Button_send_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessages();
        }
        /// <summary>
        /// Отправка сообщения при нажатии на кнопку Enter
        /// </summary>
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter&&Button_send.IsEnabled)
            {
                SendMessages();
            }
        }
        /// <summary>
        /// Метод отправки сообщений
        /// </summary>
        private void SendMessages()
        {
            message.textMessage = TextBox_message.Text;
            message.dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            if (message.textMessage == String.Empty)
            {
                TextBlock_warning.Text = "Введите сообщение";
            }
            else
            {
                TextBlock_warning.Text = String.Empty;
                networkWorking.SendMessage(message,stream);
                TextBox_message.Clear();
            }
        }
    }
}