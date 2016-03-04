using System;

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

        string Title { set; }
        string Message { get; set; }
        void DoClose();
        void OpenNew();
        void SetCellValue(int col, int row, string value);
    }

}
