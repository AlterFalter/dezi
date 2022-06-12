using dezi.Helper;
using Newtonsoft.Json;
using System.IO;

namespace dezi.Config
{
    public class EditorSettings
    {
        public ColorTheme CurrentColorTheme { get; set; }

        public KeyBindings CurrentKeyBindings { get; set; }

        public bool WrapLines { get; set; }

        public bool ShowLineNumbers { get; set; }

        private EditorSettings()
        {
        }

        public void LoadColorTheme()
        {
            // TODO
        }

        public void LoadKeyBindings()
        {
            // TODO
        }

        public static EditorSettings Load()
        {
            EditorSettings editorSettings;
            if (File.Exists(PathHelper.SettingsFilePath))
            {
                string serializedEditorSettings = File.ReadAllText(PathHelper.SettingsFilePath);
                editorSettings = JsonConvert.DeserializeObject<EditorSettings>(serializedEditorSettings);
            }
            else
            {
                editorSettings = Default();
            }

            editorSettings.LoadColorTheme();
            editorSettings.LoadKeyBindings();

            return editorSettings;
        }

        public static EditorSettings Default()
        {
            EditorSettings editorSettings = new EditorSettings();
            editorSettings.CurrentColorTheme = ColorTheme.Load("default");
            editorSettings.CurrentKeyBindings = new KeyBindings();
            editorSettings.WrapLines = true;
            editorSettings.ShowLineNumbers = true;
            // TODO: create settings file
            return editorSettings;
        }
    }
}
