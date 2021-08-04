using System;
using System.Windows;

namespace AlphaClicker
{
    public static class ThemesController
    {
        public enum ThemeTypes
        {
            Light,
            Dark,
        }
        
        public static ThemeTypes CurrentTheme { get; set; }


        private static void ChangeTheme(Uri uri)
        {
            Application.Current.Resources.MergedDictionaries[0].Source = uri;
            Application.Current.Resources.MergedDictionaries[1].Source = new Uri("Templates/Templates.xaml", UriKind.RelativeOrAbsolute);

        }


        public static void SetTheme(ThemeTypes theme)
        {
            string themeName = null;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.Dark: themeName = "DarkTheme"; break;
                case ThemeTypes.Light: themeName = "LightTheme"; break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"Themes/{themeName}.xaml", UriKind.Relative));
            }
            catch { }
        }
    }
}
