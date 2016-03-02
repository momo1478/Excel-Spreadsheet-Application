using System;
using System.IO;
using System.Text.RegularExpressions;
using MySpreadsheetGUI;

namespace FileAnalyzer
{
    /// <summary>
    /// Controls the operation of an IAnalysisView.
    /// </summary>
    public class Controller
    {
        // The window being controlled
        private ISpreadsheetView window;

        // The model being used
        private Model model;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            this.model = new Model();
            window.FileChosenEvent += HandleFileChosen;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.SetContentsEvent += Controller_SetContents;
        }

        private void Controller_SetContents(string cellName, string contents)
        {
            try
            {
                int row, col;
                GetRowsAndCols(cellName, out col, out row);

                model.sheet.SetContentsOfCell(cellName, contents);
                window.SetCellValue(col, row, model.sheet.GetCellValue(cellName).ToString());
            }
            catch
            {
                int row, col;
                GetRowsAndCols(cellName ,out col, out row);

                window.SetCellValue(col, row, "Formula Error");
            } 
        }

        private static void GetRowsAndCols(string cellName, out int col , out int row)
        {
            col = char.ToUpper(cellName[0]) - 65;
            row = int.Parse(cellName.Substring(1)) - 1;
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            try
            {
                model.ReadFile(filename);
                window.Title = filename;
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            FileAnalysisApplicationContext.GetContext().RunNew();
        }
    }
}
