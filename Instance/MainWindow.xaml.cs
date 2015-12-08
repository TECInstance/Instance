﻿using System;
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
        public static string Username;

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
            try {
                Client.Connect(IPAddress.Parse(InstanceIp), 1337);
            }
            catch (Exception) {
                MessageBox.Show("Instance Servers seems to be offline, logging in anyway");
            }

            if (Client.Connected) {
                TrySend("INSTANCEINIT " + GetLocalIpAddress() + " " + Username);
            }
        }

        private static void TrySend(string str) {
            try {
                var _streamclient = Client.GetStream();
                var _sendBytes = Encoding.ASCII.GetBytes(str);

                _streamclient.Write(_sendBytes, 0, _sendBytes.Length);
            }
            catch (Exception) {
                Debug.WriteLine("Can't connect to server (this is bad)");
            }
        }

        public static string GetLocalIpAddress() {
            var _host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var _ip in _host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return _ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {
            var _settingsWindow = new SettingsWindow();
            _settingsWindow.Show();
        }

        private void MetroWindow_Activated(object sender, EventArgs e) {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#BE1707");
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e) {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#808080");
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e) {
            ContactList.Items.Add(new User() {Name = "test", Ip = "0.0.0.0", Status = false});
            if (e.Key == Key.Enter && ChatInput.Text != "") {
                InsertIntoChat(ChatInput.Text);
                ChatInput.Text = null;
            }
        }

        private void InsertIntoChat(string str) {
            chatText.AppendText("\n" + str);
            chatText.ScrollToEnd();
        }

        private void chatText_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }

    public class User {
        public string Name { get; set; }
        public string Ip { get; set; }
        public bool Status { get; set; }
    }
}