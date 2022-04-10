using dezi.Helper;
using dezi.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace dezi.UiElements
{
    /// <summary>
    /// Editor is the basic editor functionality which loads the content
    /// and creates the specific UI for editing the file
    /// but nothing around it, such as menu.
    /// </summary>
    public class Editor
    {
        private readonly KeyboardInputs keyboardInputs;
        private IList<Cursor> cursors;
        private int width;
        private int height;

        public IList<Cursor> Cursors
        {
            get
            {
                return cursors
                    .OrderBy(c => c.Row)
                    .ThenBy(c => c.Column)
                    .ToList();
            }
        }

        public string Filepath { get; set; }

        public bool IsInFocus { get; set; }

        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
                if (this.Buffer != null)
                {
                    this.Buffer.OutputWidth = this.width - 1;
                }
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                if (this.Buffer != null)
                {
                    this.Buffer.OutputHeight = this.height - 1;
                }
            }
        }

        public EditorBuffer Buffer { get; }

        public Editor(KeyboardInputs keyboardInputs, int coordinateX, int coordinateY, int width, int height, bool isInFocus, string filepath = "")
        {
            this.keyboardInputs = keyboardInputs;

            UpdatePositionInUi(coordinateX, coordinateY, width, height);

            this.Filepath = filepath;
            IEnumerable<string> lines;
            if (this.Filepath.Length == 0)
            {
                lines = new string[1] { "" };
            }
            else
            {
                // TODO: watch out for encoding, EOL, etc.
                lines = File.ReadAllLines(filepath);
            }
            this.Buffer = new EditorBuffer(this.Width, this.Height - 1, lines);

            this.cursors = new List<Cursor>();
            Cursor cursor = new Cursor(0, 0);
            cursor.PropertyChanged += HandleCursorUpdate;
            this.cursors.Add(cursor);

            this.IsInFocus = isInFocus;
        }

        public void UpdatePositionInUi(int coordinateX, int coordinateY, int width, int height)
        {
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;
            this.Width = width;
            this.Height = height;
        }

        public void Render(IList<string> uiOutput)
        {
            IList<string> lines = this.Buffer.Render();
            for (int i = 0; i < lines.Count; i++)
            {
                // TODO: fix this
                uiOutput[this.CoordinateY + i] = StringHelper.PutInNewStringAtIndex(uiOutput[this.CoordinateY + i], this.CoordinateX, lines[i]);
            }
            // editor bar
            uiOutput[this.CoordinateY + lines.Count] = StringHelper.PutInNewStringAtIndex(uiOutput[this.CoordinateY + lines.Count], this.CoordinateX, GetEditorBar());
        }

        private string GetEditorBar()
        {
            // TODO: make content responsive
            string bar = $"{this.Filepath} | Markdown | UTF-8";
            if (bar.Length < this.width)
            {
                bar = bar.PadRight(this.Width - bar.Length);
            }
            else if (bar.Length > this.width)
            {
                bar = bar.Substring(0, this.width);
            }
            return bar;
        }

        public void HandleInput()
        {
            if (this.IsInFocus)
            {
                switch (this.keyboardInputs.GetInputActionsFromKeyboard())
                {
                    case InputAction.QuitProgram:
                        Environment.Exit(0);
                        break;
                    case InputAction.MoveCursorUp:
                        foreach (Cursor cursor in this.Cursors)
                        {
                            cursor.UpdateRow(-1, this.Buffer.GetLinesInFile());
                        }
                        break;
                    case InputAction.MoveCursorDown:
                        foreach (Cursor cursor in this.Cursors)
                        {
                            cursor.UpdateRow(+1, this.Buffer.GetLinesInFile());
                        }
                        break;
                    case InputAction.MoveCursorLeft:
                        foreach (Cursor cursor in this.Cursors)
                        {
                            cursor.UpdateColumn(-1, this.Buffer.GetLinesInFile());
                        }
                        break;
                    case InputAction.MoveCursorRight:
                        foreach (Cursor cursor in this.Cursors)
                        {
                            cursor.UpdateColumn(+1, this.Buffer.GetLinesInFile());
                        }
                        break;
                    case InputAction.Backspace:
                        this.Buffer.Backspace(this.Cursors);
                        break;
                    case InputAction.Delete:
                        this.Buffer.Delete(this.Cursors);
                        break;
                    case InputAction.AddNewLine:
                        this.Buffer.AddNewLine(this.Cursors);
                        break;
                    case InputAction.Input:
                        this.Buffer.Input(keyboardInputs.LatestInput, this.Cursors);
                        break;
                    default:
                        break;
                }
            }
        }

        public void HandleCursorUpdate(object? sender, PropertyChangedEventArgs e)
        {
            RemoveDuplicateCursors();
        }

        public void RemoveDuplicateCursors()
        {
            this.cursors = Cursor.RemoveDuplicateCursors(this.Cursors);
        }
    }
}
