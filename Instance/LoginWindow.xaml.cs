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
        public bool UsernameRemembrance;

        public LoginWindow() {
            var test = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
            if (File.Exists(test)) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var readfile = new StreamReader(rememberfileName)) {
                    UsernameRemembrance = readfile.ReadLine() == "true";
                }
            }

            InitializeComponent();

            if (UsernameRemembrance) {
                RememberUserCheck.IsChecked = true;
                var userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var readfile = new StreamReader(userfileName)) {
                    UsernameText.Text = readfile.ReadLine();
                }
            }
            else {
                RememberUserCheck.IsChecked = false;
                UsernameText.Text = "Username";
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            if (RememberUserCheck.IsChecked == true) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var writefile = new StreamWriter(rememberfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(rememberfileName)) {
                        File.Create(rememberfileName);
                    }

                    writefile.WriteLine("true");
                }

                var userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var writefile = new StreamWriter(userfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(userfileName)) {
                        File.Create(userfileName);
                    }

                    writefile.WriteLine(UsernameText.Text);
                }
            }
            else if (RememberUserCheck.IsChecked == false) {
                var rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var writefile = new StreamWriter(rememberfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(rememberfileName)) {
                        File.Create(rememberfileName);
                    }

                    writefile.WriteLine("false");
                }
            }

            if (AuthenticateLogin(UsernameText.Text, PasswordText.Password)) {
                LoginSuccess = true;
                Close();
            }
            else {
                this.ShowMessageAsync(":(", "Username and password does not seem to be valid");
            }
        }

        public bool AuthenticateLogin(string username, string password) {
            var hash = GetHash(password);
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

                if (dt.Rows.Cast<DataRow>().Where(variable => variable.Field<string>("usernames") == username).Any(variable => variable.Field<string>("passwords") == hash)) {
                    MainWindow.Username = UsernameText.Text.ToLower();
                    return true;
                }
                return false;
            }
        }

        public static string GetHash(string inputString) {
            using (var sha1 = new SHA1Managed()) {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                var sb = new StringBuilder(hash.Length*2);

                foreach (var b in hash) {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private void UsernameText_GotFocus(object sender, RoutedEventArgs e) {
            UsernameText.SelectAll();
        }

        private void PasswordText_GotFocus(object sender, RoutedEventArgs e) {
            PasswordText.SelectAll();
        }
    }
}