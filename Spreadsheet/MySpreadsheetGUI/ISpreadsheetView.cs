using System;
using SS;
using SSGui;

namespace FileAnalyzer
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface ISpreadsheetView
    {
        event Action<string> FileChosenEvent;

        event Action<string, string> SetContentsEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Func<string, object> UpdateContentsBoxEvent;

        event Func<string, object> UpdateValueBoxEvent;

        event Action<string> SaveEvent;

        string Title { set; get; }
        string Message { get; set; }
        void DoClose();
        void OpenNew();
        void OpenNew(Spreadsheet sheet);
        void SetCellValue(int col, int row, string value);
        //void updateNameBox(SpreadsheetPanel ss);
        //void updateValueBox(SpreadsheetPanel ss);
        //void updateContentsBox(SpreadsheetPanel ss);

    }

}
