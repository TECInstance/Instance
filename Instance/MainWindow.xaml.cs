#region HEADERS
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using Timer = System.Timers.Timer;
#endregion

namespace Instance {
    public partial class MainWindow {
        public static TcpClient TcpSender = new TcpClient();
        public static TcpListener TcpListener = new TcpListener(IPAddress.Parse(GetLocalIpAddress()), 1337);
        public bool IsChatting;
        public static bool IsLocked = true;
        public static string InstanceIp = GetLocalIpAddress();
        public static string Username;
        public static Brush DivBrush = (Brush) new BrushConverter().ConvertFrom("#E51400");
        public TcpClient OtherClient;

        public MainWindow() {
            var _loginWindow = new LoginWindow();
            Hide();
            _loginWindow.Show();

            InitializeComponent();
            DataContext = new ContactsPresenter();

            _loginWindow.Closed += delegate {
                if (_loginWindow.LoginSuccess) {
                    Show();
                    DivBrush = GlowBrush;
                }
                else {
                    MessageBox.Show("Unresolved authentication - evaded login fail");
                    Close();
                }
            };

            var _timer = new Timer(10);
            _timer.Elapsed += delegate { Listener(); };
        }

/*
        private ContactsPresenter Presenter => (ContactsPresenter) DataContext;
*/

        private void Listener() {
            TcpListener.Start();
            if (!IsChatting) {
                if (TcpListener.Pending()) {
                    OtherClient = TcpListener.AcceptTcpClient();
                    ChatText.Text += "New connection!\r\n";
                    IsChatting = true;
                }
            }

            var _clientStream = OtherClient.GetStream();

            if (IsChatting) {
                var _buffer = new byte[50];
                _clientStream.Read(_buffer, 0, _buffer.Length);
                ChatText.AppendText(Encoding.ASCII.GetString(_buffer));
                ChatText.AppendText("\r\n");
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
            Divider.Background = DivBrush;
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e) {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#808080");
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter && ChatInput.Text != "") {
                SendMessage(ChatInput.Text);
                ChatInput.Text = null;
            }
        }

        private void SendMessage(string msg) {
            InsertIntoChat(msg);
        }

        private void InsertIntoChat(string str) {
            ChatText.AppendText(str + "\n");
            ChatText.ScrollToEnd();
        }

        private void chatText_TextChanged(object sender, TextChangedEventArgs e) {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //try {
                TcpSender.Connect("192.168.0.254", 1337);
                var _sendBytes = Encoding.ASCII.GetBytes("Hello there!");
                TcpSender.GetStream().Write(_sendBytes,0,50);
            //}
            //catch (Exception) {
            //    this.ShowMessageAsync(":(", "Error - Connection Failed!");
            //}
        }
    }

    public class ContactsPresenter : INotifyPropertyChanged {
        private readonly ContactsModel _contactsM;

        public ContactsPresenter() {
            _contactsM = new ContactsModel();
            ContactNameList.Add("TestUser");

        }

        public ObservableCollection<string> ContactNameList => _contactsM.ContactNameList;

        public ObservableCollection<string> ContactTitleList => _contactsM.ContactTitleList;

        public ObservableCollection<string> ContactStatusList => _contactsM.ContactStatusList;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ContactsModel : INotifyPropertyChanged {
        public ObservableCollection<string> ContactNameList { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ContactTitleList { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ContactStatusList { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}