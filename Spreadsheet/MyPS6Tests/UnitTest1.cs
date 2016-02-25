using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Formulas;


namespace MyPS6Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SCC_NoException_Doubles()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1" , "5.0").ToList() , new List<string>() { "A1" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("A10", "125.111").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "5.0e3").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "4.0e3").ToList(), new List<string>() { "A11" });

            Assert.AreEqual(5.0, sheet.GetCellValue("a1"));
            Assert.AreEqual(125.111, sheet.GetCellValue("a10"));
            Assert.AreEqual(4000.0, sheet.GetCellValue("a11"));
        }

        [TestMethod]
        public void SCC_NoException_String()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "hello").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("A10", "dawg").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "!!!!234").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "123heyit'sme!").ToList(), new List<string>() { "A11" });

            Assert.AreEqual("hello", sheet.GetCellValue("a1"));
            Assert.AreEqual("dawg", sheet.GetCellValue("a10"));
            Assert.AreEqual("123heyit'sme!", sheet.GetCellValue("a11"));
        }

        [TestMethod]
        public void SCC_NoException_Formula1()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "=2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" , "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1").ToList(), new List<string>() { "A11" , "A10" , "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1").ToList(), new List<string>() { "A12" , "A1"  });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual(6.25, sheet.GetCellValue("a12"));
        }

        [TestMethod]
        public void SCC_NoException_Formula2()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "=2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1 + 4/2 * 5 + a1").ToList(), new List<string>() { "A11", "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1 / A1").ToList(), new List<string>() { "A12", "A1" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(15.5, sheet.GetCellValue("a11"));
            Assert.AreEqual(2.5, sheet.GetCellValue("a12"));

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=3 + A1").ToList(), new List<string>() { "A12", "A1" });

            Assert.AreEqual(5.5, sheet.GetCellValue("a12"));
        }


        [TestMethod]
        public void SCC_NoException_AllTypes()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1").ToList(), new List<string>() { "A11", "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "Yoink!").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual("Yoink!", sheet.GetCellValue("a12"));
        }

        [TestMethod]
        public void OneWay_Changed()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.IsFalse(sheet.Changed);
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "2.5").ToList(), new List<string>() { "A1" });
            Assert.IsTrue(sheet.Changed);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void RegexSCC_InvalidName_AllTypes()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("([A-Zb-z]+[1-9]{1}[0-9]*)"));

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A1", "2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A11", "=a10/A1").ToList(), new List<string>() { "A11", "A10", "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "Yoink!").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual("Yoink!", sheet.GetCellValue("a12"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCC_InvalidNameNull()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetContentsOfCell(null, "5.0").ToList(), new List<string>() { "A1" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCC_InvalidName()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("a"));

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a012", "5.0").ToList(), new List<string>() { "A1" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SCC_ContentsNull()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("a"));

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", null).ToList(), new List<string>() { "A12" });
        }

        [TestMethod]
        public void BasicSave()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5" ).ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });

            sheet.Save(destination);
        }

        [TestMethod]
        public void BasicConstruct()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            var writer = new StreamReader(File.OpenRead("../../ MySpreadsheet.xml"));
            TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            using (var stream = File.OpenRead("../../MySpreadsheet.xml"))
            {
                sheet.Save(destination);
            }
            
            TextReader source = File.OpenText("../../MySpreadsheet.xml");
            sheet = new Spreadsheet(source);
        }

    }
}
