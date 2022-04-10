namespace dezi.Config
{
    public class EditorSettings
    {
        public ColorTheme CurrentColorTheme { get; set; }

        public KeyBindings CurrentKeyBindings { get; set; }

        public bool WrapLines { get; set; }

        public bool ShowLineNumbers { get; set; }

        public EditorSettings()
        {
            Load();
        }

        public void Load()
        {
            // TODO
            this.CurrentColorTheme = ColorTheme.Load("default");
            this.CurrentKeyBindings = new KeyBindings();
            this.WrapLines = true;
            this.ShowLineNumbers = true;
        }
    }
}
