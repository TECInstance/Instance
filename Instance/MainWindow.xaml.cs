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
            var loginWindow = new LoginWindow();
            Hide();
            loginWindow.Show();

            InitializeComponent();

            loginWindow.Closed += delegate {
                if (loginWindow.LoginSuccess) {
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

        private static void TrySend(string str) {
            try {
                var streamclient = Client.GetStream();
                var sendBytes = Encoding.ASCII.GetBytes(str);

                streamclient.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception) {
                Debug.WriteLine("Can't connect to server (this is bad)");
            }
        }

        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                return ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}