using dezi.Config;
using System;

namespace dezi.Input
{
    public class KeyboardInputs
    {
        public string LatestInput { get; private set; }

        public KeyBindings KeyBindings { get; set; }

        public KeyboardInputs(KeyBindings keyBindings)
        {
            this.KeyBindings = keyBindings;
        }

        public InputAction GetInputActionsFromKeyboard()
        {
            ConsoleKeyInfo character = Console.ReadKey();
            // TODO: use key bindings from settings and not fix keys
            if ((ConsoleModifiers.Control & character.Modifiers) != 0 && character.Key == ConsoleKey.Q)
            {
                return InputAction.QuitProgram;
            }
            else if ((ConsoleModifiers.Control & character.Modifiers) != 0 && character.Key == ConsoleKey.W)
            {
                return InputAction.QuitUiElement;
            }
            else if ((ConsoleModifiers.Control & character.Modifiers) != 0 && character.Key == ConsoleKey.S)
            {
                return InputAction.Save;
            }
            else if (character.Key == ConsoleKey.UpArrow)
            {
                return InputAction.MoveCursorUp;
            }
            else if (character.Key == ConsoleKey.DownArrow)
            {
                return InputAction.MoveCursorDown;
            }
            else if (character.Key == ConsoleKey.LeftArrow)
            {
                return InputAction.MoveCursorLeft;
            }
            else if (character.Key == ConsoleKey.RightArrow)
            {
                return InputAction.MoveCursorRight;
            }
            else if (character.Key == ConsoleKey.Backspace)
            {
                return InputAction.Backspace;
            }
            else if (character.Key == ConsoleKey.Delete)
            {
                return InputAction.Delete;
            }
            else if (character.Key == ConsoleKey.Enter)
            {
                return InputAction.AddNewLine;
            }
            else if (character.Key == ConsoleKey.Home)
            {
                return InputAction.Home;
            }
            else if (character.Key == ConsoleKey.End)
            {
                return InputAction.End;
            }
            else if (character.Key == ConsoleKey.PageUp)
            {
                return InputAction.PageUp;
            }
            else if (character.Key == ConsoleKey.PageDown)
            {
                return InputAction.PageDown;
            }
            else if (character.Key == ConsoleKey.Tab)
            {
                return InputAction.Tab;
            }
            else
            {
                this.LatestInput = character.KeyChar.ToString();
                return InputAction.Input;
            }
        }
    }
}
