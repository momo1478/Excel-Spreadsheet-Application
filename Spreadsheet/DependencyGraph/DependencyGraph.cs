﻿// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Dictionary<String, HashSet<String>> dependents;

        /// <summary>
        /// A Dictionary where the Key (String) is the name of the dependee
        /// and the value (List<String>) is the List of dependents for that dependee. These lists are always sorted.
        /// </summary>
        private Dictionary<String, HashSet<String>> dependees;

        /// <summary>
        /// number of dependency in the DependencyGraph.
        /// </summary>
        private int count;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();

            count = 0;
        }

        public DependencyGraph(DependencyGraph DG)
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();

            count = 0;

            foreach (KeyValuePair<string, HashSet<String>> dependency in DG.dependees)
            {
                List<String> valueList = dependency.Value.ToList();

                for (int i = 0; i < dependency.Value.Count; i++)
                {
                    this.AddDependency(dependency.Key, valueList[i]);
                }
            }
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
            if (ReferenceEquals(s, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            return !ReferenceEquals(s, null) && dependees.ContainsKey(s) && dependees[s].Count > 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (ReferenceEquals(s, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }
            return !ReferenceEquals(s, null) && dependents.ContainsKey(s) && dependents[s].Count > 0;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<String> dependentsHash;  //null if s is not a dependee in DG.

            if (ReferenceEquals(s, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            if (ReferenceEquals(s, null) || !dependees.TryGetValue(s, out dependentsHash))
            //null check on s.           //TryGetValue returns false, meaning s is not in DG.
            {
                yield break;
            }
            else
            {
                List<String> dependentsList = dependentsHash.ToList();
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
            HashSet<String> dependeesHash;  //null if s is not a dependent in DG.

            if (ReferenceEquals(s, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            if (ReferenceEquals(s, null) || !dependents.TryGetValue(s, out dependeesHash))
            //null check on s.          //TryGetValue returns false, meaning s is not in DG.
            {
                yield break;
            }
            else
            {
                List<String> dependeesList = dependeesHash.ToList();
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
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            HashSet<String> dependentHash;                            //null if the dependee doesn't exist.

            if (dependees.TryGetValue(s, out dependentHash))          //dependee exists  
            {
                                                                      
                if (dependentHash.Contains(t))                        //if dependeeIndex >= 0 than (s,t) exists in dependents
                {
                    Debug.WriteLine("Dependency exists already in DG.");
                    return;
                }
                else                                                  //dependeeIndex < 0 than s exists but (s,t) doesn't. ~dependeeIndex is where it should go.
                {
                    dependentHash.Add(t);                             //Added in dependents.
                    AddDependencyInDependents(s, t);                  //Added in dependees
                    count++;                                          //Dependency added.
                }
            }
            else                                                      //dependee doesn't exist.
            {
                dependees.Add(s, new HashSet<string> { t });          //make a new dictionary entry with s as the dependee and t as first dependent. 
                AddDependencyInDependents(s, t);                      //Add in dependents;
                count++;                                              //Dependency added.
            }

        }

        /// <summary>
        /// Manages dependees dictionary. Mimics AddDependency's functionality.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        private void AddDependencyInDependents(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t. //should be unreachable.
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            HashSet<String> dependeeHash;                             //null if the dependent doesn't exist.

            if (dependents.TryGetValue(t, out dependeeHash))          //dependent exists  
            {

                if (dependeeHash.Contains(s))                         //if dependeeIndex >= 0 than (s,t) exists in dependents
                {
                                                                      //Unreachable code
                    Debug.WriteLine("Dependency exists already in DG.");
                    return;
                }
                else                                                  //dependentIndex < 0 than t exists but (s,t) doesn't. ~dependentIndex is where it should go.
                {
                    dependeeHash.Add(s);
                }
            }
            else                                                      //dependent doesn't exist.
            {
                dependents.Add(t, new HashSet<string> { s });         //make a new dictionary entry with s as the dependent and t as first dependee. 
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
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            HashSet<String> dependentHash;                            //null if the dependee doesn't exist.

            if (dependees.TryGetValue(s, out dependentHash))          //dependee exists  
            {

                if (dependentHash.Contains(t))                        //if dependeeIndex >= 0 than (s,t) exists in dependents
                {
                    dependees[s].Remove(t);                           //remove at dependeeIndex, the dependee in dependents.
                    RemoveDependencyInDependents(s, t);               //sync dependees dictionary
                    count--;                                          //Removed a dependency
                }
                else                                                  //dependentIndex < 0, then s exists but (s,t) doesn't.
                {
                    Debug.WriteLine("(s,t) is not a dependency in DG.");
                    return;
                }
            }
            else                                                      //dependee doesn't exist.
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
        private void RemoveDependencyInDependents(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t. Should be unreachable
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            HashSet<String> dependeeHash;                             //null if the dependent doesn't exist.

            if (dependents.TryGetValue(t, out dependeeHash))          //dependent exists  
            {

                if (dependeeHash.Contains(s))                         //if dependentIndex >= 0 than (s,t) exists in dependees
                {
                    dependents[t].Remove(s);                          //remove at dependeeIndex, the dependee in dependents.
                }
                else                                                  //dependeeIndex < 0 than s exists but (s,t) doesn't.
                {
                                                                      //Unreachable Code : Here to see remblance to other Remove Dependency.
                    return;
                }
            }
            else                                                      //dependent doesn't exist.
            {
                                                                      //Unreachable Code : Here to see remblance to other Remove Dependency.
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
            HashSet<String> dependentHash;

            if (ReferenceEquals(s, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            if (!ReferenceEquals(s, null) && dependees.TryGetValue(s, out dependentHash)) //null check on s. If not remove it's dependees.
            {
                List<String> dependentList = dependentHash.ToList();

                for (int i = 0; dependentHash.Count > 0; i++)
                {
                    RemoveDependency(s, dependentList[i]);                                //Iterate and remove dependencies, manages count too.                  
                }
            }
            //Whether or not we removed dependees. Add each dependee from newDependents (the name doesn't make sense)
            List<String> addList = newDependents?.ToList() ?? new List<string>();

            for (int j = 0; j < addList.Count; j++)                                       //Add each dependency. Handles null checks as well and count.
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
            HashSet<String> dependeeHash;

            if (ReferenceEquals(t, null))
            {
                throw new ArgumentNullException("One of your parameters is null.");
            }

            if (!ReferenceEquals(t, null) && dependents.TryGetValue(t, out dependeeHash)) //null check on s. If not remove it's dependees.
            {
                List<String> dependeeList = dependeeHash.ToList();

                for (int i = 0; dependeeHash.Count > 0; i++)
                {
                    RemoveDependency(dependeeList[i], t);                                 //Iterate and remove dependencies, manages count too.                       
                }
            }
            //Whether or not we removed dependees. Add each dependee from newDependents (the name doesn't make sense)
            List<String> addList = newDependees?.ToList() ?? new List<String>();

            for (int j = 0; j < addList.Count; j++)                                        //Add each dependency. Handles null checks as well and count.
            {
                AddDependency(addList[j], t);
            }
        }

    }
}
