using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SS;
using System.Xml;
using System.Windows.Forms;

namespace FileAnalyzer
{
    public class Model
    {
        // The contents of the open file in the AnalysisWindow, or the
        // empty string if no file is open.
        internal Spreadsheet sheet;

        /// <summary>
        /// 
        /// </summary>
        internal ISpreadsheetView modelWindow;

        /// <summary>
        /// Constructs a Model with an empty contents
        /// </summary>
        public Model()
        {
            sheet = new Spreadsheet();
        }

        /// <summary>
        /// Constructs a Model with a view reference
        /// </summary>
        public Model(ISpreadsheetView iWindow)
        {
            sheet = new Spreadsheet();
            modelWindow = iWindow;
        }

        /// <summary>
        /// Makes the contents of the named file the new value of contents
        /// </summary>
        public void ReadFile(string filename)
        {
            if (sheet.Changed == true)
            {
                DialogResult saveResult = MessageBox.Show("Opening this file will cause you to lose data, continue?", "Save me!" , MessageBoxButtons.YesNo);

                if (saveResult == DialogResult.No)
                {
                    return;
                }
            }
            sheet = new Spreadsheet(new StreamReader(filename));

            foreach (var cell in sheet.GetNamesOfAllNonemptyCells())
            {
                int col, row;
                Controller.GetRowsAndCols(cell, out col, out row);
                modelWindow.SetCellValue(col, row, sheet.GetCellValue(cell).ToString());
            }

        }

        public void WriteFile(string foldername)
        {
            
            sheet.Save(new StreamWriter(foldername));
        }

        ///// <summary>
        ///// Returns the number of chars in contents.
        ///// </summary>
        //public int CountChars()
        //{
        //    return contents.Length;
        //}

        ///// <summary>
        ///// Returns the number of words in contents.
        ///// </summary>
        //public int CountWords()
        //{
        //    StringReader reader = new StringReader(contents);
        //    int count = 0;
        //    string line;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        count += Regex.Split(line, @"\s+").Length;
        //    }
        //    return count;
        //}

        ///// <summary>
        ///// Returns the number of lines in contents.
        ///// </summary>
        //public int CountLines()
        //{
        //    StringReader reader = new StringReader(contents);
        //    int count = 0;
        //    string line;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        count++;
        //    }
        //    return count;
        //}

        ///// <summary>
        ///// Counts the number of times substring occurs in contents.
        ///// </summary>
        //public int CountSubstrings(string substring)
        //{
        //    int count = 0;
        //    int index = -1;
        //    while (((index < contents.Length) && (index = contents.IndexOf(substring, index + 1)) >= 0))
        //    {
        //        count++;
        //    }
        //    return count;
        //}
    }
}
