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

        public LoginWindow() {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            if (AuthenticateLogin(UsernameText.Text, PasswordText.Password)) {
                LoginSuccess = true;
                MainWindow.InitializeConnection();
                Close();
            }
            else {
                //TODO FIND PRETTIER SOLUTION
                MessageBox.Show("Username and password does not seem to be valid");
            }
        }

        public bool AuthenticateLogin(string username, string password) {
            var hash = GetHash(password);
            const string connectionString = @"Data Source=80.198.77.171,1337; Initial Catalog=Instance; User Id = InstanceLogin; Password = password";

            using (var con = new SqlConnection(connectionString)) {
                var dt = new DataTable();
                con.Open();

                var command = new SqlCommand("select * from logins", con);
                var dr = command.ExecuteReader();
                dt.Load(dr);

                if (dt.Rows.Cast<DataRow>().Where(variable => variable.Field<string>("usernames") == username).Any(variable => variable.Field<string>("passwords") == hash)) {
                    MainWindow.username = UsernameText.Text.ToLower();
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
    }
}