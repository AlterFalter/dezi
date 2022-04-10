using System.Drawing;

namespace dezi.Config
{
    public sealed class ColorTheme
    {
        public Color ForegroundColor { get; private set; }

        public Color BackgroundColor { get; private set; }

        private ColorTheme()
        {
        }

        public static ColorTheme Load(string colorThemeName)
        {
            ColorTheme defaultColorTheme = new ColorTheme();
            defaultColorTheme.ForegroundColor = Color.White;
            defaultColorTheme.BackgroundColor = Color.Black;
            return defaultColorTheme;
        }
    }
}
