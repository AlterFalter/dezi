using dezi.Input;
using dezi.UiElements.Editors;
using dezi.UiElements.StackPanel;

namespace dezi.UiElements
{
    public class SaveDialog : HorizontalStackPanel
    {
        public Editor EditorToSaveFilePath { get; }

        public SaveDialog(KeyboardInputs keyboardInputs, Editor currentEditor)
        {
            this.EditorToSaveFilePath = currentEditor;
            this.IsInteractiveElement = true;
            string inputLabel = "File path: ";
            Text text = new Text(inputLabel, 1, inputLabel.Length);
            text.MaxHeight = 1;
            this.AddLeft(text);
            this.AddRight(new SingleLineTextBox(keyboardInputs));
        }

        public SaveDialog(int yCoordinate, int xCoordinate, KeyboardInputs keyboardInputs, Editor currentEditor)
        {
            this.EditorToSaveFilePath = currentEditor;
            this.IsInteractiveElement = true;
            string inputLabel = "File path: ";
            Text text = new Text(inputLabel, yCoordinate, xCoordinate, 1, inputLabel.Length);
            text.MaxHeight = 1;
            this.AddLeft(text);
            this.AddRight(new SingleLineTextBox(keyboardInputs));
        }
    }
}
