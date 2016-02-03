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
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");

            List<string> list1 = DG.GetDependents("a").ToList();
            List<string> list2 = new List<string>() { "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("a").ToList();
            list2 = new List<string>() { "b", "c", "d" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("a").ToList();
            list2 = new List<string>() { "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "a", "c" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("c").ToList();
            list2 = new List<string>() { "a", "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "" };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// very basic test AddDependencies with no self dependencies.
        /// Tests HasDependencies and HasDependees as well as Size.
        /// </summary>
        [TestMethod]
        public void RemoveDependency1()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("a", "e");
            DG.AddDependency("a", "f");

            List<string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() {"b", "c", "d", "e", "f" };

            CollectionAssert.AreEqual(list1, list2);
            Assert.AreEqual(DG.Size, 5);

            DG.RemoveDependency("a", "e");
            DG.RemoveDependency("a", "g");
            DG.RemoveDependency(null, "g");
            DG.RemoveDependency("a", null);
            DG.RemoveDependency(null, null);
            DG.RemoveDependency("d", "a");

            list1 = DG.GetDependees("a").ToList();
            list2 = new List<string>() { "b", "c", "d", "f" };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void AddDependencies2()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");

            List<string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() { "b", "c", "d"};

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "a", "c" };

            CollectionAssert.AreEqual(list1, list2);
        }
        
        /// <summary>
        /// AddDependencies with self dependencies.
        /// Tests of if one or either string are null and if the dpendent is already there.
        /// </summary>
        [TestMethod]
        public void AddDependencies3()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "a");
            DG.AddDependency("a", "e");
            DG.AddDependency("a", "b");
            DG.AddDependency(null, "b");
            DG.AddDependency("a", null);
            DG.AddDependency(null, null);

            List<string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() { "b", "c", "a", "e" };

            CollectionAssert.AreEqual(list1, list2);
        }
        
        /// <summary>
        /// Combination of tests 1 - 3. Tests AddDependencies 1 to 4 acheive 100% code coverage
        /// </summary>
        [TestMethod]
        public void AddDependencies4()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            DG.AddDependency("b", "b");
            DG.AddDependency("c", "a");
            DG.AddDependency("c", "e");
            DG.AddDependency("d", "c");
            DG.AddDependency("e", "b");

            List<string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() { "b", "c", "d" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "a", "c" , "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "a", "e" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("d").ToList();
            list2 = new List<string>() { "c" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependees("e").ToList();
            list2 = new List<string>() { "b" };

            CollectionAssert.AreEqual(list1, list2);
        }

        

        /// <summary>
        /// very basic test AddDependees with no self dependencies. With multiple dependees lists.
        /// </summary>
        [TestMethod]
        public void AddDependees2()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "a");
            DG.AddDependency("b", "a");
            DG.AddDependency("b", "c");
            DG.AddDependency("1", "c");
            DG.AddDependency("b", "a");

            List<string> list1 = DG.GetDependents("a").ToList();
            List<string> list2 = new List<string>() { "a" , "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { "a" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("c").ToList();
            list2 = new List<string>() { "a", "b" , "1" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("d").ToList();
            list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Basic RemoveDependency test.
        /// </summary>
        [TestMethod]
        public void RemoveDependencies1()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("a", "d");
            DG.AddDependency("a", "e");
            DG.AddDependency("a", "f");
            DG.RemoveDependency("a", "f");
            DG.RemoveDependency("a", "b");


            List<string> list1 = DG.GetDependents("f").ToList();
            List<string> list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Basic RemoveDependency test.
        /// -Removes with invalid string input
        /// -Removes nothing test.
        /// </summary>
        [TestMethod]
        public void RemoveDependencies2()
        {
            DependencyGraph DG = new DependencyGraph();

            DG.AddDependency("a", "b");
            DG.AddDependency("a", "c");
            DG.AddDependency("b", "d");
            DG.AddDependency("b", "e");
            DG.AddDependency("a", "a");

            DG.RemoveDependency("a", "a");
            DG.RemoveDependency("a", "g");
            DG.RemoveDependency("g", "a");
            DG.RemoveDependency(null, "c");
            DG.RemoveDependency("b" , null);
            DG.RemoveDependency(null, null);


            List<string> list1 = DG.GetDependents("a").ToList();
            List<string> list2 = new List<string>() { };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { "a" };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void ReplaceDependents1()
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

            DG.ReplaceDependents("a", new List<String> { "m", "k", "g" });

            List<string> list1 = DG.GetDependees("a").ToList();
            List<string> list2 = new List<string>() { "m", "k", "g" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependents("c", new List<String> { "p", "r", "s" });

            list1 = DG.GetDependees("c").ToList();
            list2 = new List<string>() { "p", "r", "s" };

            CollectionAssert.AreEqual(list1, list2);
        }

        /// <summary>
        /// Very basic test AddDependencies with no self dependencies. Now with 2 dependents
        /// </summary>
        [TestMethod]
        public void ReplaceDependees1()
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

            DG.ReplaceDependees("c", new List<String> { "m", "k", "g" });

            List<string> list1 = DG.GetDependents("c").ToList();
            List<string> list2 = new List<string>() { "m", "k", "g" };

            CollectionAssert.AreEqual(list1, list2);

            DG.ReplaceDependents("b", new List<String> { "p", "r", "s" });

            list1 = DG.GetDependees("b").ToList();
            list2 = new List<string>() { "p", "r", "s" };

            CollectionAssert.AreEqual(list1, list2);
        }

    }
}
