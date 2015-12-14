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
            var loginWindow = new LoginWindow(); // Creates new instance of LoginWindow
            Hide(); // Hides MainWindow
            loginWindow.Show(); // Shows LoginWindow
            LoadContacts(); // Generates all contacts in ListView

            InitializeComponent(); // Generates contents of MainWindow
            
            // When LoginWindow is closed, it checks if login has been successful
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

        // List of contacts
        public ObservableCollection<Contact> ContactList { get; } = new ObservableCollection<Contact>();

        // Loads all users in SQL table logins
        private void LoadContacts() {
            // Conncetion string for SQL conncetion
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            // Establishes connection
            using (var con = new SqlConnection(connectionString)) {
                var dt = new DataTable();
                try {
                    con.Open();
                }
                // If no connection can be established show error
                catch (Exception) {
                    MessageBox.Show("Connection failed");
                }
                // SQL Command using connection named con
                var command = new SqlCommand("select * from logins", con);

                // Generates datareader from SQL Command named command
                var dr = command.ExecuteReader();
                dt.Load(dr);

                // Creates a contact for each row in SQL table logins
                foreach (DataRow row in dt.Rows) {
                    // If the collumn status says Offline, change EllipseColour with grey hex value. Otherwise use green hex value
                    EllipseColour = row.Field<string>("status") == "Offline" ? "#424242" : "#FF00FF00";

                    // Extracts from each collumn name and places it in the corresponding variable
                    ContactList.Add(new Contact {
                        Name = row.Field<string>("usernames"),
                        Title = row.Field<string>("title"),
                        Status = EllipseColour
                    });
                }
            }
        }

        // Finds local IP of client computer
        public static string GetLocalIpAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }

        // Opens settings window
        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        // Changes colour of divider contacts and chatbox
        private void MetroWindow_Activated(object sender, EventArgs e) {
            Divider.Background = DivBrush;
        }

        // Changes colour of divider contacts and chatbox
        private void MetroWindow_Deactivated(object sender, EventArgs e) {
            Divider.Background = (Brush) new BrushConverter().ConvertFrom("#808080");
        }

        // Moves written message from ChatInput to ChatText
        private void ChatInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter && ChatInput.Text != "") {
                InsertIntoChat(ChatInput.Text);
                ChatInput.Text = null;
            }
        }

        // Inserts string and scrolls to end
        private void InsertIntoChat(string str) {
            ChatText.AppendText(str + "\n");
            ChatText.ScrollToEnd();
        }
    }

    // Contact Class (used for contact creation)
    public class Contact {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}