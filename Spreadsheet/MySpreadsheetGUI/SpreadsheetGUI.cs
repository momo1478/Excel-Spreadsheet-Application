﻿using System;
using System.Windows.Forms;
using SSGui;
using FileAnalyzer;
using System.Diagnostics;

namespace SSGui
{
    /// <summary>
    /// Example of using a SpreadsheetPanel object
    /// </summary>
    public partial class SpreadsheetGUI : Form , ISpreadsheetView
    {
        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        protected SpreadsheetPanel SPanel
        {
            get { return spreadsheetPanel1; }    
        }

        /// <summary>
        /// Constructor for the demo
        /// </summary>
        public SpreadsheetGUI()
        {
            InitializeComponent();

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SelectionChanged += updateNameBox;
            spreadsheetPanel1.SelectionChanged += updateContentsBox;
            spreadsheetPanel1.SelectionChanged += updateValueBox;
            spreadsheetPanel1.SetSelection(2, 3);
        }

        

        public event Action<string> FileChosenEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<string, string> SetContentsEvent;
        public event Func<string, object> UpdateContentsBoxEvent;
        public event Func<string, object> UpdateValueBoxEvent;

        /// <summary>
        /// Every time the selection changes, this method is called with the
        /// Spreadsheet as its parameter.  We display the current time in the cell.
        /// </summary>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            if (value == "") 
            {
                //ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));
                //ss.GetValue(col, row, out value);
                //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
            }
        }

        /// <summary>
        /// Updates the text of cellNameBox
        /// </summary>
        /// <param name="sender"></param>
        private void updateNameBox(SpreadsheetPanel ss)
        {
            int selectedRow, selectedCol;
            ss.GetSelection(out selectedCol, out selectedRow);

            this.cellNameBox.Text = ss.GetName(selectedCol , selectedRow);
        }

        /// <summary>
        /// Updates the text of cellValueBox
        /// </summary>
        /// <param name="sender"></param>
        private void updateValueBox(SpreadsheetPanel ss)
        {
            int selectedRow, selectedCol;
            ss.GetSelection(out selectedCol, out selectedRow);

            string value;
            ss.GetValue(selectedCol, selectedRow, out value);

            this.cellValueBox.Text = UpdateValueBoxEvent(ss.GetName(selectedCol, selectedRow))?.ToString() ?? "";
        }

        /// <summary>
        /// Updates text of CellContentsBox
        /// </summary>
        /// <param name="ss"></param>
        private void updateContentsBox(SpreadsheetPanel ss)
        {
            int selectedRow, selectedCol;
            ss.GetSelection(out selectedCol, out selectedRow);

            string value;
            ss.GetValue(selectedCol, selectedRow , out value);

            this.cellContentsBox.Text = UpdateContentsBoxEvent(ss.GetName(selectedCol, selectedRow))?.ToString() ?? "";
            
        }



        /// <summary>
        /// Handles the Click event of the openItem control.
        /// </summary>
        private void OpenItem_Click(object sender, EventArgs e)
        {
            //DialogResult result = fileDialog.ShowDialog();
            //if (result == DialogResult.Yes || result == DialogResult.OK)
            //{
            //    if (FileChosenEvent != null)
            //    {
            //        FileChosenEvent(fileDialog.FileName);
            //    }
            //}
        }

        /// <summary>
        /// Handles the Click event of the closeItem control.
        /// </summary>
        private void CloseItem_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Deals with the Close menu
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            FileAnalysisApplicationContext.GetContext().RunNew();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(NewEvent != null)
                NewEvent();
        }

        private void SetCellValue(int col , int row , string value)
        {
            spreadsheetPanel1.SetValue(col, row, value);
        }

        private void cellContentsBox_TextChanged(object sender, EventArgs e)
        {
            int rowArg;
            int colArg;

            if (SetContentsEvent != null)
            {
                spreadsheetPanel1.GetSelection(out colArg, out rowArg);
                spreadsheetPanel1.SetSelection(colArg, rowArg);

                this.SetContentsEvent(spreadsheetPanel1.GetName(colArg, rowArg), this.cellContentsBox.Text);
            }
        
        }

        void ISpreadsheetView.SetCellValue(int col, int row, string value)
        {
            SetCellValue(col, row, value);
        }
    }
}
