using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Linq;
using Formulas;
using System.Collections.Generic;

namespace SpreadsheetTestCases
{
    [TestClass]
    public class SpreadSheetTestCases
    {
        [TestMethod]
        public void SCCDoubles()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a1", 5).ToList(), new List<string> { "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("b1", 10).ToList(), new List<string> { "B1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("b2", 2e5).ToList(), new List<string> { "B2" });
            CollectionAssert.AreEqual(sheet.SetCellContents("b2", 5.5e5).ToList(), new List<string> { "B2" });

            Assert.AreEqual((double)sheet.GetCellContents("a1"), 5);
            Assert.AreEqual((double)sheet.GetCellContents("B1"), 10);
            Assert.AreEqual((double)sheet.GetCellContents("B2"), 5.5e5);
            Assert.AreEqual((string)sheet.GetCellContents("a5"), default(string));
        }

        [TestMethod]
        public void SCCText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            CollectionAssert.AreEqual(sheet.SetCellContents("a12", "hello").ToList(), new List<string> { "A12" });
            CollectionAssert.AreEqual(sheet.SetCellContents("Black71", "BrOThaPls").ToList(), new List<string> { "BLACK71" });
            CollectionAssert.AreEqual(sheet.SetCellContents("AzUlE1472", "").ToList(), new List<string> { "AZULE1472" });

            Assert.AreEqual((string)sheet.GetCellContents("a12"), "hello");
            Assert.AreEqual((string)sheet.GetCellContents("Black71"), "BrOThaPls");
            Assert.AreEqual((string)sheet.GetCellContents("AzulE1472"), "");
            try { Assert.AreEqual((string)sheet.GetCellContents("))))"), default(string)); }
            catch (InvalidNameException) { }
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GCCNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents(null); 
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void InvalidCellName1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("1a");
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void InvalidCellName2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("B");
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void InvalidCellName3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("c0");
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void InvalidCellName4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("B4123C");
        }

        
    }
}
