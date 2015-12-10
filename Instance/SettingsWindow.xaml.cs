using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro;

namespace Instance {
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow {
        public static Brush DivBrush;

        public SettingsWindow() {
            InitializeComponent();
            
            
        }

        

        private void RedTile_Clicked(object sender, RoutedEventArgs e) {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Red"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void GreenTile_Clicked(object sender, RoutedEventArgs e) {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Green"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void BlueTile_Clicked(object sender, RoutedEventArgs e) {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void PurpleTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Purple"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void OrangeTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Orange"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void LimeTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Lime"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void EmeraldTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Emerald"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void TealTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Teal"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void CyanTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Cyan"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void CobaltTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Cobalt"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void IndigoTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Indigo"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void VioletTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Violet"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void PinkTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Pink"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void MagentaTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Magenta"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void CrimsonTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Crimson"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void AmberTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Amber"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void YellowTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Yellow"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void BrownTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Brown"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void OliveTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Olive"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }

        private void SteelTile_Clicked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Steel"), ThemeManager.GetAppTheme("BaseDark"));
            DivBrush = GlowBrush;
        }
    }
}