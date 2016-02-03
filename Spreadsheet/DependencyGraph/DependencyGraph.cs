// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
using
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// A Dictionary where the Key (String) is the name of the dependent
        /// and the value (List<String>) is the List of dependees for that dependent. These lists are always sorted.
        /// </summary>
        private Dictionary<String, List<String>> dependents;

        /// <summary>
        /// A Dictionary where the Key (String) is the name of the dependee
        /// and the value (List<String>) is the List of dependents for that dependee. These lists are always sorted.
        /// </summary>
        private Dictionary<String, List<String>> dependees;

        /// <summary>
        /// number of dependency in the DependencyGraph.
        /// </summary>
        private int count;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, List<string>>();
            dependees = new Dictionary<string, List<string>>();

            count = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            return !ReferenceEquals(s, null) && dependees[s].Count > 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            return !ReferenceEquals(s, null) && dependents[s].Count > 0;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            List<String> dependentsList;  //null if s is not a dependee in DG.

            if (ReferenceEquals(s, null) || !dependees.TryGetValue(s, out dependentsList))
            //null check on s.           //TryGetValue returns false, meaning s is not in DG.
            {
                yield return "";
                yield break;
            }
            else
            {
                for (int i = 0; i < dependentsList.Count; i++)
                {
                    yield return dependentsList[i]; //iterate through dependees and return.
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            List<String> dependeesList;  //null if s is not a dependent in DG.

            if (ReferenceEquals(s, null) || !dependents.TryGetValue(s, out dependeesList)) 
                //null check on s.          //TryGetValue returns false, meaning s is not in DG.
            {
                yield return "";
                yield break;
            }
            else
            {
                for (int i = 0; i < dependeesList.Count; i++)
                {
                    yield return dependeesList[i]; //iterate through dependees and return.
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t.
                return;

            List<String> dependeeList;                       //null if the dependent doesn't exist.

            if (dependents.TryGetValue(s, out dependeeList)) //dependent exists  
            {
                int dependeeIndex = dependeeList.BinarySearch(t); 

                if (dependeeIndex >= 0) //if dependeeIndex >= 0 than (s,t) exists in dependents
                {
                    Debug.WriteLine("Dependency exists already in DG.");
                    return;
                }
                else                    //dependeeIndex < 0 than s exists but (s,t) doesn't. ~dependeeIndex is where it should go.
                {
                    dependeeList.Insert(~dependeeIndex, t); //Added in dependents.
                    AddDependencyInDependees(s, t);         //Added in dependees
                    count++;                                //Dependency added.
                }
            }
            else                                             //dependent doesn't exist.
            {
                dependents.Add(s, new List<string> { t });   //make a new dictionary entry with s as the dependent and t as first dependee. 
                AddDependencyInDependees(s, t);              //Add in dependees;
                count++;                                     //Dependency added.
            }

        }

        /// <summary>
        /// Manages dependees dictionary. Mimics AddDependency's functionality.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        private void AddDependencyInDependees(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t.
                return;

            List<String> dependentList;                      //null if the dependee doesn't exist.

            if (dependees.TryGetValue(t, out dependentList)) //dependee exists  
            {
                int dependentIndex = dependentList.BinarySearch(s);

                if (dependentIndex >= 0) //if dependentIndex >= 0 than (s,t) exists in dependees
                {
                    Debug.WriteLine("Dependency exists already in DG.");
                    return;
                }
                else                    //dependentIndex < 0 than t exists but (s,t) doesn't. ~dependentIndex is where it should go.
                {
                    dependentList.Insert(~dependentIndex, s);
                }
            }
            else                                             //dependee doesn't exist.
            {
                dependees.Add(t, new List<string> { s });   //make a new dictionary entry with s as the dependent and t as first dependee. 
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t.
                return;

            List<String> dependeeList;                       //null if the dependent doesn't exist.

            if (dependents.TryGetValue(s, out dependeeList)) //dependent exists  
            {
                int dependeeIndex = dependeeList.BinarySearch(t);

                if (dependeeIndex >= 0) //if dependeeIndex >= 0 than (s,t) exists in dependents
                {
                    dependents[s].RemoveAt(dependeeIndex); //remove at dependeeIndex, the dependee in dependents.
                    RemoveDependencyInDependees(s, t);    //sync dependees dictionary
                    count--;                              //Removed a dependency
                }
                else                    //dependeeIndex < 0 than s exists but (s,t) doesn't.
                {
                    Debug.WriteLine("(s,t) is not a dependency in DG.");
                    return;
                }
            }
            else                                             //dependent doesn't exist.
            {
                Debug.WriteLine("(s,t) is not a dependency in DG.");
                return;
            }
        }

        /// <summary>
        /// Manages dependees dictionary. Mimics RemoveDependency's functionality.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        private void RemoveDependencyInDependees(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t.
                return;

            List<String> dependentList;                       //null if the dependee doesn't exist.

            if (dependents.TryGetValue(t, out dependentList)) //dependee exists  
            {
                int dependentIndex = dependentList.BinarySearch(s);

                if (dependentIndex >= 0)                      //if dependentIndex >= 0 than (s,t) exists in dependees
                {
                    dependents[t].RemoveAt(dependentIndex);  //remove at dependentsIndex, the dependent in dependees.
                }
                else                                         //dependeeIndex < 0 than s exists but (s,t) doesn't.
                {
                    return;
                }
            }
            else                                             //dependent doesn't exist.
            {
                return;
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            List<String> dependeeList;
            if (!ReferenceEquals(s, null) && dependents.TryGetValue(s,out dependeeList)) //null check on s. If not remove it's dependees.
            {
                for (int i = 0; i < dependeeList.Count; i++)
                {
                    RemoveDependency(s, dependeeList[i]); //Iterate and remove dependencies, manages count too.
                    i--;                                  //For loop removal compensation.                           
                }
            }
            //Whether or not we removed dependees. Add each dependee from newDependents (the name doesn't make sense)
            List<String> addList = newDependents.ToList();

            for (int j = 0; j < addList.Count; j++) //Add each dependency. Handles null checks as well and count.
            {
                AddDependency(s, addList[j]);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            List<String> dependentList;
            if (!ReferenceEquals(t, null) && dependees.TryGetValue(t, out dependentList)) //null check on s. If not remove it's dependees.
            {
                for (int i = 0; i < dependentList.Count; i++)
                {
                    RemoveDependency(dependentList[i] , t); //Iterate and remove dependencies, manages count too.
                    i--;                                    //For loop removal compensation.                           
                }
            }
            //Whether or not we removed dependees. Add each dependee from newDependents (the name doesn't make sense)
            List<String> addList = newDependees.ToList();

            for (int j = 0; j < addList.Count; j++) //Add each dependency. Handles null checks as well and count.
            {
                AddDependency(addList[j] , t);
            }
        }
    }
}
