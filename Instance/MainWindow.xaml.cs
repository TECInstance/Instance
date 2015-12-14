using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Instance {
    public partial class MainWindow {
        public static TcpClient Client = new TcpClient();
        public static bool IsLocked = true;
        public static string InstanceIp = GetLocalIpAddress();
        public static string Username;
        public static string EllipseColour = "#FF00FF00";
        public static Brush DivBrush = (Brush) new BrushConverter().ConvertFrom("#E51400");

        public MainWindow() {
            var loginWindow = new LoginWindow();
            Hide();
            loginWindow.Show();
            LoadContacts();

            InitializeComponent();

            loginWindow.Closed += delegate {
                if (loginWindow.LoginSuccess) {
                    Show();
                    DivBrush = GlowBrush;
                }
                else {
                    Close();
                }
            };
        }

        public ObservableCollection<Contact> ContactList { get; } = new ObservableCollection<Contact>();

        private void LoadContacts() {
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            using (var con = new SqlConnection(connectionString)) {
                var dt = new DataTable();
                try {
                    con.Open();
                }
                catch (Exception) {
                    MessageBox.Show("Connection failed");
                }

                var command = new SqlCommand("select * from logins", con);
                var dr = command.ExecuteReader();
                dt.Load(dr);

                foreach (DataRow row in dt.Rows) {
                    EllipseColour = row.Field<string>("status") == "Offline" ? "#424242" : "#FF00FF00";

                    ContactList.Add(new Contact {
                        Name = row.Field<string>("usernames"),
                        Title = row.Field<string>("title"),
                        Status = EllipseColour
                    });

                    EllipseColour = row.Field<string>("status") == "Offline" ? "#424242" : "#FF00FF00";
                }
            }
        }

        public static string GetLocalIpAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void MetroWindow_Activated(object sender, EventArgs e) {
            Divider.Background = DivBrush;
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e) {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#808080");
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter && ChatInput.Text != "") {
                InsertIntoChat(ChatInput.Text);
                ChatInput.Text = null;
            }
        }

        private void InsertIntoChat(string str) {
            ChatText.AppendText(str + "\n");
            ChatText.ScrollToEnd();
        }

        private void chatText_TextChanged(object sender, TextChangedEventArgs e) {
        }
    }

    public class Contact {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}