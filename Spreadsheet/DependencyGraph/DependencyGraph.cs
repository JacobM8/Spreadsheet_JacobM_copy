// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

/// <summary>
///     Author: Jacob Morrison
///     Date: 1/24/2020
///     This code determines the dependents and dependees of the variables in the spreadsheet and 
///     which need to be evaluated first.
///     I pledge that I did the work myself.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>  
    /// (s1,t1) is an ordered pair of strings  
    /// t1 depends on s1; s1 must be evaluated before t1  
    ///
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings. Two ordered pairs  
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.  
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a   
    /// set, and the element is already in the set, the set remains unchanged.  
    ///   
    /// Given a DependencyGraph DG:  
    ///   
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called 
    ///         dependents(s).  
    ///                 (The set of things that depend on s)      
    ///          
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called 
    ///         dependees(s).  
    ///                 (The set of things that s depends on)   
    //  
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}  
    //     dependents("a") = {"b", "c"}  
    //     dependents("b") = {"d"}  
    //     dependents("c") = {}  
    //     dependents("d") = {"d"}  
    //     dependees("a") = {}  
    //     dependees("b") = {"a"}  
    //     dependees("c") = {"a"}  
    //     dependees("d") = {"b", "d"}  
    /// </summary>  
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> DependentGraph;
        private Dictionary<string, HashSet<string>> DependeeGraph;
        private int size = 0;
        /// <summary>    
        /// Creates an empty DependencyGraph.    
        /// </summary>    
        public DependencyGraph()
        {
            DependentGraph = new Dictionary<string, HashSet<string>>();
            DependeeGraph = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>    
        /// The number of ordered pairs in the DependencyGraph.    
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>    
        /// The size of dependees(s).    
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would    
        /// invoke it like this:    
        /// dg["a"]    
        /// It should return the size of dependees("a")    
        /// </summary>    
        public int this[string s]
        {
            get
            {
                if (DependeeGraph.ContainsKey(s))
                {
                    return DependeeGraph[s].Count();
                }
                return 0;
            }
        }

        /// <summary>    
        /// Reports whether dependents(s) is non-empty.    
        /// </summary>    
        public bool HasDependents(string s)
        {
            if (DependentGraph.ContainsKey(s) && DependentGraph[s].Count() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>    
        /// Reports whether dependees(s) is non-empty.    
        /// </summary>    
        public bool HasDependees(string s)
        {
            if (DependeeGraph.ContainsKey(s) && DependeeGraph[s].Count() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>    
        /// Enumerates dependents(s).    
        /// </summary>    
        public IEnumerable<string> GetDependents(string s)
        {
            // if DepentGraph contains "s" return the set associated with "s"
            if (DependentGraph.ContainsKey(s))
            {
                return DependentGraph[s];
            }
            // new HashSet because 's' because DependentGraph[s] is empty
            return new HashSet<String>();
        }

        /// <summary>    
        /// Enumerates dependees(s).    
        /// </summary>    
        public IEnumerable<string> GetDependees(string s)
        {
            // if DepeneeGraph contains "s" return the set associated with "s"
            if (DependeeGraph.ContainsKey(s))
            {
                return DependeeGraph[s];
            }
            // new HashSet because 's' because DependeeGraph[s] is empty            
            return new HashSet<String>();
        }

        /// <summary>    
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>    
        ///     
        /// <para>This should be thought of as:</para>   
        ///     
        ///   t depends on s    
        ///    
        /// </summary>    
        /// <param name="s"> s must be evaluated first. T depends on S</param>    
        /// <param name="t"> t cannot be evaluated until s is</param>        
        public void AddDependency(string s, string t)
        {
            // check if s exists, if it does check for t, if t does not exist add t
            if (DependentGraph.ContainsKey(s))
            {
                if (!DependentGraph[s].Contains(t))
                {
                    // apply add to both graphs to keep them synced
                    DependentGraph[s].Add(t);
                    if (DependeeGraph.ContainsKey(t))
                    {
                        DependeeGraph[t].Add(s);
                        size++;
                    }
                    // if 't' doesn't exist in DependeeGraph, create new HashSet and add
                    else
                    {
                        AddReferenceToGraph(DependeeGraph, s, t);
                        size++;
                    }
                }
            }
            // if s does not exist add dependency
            else
            {
                // add new HashSet to DependentGraph, then add 't' to the new HashSet
                AddReferenceToGraph(DependentGraph, t, s);
                // add to DependeeGraph if 't' is already created
                if (DependeeGraph.ContainsKey(t))
                {
                    DependeeGraph[t].Add(s);
                    size++;
                }
                // add new HashSet to DependeeGraph then add 's' to the new HashSet
                else
                {
                    AddReferenceToGraph(DependeeGraph, s, t);
                    size++;
                }
            }
        }

        /// <summary>    
        /// Removes the ordered pair (s,t), if it exists    
        /// </summary>    
        /// <param name="s"></param>    
        /// <param name="t"></param>    
        public void RemoveDependency(string s, string t)
        {
            // check for dependency then remove appropriately from both graphs
            if (DependentGraph.ContainsKey(s) && DependentGraph[s].Contains(t))
            {
                DependentGraph[s].Remove(t);
                DependeeGraph[t].Remove(s);
                size--;
            }
        }

        /// <summary>    
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each    
        /// t in newDependents, adds the ordered pair (s,t).    
        /// </summary>     
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (DependentGraph.ContainsKey(s))
            {
                // create a copy because items are removed during the iteration
                HashSet<string> copyOfDependents = new HashSet<string>(DependentGraph[s]);
                foreach (string el in copyOfDependents)
                {
                    RemoveDependency(s, el);
                }
            }
            // add newDependents to DependentGraph
            foreach (string element in newDependents)
            {
                AddDependency(s, element);
            }
        }

        /// <summary>    
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each     
        /// t in newDependees, adds the ordered pair (t,s).    
        /// </summary>     
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (DependeeGraph.ContainsKey(s))
            {
                // create a copy because items are removed during the iteration
                HashSet<string> copyOfDependees = new HashSet<string>(DependeeGraph[s]);
                foreach (string el in copyOfDependees)
                {
                    RemoveDependency(el, s);
                }
            }
            // add newDependees to the DependeeGraph
            foreach (string el in newDependees)
            {
                AddDependency(el, s);
            }
        }

        // helper methods
        /// <summary>
        /// Adds new HashSet to given Dictionary with given strings
        /// </summary>
        /// <param name="graph"> dictionary to add a new HashSet to </param>
        /// <param name="s"> string to be added to graph </param>
        /// <param name="t"> string to be added to newGraph </param>
        private void AddReferenceToGraph(Dictionary<string, HashSet<string>> graph, string s, string t)
        {
            HashSet<string> newGraph = new HashSet<string>();
            graph.Add(t, newGraph);
            graph[t].Add(s);
        }
    }
}



