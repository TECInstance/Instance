using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace Instance {
    public partial class LoginWindow {
        public bool LoginSuccess;
        public bool UsernameRemembrance; // True = Remember username, false = don't remember username

        public LoginWindow() {
            // Finds path to %AppData% and looks for UsernameRemembrance.txt
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");

            // If file exists and file says true, set bool UsernameRemembrance to true
            if (File.Exists(file)) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var readfile = new StreamReader(rememberfileName)) {
                    UsernameRemembrance = readfile.ReadLine() == "true";
                }
            }

            InitializeComponent();

            // If UsernameRemembrance is true, read file RememberedUser.txt, check RememberUserCheck and write the username in Username.Text
            if (UsernameRemembrance) {
                RememberUserCheck.IsChecked = true;
                var userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var readfile = new StreamReader(userfileName)) {
                    UsernameText.Text = readfile.ReadLine();
                }
            }
            // Else uncheck RememberUserCheck and write "Username" in Username.Text
            else {
                RememberUserCheck.IsChecked = false;
                UsernameText.Text = "Username";
            }
        }

        // On LoginBtn click
        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            // Overrites UsernameRemembrance with True/False depending on if RememberUserCheck is checked or not
            if (RememberUserCheck.IsChecked == true) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var writefile = new StreamWriter(rememberfileName)) {
                    // If the file doesn't exist, create it
                    if (!File.Exists(rememberfileName)) {
                        File.Create(rememberfileName);
                    }

                    writefile.WriteLine("true");
                }

                // Writes the username into RememberedUser.txt if RememberUserCheck is checked
                var userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var writefile = new StreamWriter(userfileName)) {
                    // If the file doesn't exist, create it
                    if (!File.Exists(userfileName)) {
                        File.Create(userfileName);
                    }

                    writefile.WriteLine(UsernameText.Text);
                }
            }
            // If RememberUserCheck is unchecked it will write "false" in UserRemembrance.txt
            else if (RememberUserCheck.IsChecked == false) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var writefile = new StreamWriter(rememberfileName)) {
                    // If the file doesn't exist, create it
                    if (!File.Exists(rememberfileName)) {
                        File.Create(rememberfileName);
                    }

                    writefile.WriteLine("false");
                }
            }

            // If login is corrrect changes LoginSuccess to true and closes LoginWindow
            if (AuthenticateLogin(UsernameText.Text, PasswordText.Password)) {
                LoginSuccess = true;
                Close();
            }
            // If username or password is incorrect show error
            else {
                this.ShowMessageAsync(":(", "Username and password does not seem to be valid");
            }
        }

        // Checks login
        public bool AuthenticateLogin(string username, string password) {
            // Stores hash value of password in hash
            var hash = GetHash(password);

            // Connection string for SQL Connection
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            // Establishes SQL Connection
            using (var con = new SqlConnection(connectionString)) {
                // Creates datatable
                var dt = new DataTable();
                try {
                    con.Open();
                }
                    // If connection cannot be established, show error
                catch (Exception) {
                    MessageBox.Show("Connection failed");
                }

                // SQL statement
                var command = new SqlCommand("select * from logins", con);

                // Creates datareader from SQL command
                var dr = command.ExecuteReader();
                dt.Load(dr);

                // Returns true using LINQ expression
                if (dt.Rows.Cast<DataRow>().Where(variable => variable.Field<string>("usernames") == username).Any(variable => variable.Field<string>("passwords") == hash)) {
                    MainWindow.Username = UsernameText.Text.ToLower();
                    return true;
                }
                return false;
            }
        }

        // Hashes Password and returns hash value
        public static string GetHash(string inputString) {
            // Generates SHA1
            using (var sha1 = new SHA1Managed()) {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                var sb = new StringBuilder(hash.Length*2);

                foreach (var b in hash) {
                    sb.Append(b.ToString("X2"));
                }

                // Returns hash value
                return sb.ToString();
            }
        }

        // Selects all Text if username textbox is selected via TAB
        private void UsernameText_GotFocus(object sender, RoutedEventArgs e) {
            UsernameText.SelectAll();
        }

        // Selects all Text if password textbox is selected via TAB
        private void PasswordText_GotFocus(object sender, RoutedEventArgs e) {
            PasswordText.SelectAll();
        }
    }
}