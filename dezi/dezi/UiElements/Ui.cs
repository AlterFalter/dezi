using dezi.Config;
using dezi.Input;
using dezi.UiElements.Editors;
using dezi.UiElements.StackPanel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dezi.UiElements
{
    /// <summary>
    /// Creates the UI or the sub-elements in it
    /// </summary>
    public class Ui
    {
        private bool isFirstRender;

        public static int TerminalWidth => Console.WindowWidth;

        public static int TerminalHeight => Console.WindowHeight;

        private IList<string> uiOutput;

        public IList<string> UiOutput
        {
            get
            {
                // change array size to terminal size
                if (this.uiOutput.Count > TerminalHeight)
                {
                    this.uiOutput = this.uiOutput.Take(TerminalHeight).ToArray();
                }
                else if (this.uiOutput.Count < TerminalHeight)
                {
                    do
                    {
                        string emptyLine = string.Empty.PadRight(TerminalWidth);
                        this.uiOutput.Add(emptyLine);
                    }
                    while (this.uiOutput.Count < TerminalHeight);
                }

                // change string length to new length
                if (this.uiOutput.First().Length > TerminalWidth)
                {
                    this.uiOutput = this.uiOutput
                        .Select(l => l.Substring(0, TerminalWidth))
                        .ToArray();
                }
                else if (this.uiOutput.First().Length < TerminalWidth)
                {
                    int lengthDifference = TerminalWidth - this.uiOutput.First().Length;
                    this.uiOutput = this.uiOutput
                        .Select(l => l.PadRight(TerminalWidth - lengthDifference))
                        .ToArray();
                }
                return this.uiOutput;
            }
        }

        public IList<Editor> Editors { get; set; }

        public KeyboardInputs KeyboardInputs { get; }

        public EditorSettings EditorSettings { get; set; }

        public VerticalStackPanel BaseVerticalStackPanel { get; set; }

        public Ui(IList<string> filePaths)
        {
            isFirstRender = true;

            this.EditorSettings = EditorSettings.Load();
            this.KeyboardInputs = this.EditorSettings.CurrentKeyBindings.KeyboardInputs;

            this.uiOutput = new List<string>();
            for (int i = 0; i < TerminalHeight; i++)
            {
                this.uiOutput.Add(string.Empty.PadRight(TerminalWidth));
            }

            if (filePaths.Count == 0)
            {
                // open empty editor when no file path was given
                filePaths = new string[1] { "" };
            }

            this.Editors = new List<Editor>
            {
                new Editor(this.KeyboardInputs, 0, 0, TerminalWidth, TerminalHeight, true, filePaths[0])
            };

            this.BaseVerticalStackPanel = new VerticalStackPanel();
            this.BaseVerticalStackPanel.AddBottom(this.Editors.First());

            Console.Title = "Dezi";
            Console.CursorVisible = false;
            // TODO: replace with multi-platform code
            Console.SetWindowSize(TerminalWidth, TerminalHeight);
            Console.SetBufferSize(TerminalWidth, TerminalHeight);
        }

        public void Run()
        {
            while (true)
            {
                // TODO: remove this and insert it at the correct position
                this.Editors.Single(e => e.IsInFocus).RemoveDuplicateCursors();
                Render();
                HandleInput();
            }
        }

        private void Render()
        {
            // TODO: Rewrite to render from the stack panel onwards instead of editor

            Console.BackgroundColor = this.EditorSettings.CurrentColorTheme.BackgroundColor;
            Console.ForegroundColor = this.EditorSettings.CurrentColorTheme.ForegroundColor;

            // TODO: replace copy-method with non-deprecated method
            IList<string> oldUiOutput = this.uiOutput.Select(l => string.Copy(l)).ToList();

            foreach (Editor editor in this.Editors)
            {
                editor.Height = TerminalHeight;
                editor.Width = TerminalWidth;
                editor.Render(this.UiOutput);
            }

            // output to terminal
            // only update the lines that changed
            // updating complete line is faster than checking every char.
            for (int rowIndex = 0; rowIndex < this.UiOutput.Count; rowIndex++)
            {
                if (this.UiOutput[rowIndex] != oldUiOutput[rowIndex] || this.isFirstRender)
                {
                    SetTerminalCursorPosition(rowIndex, 0);
                    Console.Write(this.UiOutput[rowIndex]);
                }
            }

            foreach (Cursor cursor in this.Editors.Single(e => e.IsInFocus).Cursors)
            {
                SetTerminalCursorPosition(cursor.Row, cursor.Column);
                Console.BackgroundColor = this.EditorSettings.CurrentColorTheme.CursorColor;
                Console.Write(this.uiOutput[cursor.Row][cursor.Column]);
                Console.ResetColor();
            }
        }

        private void SetTerminalCursorPosition(int row, int column)
        {
            Console.CursorTop = row;
            Console.CursorLeft = column;
        }

        private void HandleInput()
        {
            InputAction inputAction = this.KeyboardInputs.GetInputActionsFromKeyboard();
            switch (inputAction)
            {
                case InputAction.QuitProgram:
                    QuitProgram();
                    break;
                case InputAction.QuitUiElement:
                    this.Editors.Remove(this.Editors.Single(e => e.IsInFocus));
                    if (this.Editors.Count == 0)
                    {
                        QuitProgram();
                    }
                    else
                    {
                        this.Editors.First().IsInFocus = true;
                    }
                    break;
                default:
                    this.Editors.Single(e => e.IsInFocus).HandleInput(inputAction);
                    break;
            }
        }

        private void QuitProgram()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
