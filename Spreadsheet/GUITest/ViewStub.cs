using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;
using FileAnalyzer;

namespace GUITest
{
    public class ViewStub : ISpreadsheetView
    {
        public string Message
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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
            throw new NotImplementedException();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void SetCellValue(int col, int row, string value)
        {
            throw new NotImplementedException();
        }
    }
}
