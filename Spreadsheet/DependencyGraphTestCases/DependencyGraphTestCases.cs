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
        /// very basic test AddDependencies with no self dependencies.
        /// </summary>
        [TestMethod]
        public void AddDependencies1()
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
        public void AddDependees1()
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

            list1 = DG.GetDependents("b").ToList();
            list2 = new List<string>() { "a" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("c").ToList();
            list2 = new List<string>() { "a" , "b" };

            CollectionAssert.AreEqual(list1, list2);

            list1 = DG.GetDependents("d").ToList();
            list2 = new List<string>() { "a" };

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


    }
}
