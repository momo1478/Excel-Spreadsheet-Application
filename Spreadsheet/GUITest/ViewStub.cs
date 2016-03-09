using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;
using FileAnalyzer;
using SS;
using System.Windows.Forms;

namespace GUITest
{
    public class ViewStub : ISpreadsheetView
    {
        SpreadsheetPanel spreadsheetPanel1 = new SpreadsheetPanel();

        public string ContentsBox { get; private set; }

        public string NameBox { get; private set; }

        public string ValueBox { get; private set; }

        public bool CalledDoClose { get; private set; }

        public bool CalledOpenNew { get; private set; }

        public string Message
        {
            get;  set;
        }

        public string Title
        {
            get;
            set;
        }

        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action NewEvent;
        public event Action<string> SaveEvent;
        public event Action<string, string> SetContentsEvent;
        public event Func<string, object> UpdateContentsBoxEvent;
        public event Func<string, object> UpdateValueBoxEvent;

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void OpenNew(Spreadsheet sheet)
        {
            CalledOpenNew = true;

            Message = "Load success!";
        }

        public void FireSetContentsEvent(int col , int row , string value)
        {
            if (SetContentsEvent != null)
            {
                SetContentsEvent(spreadsheetPanel1.GetName(col, row), value);
            }
        }

        public void SetCellValue(int col, int row, string value)
        {
            spreadsheetPanel1.SetValue(col, row, value);
        }

        public string GetCellValue(int col, int row)
        {
            string val;
            spreadsheetPanel1.GetValue(col, row, out val);
            return val;
        }

        public void SelectCell(int col , int row)
        {
            spreadsheetPanel1.SetSelection(col, row);
        }

        public void GetSelected(out int col , out int row)
        {
            spreadsheetPanel1.GetSelection(out col, out row);
        }

        public void FireOpenEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireSaveEvent(string filename)
        {
            if (SaveEvent != null)
            {
                SaveEvent(filename);
            }
        }

        public void FireFileChosenEvent(string filename)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(filename);
            }
        }

        public void FireUpdateValueBoxEvent(string cellName, SpreadsheetPanel ss = null)
        {
            if (UpdateValueBoxEvent != null)
            {
                ValueBox = UpdateValueBoxEvent(cellName)?.ToString() ?? "";
            }
        }

        public void FireUpdateContentsBoxEvent(string cellName, SpreadsheetPanel ss = null)
        {
            if (UpdateContentsBoxEvent != null)
            {
                ContentsBox = UpdateContentsBoxEvent(cellName)?.ToString() ?? "";
            }
        }
    }
}
