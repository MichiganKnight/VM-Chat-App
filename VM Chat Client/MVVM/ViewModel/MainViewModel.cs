using System.Collections.ObjectModel;
using System.Windows;
using VM_Chat_Client.MVVM.Core;
using VM_Chat_Client.MVVM.Model;
using VM_Chat_Client.Net;

namespace VM_Chat_Client.MVVM.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }

        public string Username { get; set; }

        private Server _server;

        public MainViewModel()
        {
            Users = [];

            _server = new Server();
            _server.connectedEvent += UserConnected;

            ConnectToServerCommand = new(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
        }

        private void UserConnected()
        {
            UserModel user = new()
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage()
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
