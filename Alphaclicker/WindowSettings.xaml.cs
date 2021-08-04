using System.Windows;
using System.Windows.Input;

namespace AlphaClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WindowSettings: Window
    {
        public WindowSettings()
        {
            InitializeComponent();
        }

        private void SetSettings()
        {
            AlphaRegistry.SetWindowSettings(((MainWindow)this.Owner).Topmost);
        }

        private void windowSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void windowSettings_Loaded(object sender, RoutedEventArgs e)
        {
            darkThemeSwitch.IsChecked = ThemesController.CurrentTheme == ThemesController.ThemeTypes.Dark;
            topMostSwitch.IsChecked = ((MainWindow)this.Owner).Topmost;
        }

        private void windowSettings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).keyEnabled = true;
        }

        private void darkThemeSwitch_Checked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
            SetSettings();
        }
        private void darkThemeSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
            SetSettings();
        }

        private void topMostSwitch_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.Owner).Topmost = true;
            SetSettings();
        }

        private void topMostSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.Owner).Topmost = false;
            SetSettings();
        }
    }
}
