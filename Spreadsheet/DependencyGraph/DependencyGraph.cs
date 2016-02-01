// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016 

using System;
using System.Collections.Generic;
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
        /// 2-D List of Strings that hold dependents in its first dimension.
        /// The first dimension holds the name of the dependent that the dependees rely upon.
        /// The second dimension holds the List of dependees for each dependent.
        /// This 2-D list is sorted in each of its dimensions (using the string comparer).
        /// </summary>
        private List<List<String>> dependents;

        /// <summary>
        /// 2-D List of Strings that hold dependees in its first dimension.
        /// The first dimension holds the name of the dependee that the dependents rely upon.
        /// The second dimension holds the List of dependents for each dependee.
        /// This 2-D list is sorted in each of its dimensions (using the string comparer).
        /// Do we need this?
        /// </summary>
        private List<List<String>> dependees;

        /// <summary>
        /// Holds the amount of depndencies in the instance of the depency graph.
        /// </summary>
        private int count;


        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new List<List<String>>();
            dependees = new List<List<String>>();

            count = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            //?
            get { return count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s.Equals(null))
                yield break;

            int firstDimIndex = findInFirstDim(dependents, s);

            if (firstDimIndex < 0)
            {
                yield break;
            }
            else
            {
                for (int i = 1; i < dependents[firstDimIndex].Count; i++)
                {
                    yield return dependents[firstDimIndex][i];
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s.Equals(null))
                yield break;

            int firstDimIndex = findInFirstDim(dependees, s);

            if (firstDimIndex < 0)
            {
                yield break;
            }
            else
            {
                for (int i = 1; i < dependees[firstDimIndex].Count; i++)
                {
                    yield return dependees[firstDimIndex][i];
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
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null))
                return;

            int firstDimIndex = findInFirstDim(dependents, s);

            if (firstDimIndex >= 0) //s is already a dependent
            {
                int secondDimIndex = findInSecondDim(dependents[firstDimIndex], t);

                if (secondDimIndex >= 0) // (s,t) already exists, do nothing.
                {
                    return;
                }
                else // s exists but t doesn't, add it!
                {
                    dependents[firstDimIndex].Add(t);
                    AddDependee(s, t);
                    count++;
                }
            }
            else 
            {
                dependents.Add(new List<string> { s, t } ); //s is not a dependent. Add it with t.
                AddDependee(s, t);
                count++;
            }

        }

        /// <summary>
        /// Mimics AddDependent but reverses the roles of the dependent and the dependee.
        /// The dependee will now be in the first dimension and the dependent will the be the list of
        /// items following each dependee.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private void AddDependee(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null))
                return;

            int firstDimIndex = findInFirstDim(dependees , t);

            if (firstDimIndex >= 0) //t is already a dependee
            {
                int secondDimIndex = findInSecondDim(dependees[firstDimIndex], s);

                if (secondDimIndex >= 0) // (s,t) already exists, do nothing.
                {
                    return;
                }
                else // t exists but s doesn't, add it!
                {
                    dependees[firstDimIndex].Add(s);
                }
            }
            else
            {
                dependees.Add(new List<string> { t, s });  //add reversed to dependees.
            }
        }

        /// <summary>
        /// Searches the first element of each list in the given list of lists.
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static int findInFirstDim(List<List<String>> lists, string s)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                if (lists[i][0].Equals(s))
                {
                    return i;
                }
            }
            return -1;
        }
        
        /// <summary>
        /// Sees if an item is present in a list. Excludes first element in search (name element).
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static int findInSecondDim(List<String> list, string t)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].Equals(t))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null))
                return;

            int firstDimIndex = findInFirstDim(dependents, s);

            if (firstDimIndex >= 0) //t is already a dependee
            {
                int secondDimIndex = findInSecondDim(dependents[firstDimIndex], t);

                if (secondDimIndex >= 0) // (s,t) already exists remove it.
                {
                    dependents[firstDimIndex].RemoveAt(secondDimIndex);
                }
                else //t exists so s can't exist.
                {
                    return;
                }
            }
            else //s doesn't exist so (s,t) can't exists.
            {
                return;
            }

        }
        /// <summary>
        /// Applies the same logic as Remove Dependency to dependees.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        private void RemoveDependee(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null))
                return;

            int firstDimIndex = findInFirstDim(dependees, t);

            if (firstDimIndex >= 0) //t is already a dependee
            {
                int secondDimIndex = findInSecondDim(dependees[firstDimIndex], s);

                if (secondDimIndex >= 0) // (s,t) exists, do nothing.
                {
                    return;
                }
                else //t exists but s doesn't, add it!
                {
                    dependees[firstDimIndex].RemoveAt(secondDimIndex);
                }
            }
            else //s doesn't exist so (s,t) can't exists.
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

        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {

        }
    }
}
