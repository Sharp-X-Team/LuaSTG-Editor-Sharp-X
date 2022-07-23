using System;
using System.Windows;

namespace REghZyFramework.Themes
{
    public static class ThemesController
    {
        public enum ThemeTypes
        {
            Light, ColourfulLight,
            Dark, ColourfulDark
        }

        public static ThemeTypes CurrentTheme { get; set; }

        public static ResourceDictionary ThemeDictionary;

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(string themeInput)
        {
            //ThemeDictionary = new ResourceDictionary() { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative)};
            //MessageBox.Show($"Hello, the new theme dictionary is {ThemeDictionary}");
            ThemeTypes theme = (ThemeTypes)Enum.Parse(typeof(ThemeTypes), themeInput);
            string themeName;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.Dark: themeName = "DarkTheme"; break;
                case ThemeTypes.Light: themeName = "LightTheme"; break;
                case ThemeTypes.ColourfulDark: themeName = "ColourfulDarkTheme"; break;
                case ThemeTypes.ColourfulLight: themeName = "ColourfulLightTheme"; break;
                default: themeName = "LightTheme"; break;
            }

            ChangeTheme(new Uri($"../Themes/{themeName}.xaml", UriKind.Relative));
        }
    }
}
