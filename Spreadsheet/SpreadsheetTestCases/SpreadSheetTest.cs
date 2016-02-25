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
            Assert.AreEqual((string)sheet.GetCellContents("a5"), "");
        }
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SCCDoublesNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents(null, 2e5).ToList(), new List<string> { "B2" });
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SCCDoublesInvalid()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("10Azaz", 2e5).ToList(), new List<string> { "B1" });
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SCCTextNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents(null, "").ToList(), new List<string> { "B2" });
        }

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SCCTextInvalid()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("10Azaz", "    ").ToList(), new List<string> { "B1" });
        }

        [TestMethod]
        public void SCCText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            CollectionAssert.AreEqual(sheet.SetCellContents("a12", "hello").ToList(), new List<string> { "A12" });
            CollectionAssert.AreEqual(sheet.SetCellContents("Black71", "BrOThaPls").ToList(), new List<string> { "BLACK71" });
            CollectionAssert.AreEqual(sheet.SetCellContents("AzUlE1472", "").ToList(), new List<string> { "AZULE1472" });
            CollectionAssert.AreEqual(sheet.SetCellContents("a12", "hey!").ToList(), new List<string> { "A12" });

            Assert.AreEqual((string)sheet.GetCellContents("a12"), "hey!");
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

        [TestMethod]
        public void SCCFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a1", new Formula("5 + 10 * 4 / (2/2)")).ToList(), new List<string> { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetCellContents("b1", new Formula("a1 + 4*3")).ToList(), new List<string> { "B1" , "A1" });
            CollectionAssert.AreEquivalent(sheet.SetCellContents("cARSON1", new Formula("B1 + b1")).ToList(), new List<string> { "CARSON1" , "B1", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetCellContents("d1", new Formula("cARSON1/3")).ToList(), new List<string> { "D1", "CARSON1" , "B1" , "A1" });
            CollectionAssert.AreEquivalent(sheet.SetCellContents("e1", 0).ToList(), new List<string> { "E1" });
            CollectionAssert.AreEquivalent(sheet.SetCellContents("e1", new Formula("5 + 10*70 * 4 / (2/2)")).ToList(), new List<string> { "E1" });

            Assert.AreEqual(sheet.GetCellContents("A1"), new Formula("5 +10 *4 /(2 / 2)"));
            Assert.AreEqual(sheet.GetCellContents("b1"), new Formula("a1+4*3"));
            Assert.AreEqual(sheet.GetCellContents("CaRsOn1").ToString(), new Formula("b1+B1").ToString() , true);
            Assert.AreEqual(sheet.GetCellContents("d1"), new Formula("cARSON1/3"));
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SCCFormulaCircular()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a1", new Formula("5 + 10 * 4 / (2/2)")).ToList(), new List<string> { "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("b1", new Formula("a1 + 4*3")).ToList(), new List<string> { "B1", "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("cARSON1", new Formula("B1 + b1")).ToList(), new List<string> { "CARSON1", "B1", "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("d1", new Formula("e1/3")).ToList(), new List<string> { "D1" , "E1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("e1", new Formula("(g1) * 5")).ToList(), new List<string> { "E1", "G1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("e1", new Formula("(d1) * 5")).ToList(), new List<string> { "E1", "D1" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCCFormulaNullName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a1", new Formula("5 + 10 * 4 / (2/2)")).ToList(), new List<string> { "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("b1", new Formula("a1 + 4*5*a1")).ToList(), new List<string> { "B1", "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("bRo1023", new Formula("B1 / b1")).ToList(), new List<string> { "BRO1023", "B1", "A1" });
            CollectionAssert.AreEqual(sheet.SetCellContents(null, new Formula("a1 + 4*5*a1")).ToList(), new List<string> { "B1", "A1" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCCFormulaInvalidName1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a0", new Formula("a1 + 4*5*a1")).ToList(), new List<string> { "A0", "A1" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCCFormulaInvalidName2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a10b12", new Formula("a1 + 4*5*a1")).ToList(), new List<string> { "A10B12", "A1" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCCFormulaInvalidName3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("abDDDf01232", new Formula("a1 + 4*5*a1")).ToList(), new List<string> { "ABDDDF01232", "A1" });
        }

        [TestMethod]
        public void SCCFormulaZeroArg()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("abDDDf1232", new Formula()).ToList(), new List<string> { "ABDDDF1232" });
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SCCFormulaEmptyArg()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("abDDDf1232", new Formula("")).ToList(), new List<string> { "ABDDDF1232" });
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SCCFormulaSpaceArg()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("abDDDf1232", new Formula("   ")).ToList(), new List<string> { "abDDDf1232" });
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SCCFormulaCircularNoSet()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("f1", new Formula("(g1) * 5e5")).ToList(), new List<string> { "F1", "G1" });
            CollectionAssert.AreEqual(sheet.SetCellContents("g1", new Formula("f1 * 2.4/3")).ToList(), new List<string> { "G1", "F1" });
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SCCFormulaSelfCircular()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetCellContents("a1", new Formula("5 + 10 * 4 / (a1/2)")).ToList(), new List<string> { "A1" });
        }

    }
}
