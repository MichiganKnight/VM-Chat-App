using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM_Chat_Client.MVVM.Core;
using VM_Chat_Client.Net;

namespace VM_Chat_Client.MVVM.ViewModel
{
    public class MainViewModel
    {
        public RelayCommand ConnectToServerCommand { get; set; }

        private Server _server;

        public MainViewModel()
        {
            _server = new Server();

            ConnectToServerCommand = new(o => _server.ConnectToServer());
        }
    }
}
