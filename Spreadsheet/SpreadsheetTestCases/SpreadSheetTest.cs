using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Linq;

namespace SpreadsheetTestCases
{
    [TestClass]
    public class SpreadSheetTestCases
    {
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.SetCellContents("a1", 5);
            sheet.SetCellContents("b1", 5);
            sheet.SetCellContents("a2", 5);
            sheet.SetCellContents("b2", 5);
            sheet.SetCellContents("a1", 5);

            sheet.GetNamesOfAllNonemptyCells().ToList();
        }
    }
}
