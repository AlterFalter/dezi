using System;

namespace dezi.Config
{
    public sealed class ColorTheme
    {
        public ConsoleColor ForegroundColor { get; private set; }

        public ConsoleColor BackgroundColor { get; private set; }

        public ConsoleColor CursorColor { get; set; }

        private ColorTheme()
        {
        }

        public static ColorTheme Load(string colorThemeName)
        {
            // TODO: select color theme
            ColorTheme defaultColorTheme = new ColorTheme();
            defaultColorTheme.ForegroundColor = ConsoleColor.Black;
            defaultColorTheme.BackgroundColor = ConsoleColor.White;
            defaultColorTheme.CursorColor = ConsoleColor.Blue;
            return defaultColorTheme;
        }
    }
}
