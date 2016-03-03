using System;
using System.IO;
using System.Text.RegularExpressions;
using MySpreadsheetGUI;
using System.Diagnostics;
using Formulas;
using System.Collections.Generic;
using System.Linq;

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

            window.SetContentsEvent += Controller_SetContentsInModel;
            window.SetContentsEvent += Controller_SetValueInPanel;

            window.UpdateContentsBoxEvent += Controller_UpdateContentsBox;

            window.UpdateValueBoxEvent += Controller_UpdateValueBox;
        }

        



        /// <summary>
        /// Returns the contents of a given cell Name, if an exception occurs, return null.
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private object Controller_UpdateContentsBox(string cellName)
        {
            try
            {
                if (model.sheet.GetCellContents(cellName) is Formula)
                {
                    return "=" + model.sheet.GetCellContents(cellName);
                }
                else
                {
                    return model.sheet.GetCellContents(cellName);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the value of a given cell Name, if an exception occurs , return null;
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object Controller_UpdateValueBox(string cellName)
        {
            try
            {
                return model.sheet.GetCellValue(cellName);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Attempts to perform SetContentsOfCell
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="contents"></param>
        private void Controller_SetContentsInModel(string cellName, string contents)
        {
            try
            {
                int row, col;
                GetRowsAndCols(cellName, out col, out row);

               List<string> recalculateCells = model.sheet.SetContentsOfCell(cellName, contents).ToList();

                foreach (string name in recalculateCells)
                {
                    window.SetCellValue(col, row, model.sheet.GetCellValue(name).ToString());
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString() + " From Controller_SetContentsInModel");

                if (e is SS.CircularException)
                {
                    window.Message = "Your input created a circular dependency, Stop it... now.";
                }
            } 
        }

        /// <summary>
        /// Sets the value of a cell in the SpreadsheetPanel to it's value according to its model. If any exceptions occur
        /// sets value of cell in the SpreadsheetPanel to be a "Formula Error"
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="cellContents"></param>
        private void Controller_SetValueInPanel(string cellName, string cellContents)
        {
            int row, col;
            GetRowsAndCols(cellName, out col, out row);

            try
            {
                window.SetCellValue(col, row, model.sheet.GetCellValue(cellName).ToString());
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString() + " From Controller_SetValueInPanel");
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
