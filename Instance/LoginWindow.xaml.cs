using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Instance {
    public partial class LoginWindow {
        public bool LoginSuccess;
        public bool UsernameRemembrance;

        public LoginWindow() {
            var _test = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
            if (File.Exists(_test)) {
                var _rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var _readfile = new StreamReader(_rememberfileName)) {
                    UsernameRemembrance = _readfile.ReadLine() == "true";
                }
            }

            InitializeComponent();

            if (UsernameRemembrance) {
                RememberUserCheck.IsChecked = true;
                var _userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var _readfile = new StreamReader(_userfileName)) {
                    UsernameText.Text = _readfile.ReadLine();
                }
            }
            else {
                RememberUserCheck.IsChecked = false;
                UsernameText.Text = "Username";
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            if (RememberUserCheck.IsChecked == true) {
                var _rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var _writefile = new StreamWriter(_rememberfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(_rememberfileName)) {
                        File.Create(_rememberfileName);
                    }

                    _writefile.WriteLine("true");
                }

                var _userfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (var _writefile = new StreamWriter(_userfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(_userfileName)) {
                        File.Create(_userfileName);
                    }

                    _writefile.WriteLine(UsernameText.Text);
                }
            }
            else if (RememberUserCheck.IsChecked == false) {
                var _rememberfileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (var _writefile = new StreamWriter(_rememberfileName)) {
                    //if the file doesn't exist, create it
                    if (!File.Exists(_rememberfileName)) {
                        File.Create(_rememberfileName);
                    }

                    _writefile.WriteLine("false");
                }
            }

            if (AuthenticateLogin(UsernameText.Text, PasswordText.Password)) {
                LoginSuccess = true;
                MainWindow.InitializeConnection();
                Close();
            }
            else {
                //FIND PRETTIER SOLUTION
                MessageBox.Show("Username and password does not seem to be valid");
            }
        }

        public bool AuthenticateLogin(string username, string password) {
            var _hash = GetHash(password);
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            using (var _con = new SqlConnection(connectionString)) {
                var _dt = new DataTable();
                _con.Open();

                var _command = new SqlCommand("select * from logins", _con);
                var _dr = _command.ExecuteReader();
                _dt.Load(_dr);
                
                if (_dt.Rows.Cast<DataRow>().Where(variable => variable.Field<string>("usernames") == username).Any(variable => variable.Field<string>("passwords") == _hash)) {
                    MainWindow.Username = UsernameText.Text.ToLower();
                    return true;
                }
                return false;
            }
        }

        public static string GetHash(string inputString) {
            using (var _sha1 = new SHA1Managed()) {
                var _hash = _sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                var _sb = new StringBuilder(_hash.Length*2);

                foreach (var _b in _hash) {
                    _sb.Append(_b.ToString("X2"));
                }

                return _sb.ToString();
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