using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Formulas;
using System.Xml;


namespace MyPS6Tests
{
    [TestClass]
    public class UnitTest1
    {
        //Basic Doubles Test
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
        //Basic String Test
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
        //Formula Evaluate test
        [TestMethod]
        public void SCC_NoException_Formula1()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "=2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual(6.25, sheet.GetCellValue("a12"));
        }
        //Formula Evaluate test
        [TestMethod]
        public void SCC_NoException_Formula2()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "=2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1 + 4/2 * 5 + a1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1 / A1").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(15.5, sheet.GetCellValue("a11"));
            Assert.AreEqual(2.5, sheet.GetCellValue("a12"));

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=3 + A1").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(5.5, sheet.GetCellValue("a12"));
        }
        //Formula Evaluate test and string and double test.
        [TestMethod]
        public void SCC_NoException_AllTypes()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "Yoink!").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual("Yoink!", sheet.GetCellValue("a12"));
        }
        //Changed changes states
        [TestMethod]
        public void OneWay_Changed()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.IsFalse(sheet.Changed);
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "2.5").ToList(), new List<string>() { "A1" });
            Assert.IsTrue(sheet.Changed);
        }

        //Regex constructor constrictions
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void RegexSCC_InvalidName_AllTypes()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("([A-Zb-z]+[1-9]{1}[0-9]*)"));

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A1", "2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A11", "=a10/A1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A11", "=a10/A1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "Yoink!").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(3.0, sheet.GetCellValue("a11"));
            Assert.AreEqual("Yoink!", sheet.GetCellValue("a12"));
        }

        //null name for Set Contents of Cell
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCC_InvalidNameNull()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEqual(sheet.SetContentsOfCell(null, "5.0").ToList(), new List<string>() { "A1" });
        }
        
        //Invalid name for Set Contents of Cell
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SCC_InvalidName()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("a"));

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a012", "5.0").ToList(), new List<string>() { "A1" });
        }

        //contents null for Set Contents of Cell
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SCC_ContentsNull()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("a"));

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", null).ToList(), new List<string>() { "A12" });
        }

        //Easy Save Test
        [TestMethod]
        public void BasicSave()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "3.5" ).ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "=3.5e5 + a10").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEqual(sheet.SetContentsOfCell("a13", "Wow it does work!").ToList(), new List<string>() { "A13" });
            sheet.Save(destination);
        }

        //Duplicate Construct Test
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void DuplicateConstruct()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            //TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5").ToList(), new List<string>() { "A1" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "=3.5e5 + a10").ToList(), new List<string>() { "A11" });
            //sheet.Save(destination);

            using (XmlReader reader = XmlReader.Create("../../MySpreadsheetDuplicate.xml"))
            {
                TextReader source = File.OpenText("../../MySpreadsheetDuplicate.xml");

                sheet = new Spreadsheet(source);

                Assert.IsFalse(sheet.Changed);

                reader.Dispose();
            }
            Assert.IsFalse(sheet.Changed);

        }

        //Read Evaluate test.
        [TestMethod]
        public void EvaluateChainConstruct()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            //TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5").ToList(), new List<string>() { "A1" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "=3.5e5 + a10").ToList(), new List<string>() { "A11" });
            //sheet.Save(destination);

            using (XmlReader reader = XmlReader.Create("../../MySpreadsheetChain.xml"))
            {
                TextReader source = File.OpenText("../../MySpreadsheetChain.xml");

                sheet = new Spreadsheet(source);

                Assert.AreEqual(0.15 ,sheet.GetCellValue("a6"));
                reader.Dispose();
            }

        }

        //Basic Load Spreadsheet
        [TestMethod]
        public void BasicConstruct()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            //TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5").ToList(), new List<string>() { "A1" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "=3.5e5 + a10").ToList(), new List<string>() { "A11" });
            //sheet.Save(destination);

            using (XmlReader reader = XmlReader.Create("../../MySpreadsheetCopy.xml"))
            {
                TextReader source = File.OpenText("../../MySpreadsheetCopy.xml");

                sheet = new Spreadsheet(source);

                reader.Dispose();
            }

        }
        // Circular Exception Load
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void Construct_CircularException()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex("[A-Za-z]+[1-9]"));
            //TextWriter destination = File.CreateText("../../MySpreadsheet.xml");

            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "I hope this works!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "Wow it does work!").ToList(), new List<string>() { "A12" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a1", "=3.5").ToList(), new List<string>() { "A1" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a11", "3.5e5").ToList(), new List<string>() { "A11" });
            //CollectionAssert.AreEqual(sheet.SetContentsOfCell("a12", "=3.5e5 + a10").ToList(), new List<string>() { "A11" });
            //sheet.Save(destination);

            using (XmlReader reader = XmlReader.Create("../../MySpreadsheetCorrupt.xml"))
            {
                TextReader source = File.OpenText("../../MySpreadsheetCorrupt.xml");

                sheet = new Spreadsheet(source);
            }

        }

        //Get Cell Value InvalidNameException
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GCV_Empty_InvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.AreEqual(sheet.GetCellValue("a1"), "");

            Assert.AreEqual(sheet.GetCellValue("a0"), "");
        }

        //Get Cell Value null name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GCV_Empty_NullName()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.AreEqual(sheet.GetCellValue("a1"), "");

            Assert.AreEqual(sheet.GetCellValue(null), "");
        }

        //Get Cell Value of an empty cell
        [TestMethod]
        public void GCV_Empty_NotPresent()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.AreEqual(sheet.GetCellValue("a1"), "");

            Assert.AreEqual(sheet.GetCellValue("A2"), "");
        }

        //Get Cell Value of a non empty cell
        [TestMethod]
        public void GCV_Empty_Present()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "5.0");

            Assert.AreEqual(sheet.GetCellValue("a1"), 5.0);
            Assert.AreEqual(sheet.GetCellValue("A2"), "");
        }

        //Complex evaluate
        [TestMethod]
        public void SCC_NoException_Formula3()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "=2.5").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1 + 4/2 * 5 + a1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1 / A1").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("B1", "=A1 + 4 *3*A1 / A1 + (a10/ 1)").ToList(), new List<string>() { "B1" });

            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(7.5, sheet.GetCellValue("a10"));
            Assert.AreEqual(15.5, sheet.GetCellValue("a11"));
            Assert.AreEqual(2.5, sheet.GetCellValue("a12"));
            Assert.AreEqual(22.0, sheet.GetCellValue("B1"));

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=3 + A1").ToList(), new List<string>() { "A12" });

            Assert.AreEqual(5.5, sheet.GetCellValue("a12"));
        }

        //Formula Error Test
        [TestMethod]
        public void SCC_NoException_FormulaError()
        {
            Spreadsheet sheet = new Spreadsheet();

            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a1", "Yoink").ToList(), new List<string>() { "A1" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("A10", "=a1 * 3").ToList(), new List<string>() { "A10" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a11", "=a10/A1 + 4/2 * 5 + a1").ToList(), new List<string>() { "A11" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("a12", "=A1 * A1 / A1").ToList(), new List<string>() { "A12" });
            CollectionAssert.AreEquivalent(sheet.SetContentsOfCell("B1", "=A1 + 4 *3*A1 / A1 + (a10/ 1)").ToList(), new List<string>() { "B1" });

            Assert.IsTrue(sheet.GetCellValue("a1") is String);
            Assert.IsTrue(sheet.GetCellValue("a10") is FormulaError);
            Assert.IsTrue(sheet.GetCellValue("a11") is FormulaError);
            Assert.IsTrue(sheet.GetCellValue("a12") is FormulaError);
            Assert.IsTrue(sheet.GetCellValue("b1") is FormulaError);

        }

    }
}
