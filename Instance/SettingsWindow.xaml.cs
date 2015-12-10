using System;
using System.Windows;
using System.Windows.Media;

namespace Instance {
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow {
        public SettingsWindow() {
            InitializeComponent();
        }

        private void RedRadio_Checked(object sender, RoutedEventArgs e) {
            Application.Current.Resources.MergedDictionaries.Clear();
            MainWindow.DividerBrush = (Brush) new BrushConverter().ConvertFrom("BE1707");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/Red.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/BaseDark.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        private void BlueRadio_Checked(object sender, RoutedEventArgs e) {
            Application.Current.Resources.MergedDictionaries.Clear();
            MainWindow.DividerBrush = (Brush)new BrushConverter().ConvertFrom("BE1707");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/Blue.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/BaseDark.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        private void GreenRadio_Checked(object sender, RoutedEventArgs e) {
            Application.Current.Resources.MergedDictionaries.Clear();
            MainWindow.DividerBrush = (Brush)new BrushConverter().ConvertFrom("BE1707");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/Green.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("/MahApps.Metro;component/Styles/Accents/BaseDark.xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}