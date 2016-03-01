using System;

namespace FileAnalyzer
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface ISpreadsheetView
    {
        event Action<string> FileChosenEvent;

        event Action<string> CountEvent;

        event Action CloseEvent;

        event Action NewEvent;

        string Title { set; }
        string Message { get; set; }
        void DoClose();
        void OpenNew();
    }
}
