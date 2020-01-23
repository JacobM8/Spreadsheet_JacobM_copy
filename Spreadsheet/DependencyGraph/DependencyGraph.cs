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
        Dictionary<string, HashSet<string>> DependentGraph;
        Dictionary<string, HashSet<string>> DependeeGraph;
        private int size = 0;
        /// <summary>    
        /// Creates an empty DependencyGraph.    
        /// </summary>    
        public DependencyGraph()
        {
            // initialize member variables (global variables) for this class here in constructor
            // initialize adjencecy list as a dictionary
            // need a dictionary for dependents and another one for dependees
            // dependents will return it's dependees (which cells can't be solved until it is)
            // dependees will return it's dependents (which cells need to be solved before it)

            DependentGraph = new Dictionary<string, HashSet<string>>();
            DependeeGraph  = new Dictionary<string, HashSet<string>>();
            
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
            get { return s.Count(); }
        }

        /// <summary>    
        /// Reports whether dependents(s) is non-empty.    
        /// </summary>    
        public bool HasDependents(string s)
        {
            if (s.Count() != 0)
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
            if (s.Count() != 0)
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
            // if depentGraph contains "s" return the set associated with "s"
            if (DependentGraph.ContainsKey(s))
            {
                return DependentGraph[s];
            }
            // if no set is associated with "s" return an empty hashset because there is no set associated with that value
            return new HashSet<String>();
        }

        /// <summary>    
        /// Enumerates dependees(s).    
        /// </summary>    
        public IEnumerable<string> GetDependees(string s)
        {
            // if depeneGraph contains "s" return the set associated with "s"
            if (DependeeGraph.ContainsKey(s))
            {
                return DependeeGraph[s];
            }
            // if no set is associated with "s" return an empty hashset because there is no set associated with that value
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
            // added this and tests started failing that were previously passing
            // check to see if dependency exists
            // add if it doesn't
            if (DependentGraph[s].Contains(t))
            {
                throw new ArgumentException("Ordered pair (s,t) already exsists as a dependency so it can't be added.");
            }
            DependentGraph[s].Add(t);
            DependeeGraph[t].Add(s);
            size++;
        }

        /// <summary>    
        /// Removes the ordered pair (s,t), if it exists    
        /// </summary>    
        /// <param name="s"></param>    
        /// <param name="t"></param>    
        public void RemoveDependency(string s, string t)
        {
            if (DependentGraph[s].Contains(t))
            {
                DependentGraph[s].Remove(t);
                DependeeGraph[t].Remove(s);
                size--;
            }
            throw new ArgumentException("Ordered pair (s,t) doesn't exsists as a dependency so it can't be removed.");
        }

        /// <summary>    
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each    
        /// t in newDependents, adds the ordered pair (s,t).    
        /// </summary>     

        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
        }

        /// <summary>    
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each     
        /// t in newDependees, adds the ordered pair (t,s).    
        /// </summary>     
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
        }
    }
}



