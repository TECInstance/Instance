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

namespace Instance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static TcpClient client = new TcpClient();

        private void button_Click(object sender, RoutedEventArgs e) {
            var message = IpInput.Text;
            string ip = "";
            if (DEBUGIPCHECK.IsChecked.Value) {
                ip = "172.26.0.255";
            }
            else {
                ip = IpInput.Text;
            }

            try {
                client.Connect((IPAddress.Parse(ip)), 1337);
            }
            catch (Exception) {
                MessageBox.Show("Invalid IP");
            }

            if (client.Connected) {
                ConnectBtn.IsEnabled = false;
                DisconnectBtn.IsEnabled = true;
                IpInput.IsEnabled = false;


                byte[] buffer = new byte[1000];

                NetworkStream streamclient = client.GetStream();
                byte[] sendBytes = Encoding.ASCII.GetBytes("");

                streamclient.Write(sendBytes,0,sendBytes.Length);
                
            }

            
        }

        private void ChatInput_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            TrySend(ChatInput.Text);
        }

        private static void TrySend(string str) {
            try
            {
                NetworkStream streamclient = client.GetStream();
                byte[] sendBytes = Encoding.ASCII.GetBytes(str);

                streamclient.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception)
            {
                Debug.WriteLine("Can't connect to server (this is bad)");
            }
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            client.Close();
        }

        private void button_Click_1(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 100; i++) {
                TrySend(Properties.Resources.ThousandChars);
            }
            
        }
    }
}
