using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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
using Instance.Annotations;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Instance
{
    public partial class MainWindow
    {
        public static TcpClient Client = new TcpClient();
        public static bool IsLocked = true;
        public static string InstanceIp = GetLocalIpAddress();
        public static string Username;
        public static string EllipseColour = "#FF00FF00";
        public static Brush DivBrush = (Brush) new BrushConverter().ConvertFrom("#E51400");
        private ObservableCollection<Contact> _ContactList = new ObservableCollection<Contact>();
        public ObservableCollection<Contact> ContactList => _ContactList;

        public MainWindow()
        {
            var _loginWindow = new LoginWindow();
            Hide();
            _loginWindow.Show();
            LoadContacts();

            InitializeComponent();

            _loginWindow.Closed += delegate
            {
                if (_loginWindow.LoginSuccess)
                {
                    Show();
                    DivBrush = GlowBrush;
                }
                else
                {
                    MessageBox.Show("Unresolved authentication - evaded login fail");
                    Close();
                }
            };
        }

        private void LoadContacts () {
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            using (var _con = new SqlConnection(connectionString))
            {
                var _dt = new DataTable();
                try
                {
                    _con.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("Connection failed");
                }

                var _command = new SqlCommand("select * from logins", _con);
                var _dr = _command.ExecuteReader();
                _dt.Load(_dr);

                foreach (DataRow _row in _dt.Rows) {
                    if (_row.Field<string>("status") == "Offline")
                    {
                        EllipseColour = "#424242";
                    }
                    else
                    {
                        EllipseColour = "#FF00FF00";
                    }

                    _ContactList.Add(new Contact
                    {
                        Name = _row.Field<string>("usernames"),
                        Title = _row.Field<string>("title"),
                        Status = EllipseColour
                    });

                    if (_row.Field<string>("status") == "Offline") {
                        EllipseColour = "#424242";
                    }
                    else {
                        EllipseColour = "#FF00FF00";
                    }
                }
            }
        }

        public static void InitializeConnection()
        {
            try
            {
                Client.Connect(IPAddress.Parse(InstanceIp), 1337);
            }
            catch (Exception)
            {
                MessageBox.Show("Instance Servers seems to be offline, logging in anyway");
            }

            if (Client.Connected)
            {
                TrySend("INSTANCEINIT " + GetLocalIpAddress() + " " + Username);
            }
        }

        private static void TrySend(string str)
        {
            try
            {
                var _streamclient = Client.GetStream();
                var _sendBytes = Encoding.ASCII.GetBytes(str);

                _streamclient.Write(_sendBytes, 0, _sendBytes.Length);
            }
            catch (Exception)
            {
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

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            Divider.Background = DivBrush;
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e)
        {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#808080");
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ChatInput.Text != "")
            {
                InsertIntoChat(ChatInput.Text);
                ChatInput.Text = null;
            }
        }

        private void InsertIntoChat(string str)
        {
            ChatText.AppendText(str + "\n");
            ChatText.ScrollToEnd();
        }

        private void chatText_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}