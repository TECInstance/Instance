using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace Instance {
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow {
        public SettingsWindow() {
            InitializeComponent();
        }

        private void BlueRadio_Checked(object sender, RoutedEventArgs e) {
            var _app = Application.Current as App;
            _app.ChangeTheme(new Uri(@"pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml"));
        }
    }
}