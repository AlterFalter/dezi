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
    public class Editor : UiElement
    {
        private readonly KeyboardInputs keyboardInputs;
        private IList<Cursor> cursors;
        private bool hasUnsavedChanges;
        private int tabSize;

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

        public EditorBuffer Buffer { get; }

        public Editor(KeyboardInputs keyboardInputs, int coordinateX, int coordinateY, int width, int height, bool isInFocus, string filepath = "")
        {
            this.keyboardInputs = keyboardInputs;

            this.hasUnsavedChanges = false;

            this.tabSize = 4;

            UpdatePositionInUi(coordinateX, coordinateY, width, height);

            this.Filepath = filepath;
            IEnumerable<string> lines;
            if (this.Filepath.Length == 0)
            {
                lines = new string[1] { "" };
            }
            else
            {
                // TODO: watch out for encoding
                // TODO: watch out for EOL
                lines = File.ReadAllLines(filepath);
                // TODO: detect tab size and type (spaces or tab)
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
            string bar = this.Filepath;
            if (this.hasUnsavedChanges)
            {
                bar += "*";
            }
            bar += " | Markdown | UTF-8";
            if (bar.Length < this.Width)
            {
                bar = bar.PadRight(this.Width - bar.Length);
            }
            else if (bar.Length > this.Width)
            {
                bar = bar.Substring(0, this.Width);
            }
            return bar;
        }

        public void HandleInput(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.Save:
                    this.Save();
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
                case InputAction.SpawnMultiCursorAbove:
                    Cursor firstCursor = this.Cursors.OrderBy(c => c.Row).ThenBy(c => c.Column).First();
                    if (firstCursor.Row > 0)
                    {
                        int rowFromNewCursor = firstCursor.Row - 1;
                        this.cursors.Add(
                            new Cursor(
                                rowFromNewCursor,
                                Math.Min(firstCursor.Column, this.Buffer.GetLinesInFile()[rowFromNewCursor].Length)));
                    }
                    break;
                case InputAction.SpawnMultiCursorUnder:
                    Cursor lastCursor = this.Cursors.OrderByDescending(c => c.Row).ThenByDescending(c => c.Column).First();
                    if (lastCursor.Row < this.Buffer.GetLinesInFile().Count - 1)
                    {
                        int rowFromNewCursor = lastCursor.Row + 1;
                        this.cursors.Add(
                            new Cursor(
                                rowFromNewCursor,
                                Math.Min(lastCursor.Column, this.Buffer.GetLinesInFile()[rowFromNewCursor].Length)));
                    }
                    break;
                case InputAction.DeactivateMultiCursors:
                    this.cursors = new List<Cursor> { this.Cursors.First() };
                    break;
                case InputAction.Backspace:
                    this.hasUnsavedChanges = true;
                    this.Buffer.Backspace(this.Cursors);
                    break;
                case InputAction.Delete:
                    this.hasUnsavedChanges = true;
                    this.Buffer.Delete(this.Cursors);
                    break;
                case InputAction.AddNewLine:
                    this.hasUnsavedChanges = true;
                    this.Buffer.AddNewLine(this.Cursors);
                    break;
                case InputAction.Input:
                    this.hasUnsavedChanges = true;
                    this.Buffer.Input(keyboardInputs.LatestInput, this.Cursors);
                    break;
                case InputAction.Tab:
                    this.hasUnsavedChanges = true;
                    this.Buffer.AddTab(this.tabSize, this.Cursors);
                    break;
                case InputAction.Home:
                    foreach (Cursor cursor in this.Cursors)
                    {
                        cursor.UpdateColumnAbsolute(0, this.Buffer.GetLinesInFile());
                    }
                    break;
                case InputAction.End:
                    foreach (Cursor cursor in this.Cursors)
                    {
                        cursor.UpdateColumnAbsolute(-1, this.Buffer.GetLinesInFile());
                    }
                    break;
                case InputAction.PageUp:
                    foreach (Cursor cursor in this.Cursors)
                    {
                        cursor.UpdateRow(-this.Height, this.Buffer.GetLinesInFile());
                    }
                    break;
                case InputAction.PageDown:
                    foreach (Cursor cursor in this.Cursors)
                    {
                        cursor.UpdateRow(this.Height, this.Buffer.GetLinesInFile());
                    }
                    break;
                default:
                    break;
            }
        }

        public void Save()
        {
            File.WriteAllLines(this.Filepath, this.Buffer.GetLinesInFile());
            this.hasUnsavedChanges = false;
        }

        public void HandleCursorUpdate(object? sender, PropertyChangedEventArgs e)
        {
            RemoveDuplicateCursors();
        }

        public void RemoveDuplicateCursors()
        {
            this.cursors = Cursor.RemoveDuplicateCursors(this.Cursors);
        }

        public override void UpdateSubmoduleSizes(int widthDelta, int heightDelta)
        {
            if (this.Buffer != null)
            {
                if (widthDelta != 0)
                {
                    this.Buffer.OutputWidth = this.Width + widthDelta;
                }
                if (heightDelta != 0)
                {
                    this.Buffer.OutputHeight = this.Height + heightDelta;
                }
            }
        }
    }
}
