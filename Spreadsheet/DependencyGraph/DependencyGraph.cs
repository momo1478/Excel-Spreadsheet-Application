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
        /// The first element of each list holds the name of the dependent that the dependees rely upon.
        /// The second element onwards holds the List of dependees for that dependent.
        /// This 2-D list is sorted in each of its dimensions (using the string comparer).
        /// </summary>
        private List<List<String>> dependents;

        /// <summary>
        /// 2-D List of Strings that hold dependees in its first dimension.
        /// The first element of each list holds the name of the dependee that the dependents rely upon.
        /// The second element onwards holds the List of dependents for that dependee.
        /// This 2-D list is sorted in each of its dimensions (using the string comparer).
        /// Do we need this? It makes my life a whole lot easier! It also makes the dependee side much more efficient.
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
            get { return count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            //do a null check on s first. This method can be made faster.
            return !ReferenceEquals(s,null) && GetDependents(s).Count() > 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            //do a null check on s first. This method can be made faster.
            return !ReferenceEquals(s, null) && GetDependees(s).Count() > 0;
        }

        /// <summary>
        /// Enumerates dependee(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (ReferenceEquals(s,null)) //s is null? Break enumeration (IEnumerable's count will be 0)
                yield break;

            int firstDimIndex = findInFirstDim(dependents, s); //Find dependent so we can extract its list of dependees.

            if (firstDimIndex < 0)  //couldn't find s, s doesn't exist, so it doesn't have dependees.
            {
                yield break;
            }
            else
            {
                //we start at 1 because the first element (0) is the dependent of the list.
                for (int i = 1; i < dependents[firstDimIndex].Count; i++)  
                {
                    yield return dependents[firstDimIndex][i]; //return each dependee one at a time.
                }
            }
        }

        /// <summary>
        /// Enumerates dependent(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (ReferenceEquals(s, null)) //s is null? Break enumeration (IEnumerable's count will be 0)
                yield break;

            int firstDimIndex = findInFirstDim(dependees, s); //Find dependee so we can extract its list of dependents.

            if (firstDimIndex < 0) //can't find dependee, if dependee doesn't exist, then it has no dependents.
            {
                yield break; //return each dependent one at a time.
            }
            else
            {
                //we start at 1 because the first element (0) is the dependent of the list.
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
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check on s and t
                return;

            int firstDimIndex = findInFirstDim(dependents, s); //see if s is present in DG and if so, where?

            if (firstDimIndex >= 0) //s is already a dependent
            {
                int secondDimIndex = findInSecondDim(dependents[firstDimIndex], t); ////see if t is present in s's list and if so, where?

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
            else //s doesn't exist, so t can't! Add (s,t)
            {
                dependents.Add(new List<string> { s, t } ); 
                AddDependee(s, t); //manage dependee 2-D List.
                count++;           //we've added a dependency.
            }

        }

        /// <summary>
        /// Mimics AddDependent but reverses the roles of the dependent and the dependee.
        /// The dependee will now be in the first dimension and the dependent will the be the list of
        /// items following each dependee.
        /// Note that this is done in a sperate list.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private void AddDependee(string s, string t)
        {
            
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check
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
        /// Removes the dependency (s,t) in dependents from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null)) //null check
                return;

            int firstDimIndex = findInFirstDim(dependents, s); //find s's list of dependees.

            if (firstDimIndex >= 0) //t is already a dependee
            {
                int secondDimIndex = findInSecondDim(dependents[firstDimIndex], t);

                if (secondDimIndex >= 0) // (s,t) already exists remove it.
                {
                    dependents[firstDimIndex].RemoveAt(secondDimIndex);
                    RemoveDependee(s, t);
                }
                else //s exists, but t doesn't, don't do anything.
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
        /// Removes dependency (s,t) in dependee.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        private void RemoveDependee(string s, string t)
        {
            if (ReferenceEquals(s, null) || ReferenceEquals(t, null))
                return;

            int firstDimIndex = findInFirstDim(dependees, t); //find t's list of dependents

            if (firstDimIndex >= 0) //t is already a dependee
            {
                int secondDimIndex = findInSecondDim(dependees[firstDimIndex], s);

                if (secondDimIndex >= 0) // (s,t) exists remove it.
                {
                    dependees[firstDimIndex].RemoveAt(secondDimIndex);
                }
                else //t exists but s doesn't do remove anaything.
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
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (ReferenceEquals(s, null))
                return;

            int dependentsRow = findInFirstDim(dependents, s);

            if (dependentsRow >= 0) //s is a dependent in DG.
            {
                for (int i = 1; i < dependents[dependentsRow].Count; i++) //removing all dependees for s
                {
                    RemoveDependee(s, dependents[dependentsRow][i]); //Remove in dependee
                    dependents[dependentsRow].RemoveAt(i);           //Remove in dependents
                    i--;
                }


                
            }
            //add regardless of removing.
            List<String> newDependentsList = newDependents.ToList();
            for (int j = 0; j < newDependentsList.Count; j++)          //adding dependees for s
            {
                if (!ReferenceEquals(newDependentsList[j], null))      //null check
                {
                    AddDependency(s, newDependentsList[j]);            //Add Dependency (also modifies dependees)
                }
            }

        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary> 
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (ReferenceEquals(t, null)) //null check on t
                return;

            int dependeeRow = findInFirstDim(dependees, t); //find dependent list for t.

            if (dependeeRow >= 0)
            {
                for (int i = 1; i < dependees[dependeeRow].Count; i++) //removing all dependents for t
                {
                    RemoveDependency(dependees[dependeeRow][i], t);  //Remove in dependents and dependees.
                    i--;
                }
            }

            List<String> newDependeesList = newDependees.ToList();
            for (int j = 0; j < newDependeesList.Count; j++)          //adding dependees for s
            {
                if (!ReferenceEquals(newDependeesList[j], null))      //null check
                {
                    AddDependency(newDependeesList[j], t);            //Add Dependency (also modifies dependees)
                }
            }
        }
    }
}
