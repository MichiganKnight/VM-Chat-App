using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace VM_Chat_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;

        public MainWindow()
        {
            InitializeComponent();

            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("192.186.4.228", 442);

                NetworkStream stream = client.GetStream();

                reader = new(stream, Encoding.UTF8);
                writer = new(stream, Encoding.UTF8);

                Thread receiveThread = new(ReceiveMessages)
                {
                    IsBackground = true
                };

                receiveThread.Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error Connecting to Server: {ex.Message}");
            }
        }

        private void ReceiveMessages(object? obj)
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();

                    if (message != null)
                    {
                        Dispatcher.Invoke(() => ChatBox.AppendText(message + "\n"));
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Disconnected From Server");

                client.Close();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (client != null && client.Connected)
            {
                writer.WriteLine(MessageBox.Text);
                writer.Flush();

                MessageBox.Clear();
            }
        }
    }
}