using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionTestCases
{
    [TestClass]
    public class ExtensionTestCases
    {
        /// <summary>
        /// Testing the 3 parameter constructor in Formula
        /// </summary>
        [TestMethod]
        public void ThreeParamConstruct1()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);
        }

        /// <summary>
        /// Testing the 3 parameter constructor in Formula
        /// </summary>
        [TestMethod]
        public void ThreeParamConstruct2()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);

            myFormula = new Formula("2 + x3", s => s.ToUpper(), s => !s.Contains("x"));
        }

        /// <summary>
        /// Testing the 3 parameter constructor in Formula
        /// </summary>
        [TestMethod]
        public void ThreeParamConstruct3()
        {
            Formula myFormula = new Formula("(x2 + x3)", s => s.Replace("x", "y") , s => !s.Contains("x") );
        }

        /// <summary>
        /// Testing the 3 parameter constructor in Formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeParamConstruct4()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);

            myFormula = new Formula("2 + x3", s => null, s => !s.Contains("x"));
        }

        /// <summary>
        /// Validator should throw the exception here.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeParamConstruct5()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);

            myFormula = new Formula("2 + x3", s => s.Replace("x" ,string.Empty), s => s.Contains("x"));
        }

        /// <summary>
        /// Basic ToString() test.
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula myFormula = new Formula("7 * 3 / 2", s => s, s => true);
            Assert.IsTrue(myFormula.ToString().Equals("7 * 3 / 2"));
        }

        /// <summary>
        /// Basic ToString() test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ToString2()
        {
            Formula myFormula = new Formula(null, s => null, s => true);
            Assert.IsTrue(myFormula.ToString() == null);
        }

        /// <summary>
        /// GetVariables() basic test
        /// </summary>
        [TestMethod]
        public void GetVariables1()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);
            Assert.IsTrue(myFormula.GetVariables().Count == 0);

            myFormula = new Formula("2 + x3", s => s.Replace("x", "y"), s => s.Contains("y"));
            Assert.IsTrue(myFormula.GetVariables().Count == 1);
        }

        /// <summary>
        /// GetVariables() basic test
        /// </summary>
        [TestMethod]
        public void GetVariables2()
        {
            Formula myFormula = new Formula("2 + 3", s => s, s => true);
            Assert.IsTrue(myFormula.GetVariables().Count == 0);

            myFormula = new Formula("y2 + x3 / f412", s => s.Replace("x", "y"), s => !s.Contains("x"));
            Assert.IsTrue(myFormula.GetVariables().Count == 3);

            HashSet<String> expectedVars = new HashSet<string>();
            expectedVars.Add("y2");
            expectedVars.Add("y3");
            expectedVars.Add("f412");

            foreach (var item in expectedVars)
            {
                Assert.IsTrue(myFormula.GetVariables().Contains(item));
            }
        }

        /// <summary>
        /// Testing Dependency Graph copy functionality.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void DGCopy1()
        {
            DependencyGraph dg1 = new DependencyGraph();

            dg1.AddDependency("a", "b");
            dg1.AddDependency("a", "c");
            dg1.AddDependency("a", "d");
            dg1.AddDependency("a", "e");

            DependencyGraph dg2 = new DependencyGraph(dg1);

            Assert.IsFalse(ReferenceEquals(dg1, dg2));

            foreach (string dependent in dg1.GetDependents("a"))
            {
                CollectionAssert.AreEqual(dg1.GetDependees(dependent).ToList(), dg2.GetDependees(dependent).ToList());
            }

            dg2.AddDependency("b", "c");
            dg2.AddDependency("b", "d");
            dg2.AddDependency("b", "e");
            dg2.AddDependency("b", "b");

            Assert.IsFalse(ReferenceEquals(dg1, dg2));

            foreach (string dependent in dg1.GetDependents("a"))
            {
                CollectionAssert.AreNotEqual(dg1.GetDependees(dependent).ToList(), dg2.GetDependees(dependent).ToList());
            }

            throw new NotImplementedException("We should end up here. Arbitrary Exception.");
        }
    }
}
