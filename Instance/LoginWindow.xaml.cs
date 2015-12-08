using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Instance {

    public partial class LoginWindow {
        public bool LoginSuccess;
        public bool UsernameRemembrance;

        public LoginWindow()
        {

            var test = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
            if (File.Exists(test))
            {
                var rememberfileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (System.IO.StreamReader readfile = new System.IO.StreamReader(rememberfileName))
                {
                    if (readfile.ReadLine() == "true")
                    {
                        UsernameRemembrance = true;
                    }
                    else
                    {
                        UsernameRemembrance = false;
                    }
                }
            }

            InitializeComponent();

            if (UsernameRemembrance == true)
            {
                RememberUserCheck.IsChecked = true;
                var userfileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (System.IO.StreamReader readfile = new System.IO.StreamReader(userfileName))
                {
                    UsernameText.Text = readfile.ReadLine();
                }
            }
            else
            {
                RememberUserCheck.IsChecked = false;
                UsernameText.Text = "Username";
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            if (RememberUserCheck.IsChecked == true)
            {
                var rememberfileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (System.IO.StreamWriter writefile = new System.IO.StreamWriter(rememberfileName))
                {
                    //if the file doesn't exist, create it
                    if (!File.Exists(rememberfileName))
                        File.Create(rememberfileName);

                    writefile.WriteLine("true");
                }

                var userfileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RememberedUser.txt");
                using (System.IO.StreamWriter writefile = new System.IO.StreamWriter(userfileName))
                {
                    //if the file doesn't exist, create it
                    if (!File.Exists(userfileName))
                        File.Create(userfileName);

                    writefile.WriteLine(UsernameText.Text);
                }
            }
            else if (RememberUserCheck.IsChecked == false)
            {
                var rememberfileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UsernameRemembrance.txt");
                using (System.IO.StreamWriter writefile = new System.IO.StreamWriter(rememberfileName))
                {
                    //if the file doesn't exist, create it
                    if (!File.Exists(rememberfileName))
                        File.Create(rememberfileName);

                    writefile.WriteLine("false");
                }
            }

            if (AuthenticateLogin(UsernameText.Text, PasswordText.Password))
            {
                LoginSuccess = true;
                MainWindow.InitializeConnection();
                Close();
            }
            else
            {
                //TODO FIND PRETTIER SOLUTION
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

        private void UsernameText_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernameText.SelectAll();
        }

        private void PasswordText_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordText.SelectAll();
        }
    }
}