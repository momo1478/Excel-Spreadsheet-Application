using Dependencies;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class DependencyGraphTestCases
    {

        /// <summary>
        /// Tests AddDependency and GetDependents and GetDependees without null values. Not a stress test.
        /// </summary>
        [TestMethod]
        public void AddDependency1()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "d");
            DG.AddDependency("a", "c");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            DG.AddDependency(null, "c");
            DG.AddDependency("b", null);
            DG.AddDependency(null, null);


            List <string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() { "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "a" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("a").ToList();
            list2 = new List<string>() { "b", "c", "d" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { "a", "c" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("c").ToList();
            list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "a", "b" };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Tests AddDependency and GetDependents and GetDependees without null values. Not a stress test.
        /// </summary>
        [TestMethod]
        public void HasDependency1()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "d");
            DG.AddDependency("a", "c");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            DG.AddDependency("e", "c");

            Assert.IsTrue(DG.HasDependees("b"));
            Assert.IsTrue(DG.HasDependees("c"));
            Assert.IsTrue(!DG.HasDependees("g"));
            Assert.IsTrue(!DG.HasDependees(""));
            Assert.IsTrue(!DG.HasDependees("      "));
            Assert.IsTrue(!DG.HasDependees(null));
            Assert.IsTrue(!DG.HasDependees(string.Empty));

            Assert.IsTrue(DG.HasDependents("b"));
            Assert.IsTrue(DG.HasDependents("a"));
            Assert.IsTrue(DG.HasDependents("e"));
            Assert.IsTrue(!DG.HasDependents("c"));
            Assert.IsTrue(!DG.HasDependents(""));
            Assert.IsTrue(!DG.HasDependents("      "));
            Assert.IsTrue(!DG.HasDependents(null));
            Assert.IsTrue(!DG.HasDependents(string.Empty));
        }

        /// <summary>
        /// very basic test AddDependencies with no self dependencies.
        /// Tests HasDependencies and HasDependees as well as Size.
        /// </summary>
        [TestMethod]
        public void RemoveDependency2()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "f");
            DG.AddDependency("a", "d");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "e");
            DG.AddDependency("a", "b");
            DG.AddDependency("b", "f");
            DG.AddDependency("b", "c");

            List<string> list1 = DG.GetDependents("a").ToList();
            List<string> list2 = new List<string>() { "b", "c", "d", "e", "f" };

            CollectionAssert.AreEqual(list1, list2);
            CollectionAssert.AreEqual(DG.GetDependees("a").ToList(), new List<String> { });
            CollectionAssert.AreEqual(DG.GetDependees("b").ToList(), new List<String> { "a" });
            CollectionAssert.AreEqual(DG.GetDependees(null).ToList(), new List<String> { });
            Assert.AreEqual(DG.Size, 7);

            DG.RemoveDependency("a", "e");
            DG.RemoveDependency("b", "f");
            DG.RemoveDependency("a", "g");
            DG.RemoveDependency(null, "g");
            DG.RemoveDependency("a", null);
            DG.RemoveDependency(null, null);
            DG.RemoveDependency("d", "a");
            DG.RemoveDependency("c", "b");
            DG.RemoveDependency("", "");
            DG.RemoveDependency("     ", string.Empty);

            Assert.AreEqual(DG.Size, 5);

            list1 = DG.GetDependents("a").ToList();
            list2 = new List<string>() { "b", "c", "d", "f" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { "c" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "a" };

            CollectionAssert.AreEqual(list1, list2);

            DG.RemoveDependency("a", "b");

            Assert.AreEqual(DG.Size, 4);

            CollectionAssert.AreNotEqual(DG.GetDependees("b").ToList(), list2);
        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void Replace1()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            

            DG.ReplaceDependents("a", new List<String> { "m", "k", "g" });

            List<string> list1 = DG.GetDependents("a").ToList();
            List<string> list2 = new List<string>() { "g", "k", "m" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependents("c", new List<String> { "p", "r", "s" });

            list1 = DG.GetDependents("c").ToList();
            list2 = new List<string>() { "p", "r", "s" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependees("b", new List<String> { "1", "2" });

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "b" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependees("c", new List<String> { "p", "r", "s" });

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "p", "r", "s" };

            CollectionAssert.AreEqual(list1, list2);

        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void Replace2()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            DG.AddDependency("c", "a");
            DG.AddDependency("c", "c");
            DG.AddDependency("c", "a");
            DG.AddDependency("c", "c");
            DG.AddDependency("e", "f");

            DG.ReplaceDependees("c", new List<String> { "m", "k", "g" });

            List<string> list1 = DG.GetDependees("c").ToList();
            List<string> list2 = new List<string>() { "g", "k", "m" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependees("b", new List<String> { "p", "r", "s" });

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "p", "r", "s" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependees(null, new List<String> { "4" , "3" , "2", "1" });
            DG.ReplaceDependees("f", new List<String> { "4", "3", "2", "1" });
            DG.ReplaceDependees("x", null);
            DG.ReplaceDependees("a", null);

            list1 = DG.GetDependees("a").ToList();
            list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependents(null, new List<String> { "4", "3", "2", "1" });
            DG.ReplaceDependents("f", new List<String> { "4", "3", "2", "1" });
            DG.ReplaceDependents("x", null);
            DG.ReplaceDependents("a", null);

            list1 = DG.GetDependents("a").ToList();
            list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void StressTest1()
        {
            DependencyGraph DG = new DependencyGraph();

            List<string> list2 = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    DG.AddDependency("dee" + i, "dent" + j);
                }
            }

            List<string> list1 = DG.GetDependees("dent1").ToList();
            list2 = DG.GetDependents("dee1").ToList();
            Assert.IsTrue(DG.HasDependees("dent1"));
            Assert.IsTrue(DG.HasDependents("dee1"));
            Assert.AreEqual(list1.Count, 1000);
            Assert.AreEqual(list2.Count, 100);

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    DG.RemoveDependency("dee" + i, "dent" + j);
                }
            }

            list1 = DG.GetDependees("dent1").ToList();
            list2 = DG.GetDependents("dee1").ToList();

            Assert.AreEqual(list1.Count, 0);
            Assert.AreEqual(list2.Count, 0);
        }
    }
}
