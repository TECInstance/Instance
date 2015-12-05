using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
using MahApps.Metro.Controls;



namespace Instance {
    

    public partial class MainWindow {
        public static TcpClient Client = new TcpClient();
        public static bool IsLocked = true;
        public static string InstanceIp = GetLocalIpAddress();
        public static string username;

        public MainWindow() {
            var _loginWindow = new LoginWindow();
            Hide();
            _loginWindow.Show();

            InitializeComponent();

            _loginWindow.Closed += delegate {
                if (_loginWindow.LoginSuccess) {
                    Show();
                }
                else {
                    MessageBox.Show("Unresolved authentication - evaded login fail");
                    Close();
                }
            };

        }

        public static void InitializeConnection() {
            
            try{
                Client.Connect(IPAddress.Parse(InstanceIp), 1337);
            }
            catch (Exception) {
                MessageBox.Show("Instance Servers seems to be offline, logging in anyway");
            }

            if (Client.Connected){
                TrySend("INSTANCEINIT " + GetLocalIpAddress() + " " + username);
            }
        }

        private static void TrySend(string _str) {
            try {
                var _streamclient = Client.GetStream();
                var _sendBytes = Encoding.ASCII.GetBytes(_str);

                _streamclient.Write(_sendBytes, 0, _sendBytes.Length);
            }
            catch (Exception) {
                Debug.WriteLine("Can't connect to server (this is bad)");
            }
        }

        public static string GetLocalIpAddress()
        {
            var _host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var _ip in _host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                return _ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var _settingsWindow = new SettingsWindow();
            _settingsWindow.Show();
        }
    }
}