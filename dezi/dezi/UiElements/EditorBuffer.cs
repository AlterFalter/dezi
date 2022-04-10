using dezi.Input;
using System.Collections.Generic;
using System.Linq;

namespace dezi.UiElements
{
    public class EditorBuffer
    {
        // TODO: add coordinates where
        private readonly IList<string> allLinesInFile;

        public int OutputWidth { get; set; }

        public int OutputHeight { get; set; }

        public int NumberOfLines => allLinesInFile.Count();

        public EditorBuffer(int outputWidth, int outputHeight, IEnumerable<string> lines)
        {
            this.OutputWidth = outputWidth;
            this.OutputHeight = outputHeight;
            this.allLinesInFile = lines.ToList();
        }

        public IList<string> Render()
        {
            // TODO: give cursors as parameter so buffer shows what's edited right now
            List<string> output = allLinesInFile
                //.Select(l => l.PadRight(Math.Max(this.OutputWidth - l.Length, 0)))
                .Select(l => l.PadRight(this.OutputWidth))
                .Select(l2 => l2.Substring(0, this.OutputWidth))
                .ToList();
            if (output.Count() < this.OutputHeight)
            {
                for (int i = 0; output.Count() < this.OutputHeight; i++)
                {
                    output.Add(string.Empty.PadRight(this.OutputWidth));
                }
            }

            if (output.Count() > this.OutputHeight)
            {
                // TODO
            }
            return output;
        }

        public int GetLengthOfLine(int lineIndex)
        {
            return this.allLinesInFile[lineIndex].Length;
        }

        public IList<string> GetLinesInFile()
        {
            return this.allLinesInFile;
        }

        public void Delete(IList<Cursor> cursors)
        {
            foreach (Cursor cursor in cursors)
            {
                if (cursor.Column < allLinesInFile[cursor.Row].Length)
                {
                    allLinesInFile[cursor.Row].Remove(cursor.Column, 1);
                }
                // check if it is last line in file or not
                else if (cursor.Row < this.allLinesInFile.Count - 1)
                {
                    // cursor is at end of line
                    allLinesInFile[cursor.Row] += allLinesInFile[cursor.Row + 1];
                    allLinesInFile.RemoveAt(cursor.Row + 1);
                    // update cursors
                    foreach (Cursor updatingCursor in cursors.Where(c => c.Row > cursor.Row))
                    {
                        updatingCursor.UpdateRow(-1, this.allLinesInFile);
                    }
                }
            }
        }

        public void Backspace(IList<Cursor> cursors)
        {
            foreach (Cursor cursor in cursors)
            {
                // TODO: start of line
                if (cursor.Column > 0)
                {
                    allLinesInFile[cursor.Row].Remove(cursor.Column - 1, 1);
                    cursor.UpdateColumn(-1, this.allLinesInFile);
                }
                else if (cursor.Row > 0)
                {
                    // cursor is at end of line
                    allLinesInFile[cursor.Row - 1] += allLinesInFile[cursor.Row];
                    allLinesInFile.RemoveAt(cursor.Row);
                    // update cursors
                    cursor.UpdateRow(-1, this.allLinesInFile);
                    foreach (Cursor updatingCursor in cursors.Where(c => c.Row > cursor.Row))
                    {
                        updatingCursor.UpdateRow(-1, this.allLinesInFile);
                    }
                }
            }
        }

        public void AddNewLine(IList<Cursor> cursors)
        {
            foreach (Cursor cursor in cursors)
            {
                string newLineContent = this.allLinesInFile[cursor.Row].Substring(cursor.Column);
                this.allLinesInFile[cursor.Row] = this.allLinesInFile[cursor.Row].Substring(0, cursor.Column);
                this.allLinesInFile.Insert(cursor.Row + 1, newLineContent);
                cursor.UpdateColumnAbsolute(0, this.allLinesInFile);
                foreach (Cursor updatingCursor in cursors.Where(c => c.Row >= cursor.Row))
                {
                    updatingCursor.UpdateRow(+1, this.allLinesInFile);
                }
            }
        }

        public void Input(string latestInput, IList<Cursor> cursors)
        {
            foreach (Cursor cursor in cursors)
            {
                this.allLinesInFile[cursor.Row] = this.allLinesInFile[cursor.Row].Insert(cursor.Column, latestInput);
                foreach (Cursor updatingCursor in cursors.Where(c => c.Row == cursor.Row && c.Column >= cursor.Column))
                {
                    updatingCursor.UpdateColumn(latestInput.Length, this.allLinesInFile);
                }
            }
        }
    }
}
