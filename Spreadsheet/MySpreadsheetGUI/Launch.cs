using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySpreadsheetGUI;
using FileAnalyzer;

namespace MySpreadsheetGUI
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var context = FileAnalysisApplicationContext.GetContext();
            FileAnalysisApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
