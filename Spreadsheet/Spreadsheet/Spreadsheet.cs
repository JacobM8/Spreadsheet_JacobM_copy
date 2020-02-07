/// <summary>
///     Author: Jacob Morrison
///     Date: 2/7/2020
///     This file is used to get and set the contents of a cell, remember the contents and the value of the cell are 
///     separate. This file is also used to recalculate cells when a dependent cell is changed.
///     I pledge that I did the work myself.
/// </summary>
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, Cell> cells;
        DependencyGraph cellDependecies = new DependencyGraph();
        /// <summary>
        /// Creates a an empty constructor (Dictionary) with zero arguments.
        /// </summary>
        public Spreadsheet()
        {
            // the string will be the cell name and Cell is the contents of the cell i.e. string, double, or formula. 
            cells = new Dictionary<string, Cell>();
        }

        /// <summary>
        ///   Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is null or invalid
        /// </exception>
        /// 
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// 
        /// <returns>
        ///   The return value should be either a string, a double, or a Formula.
        ///   See the class header summary 
        /// </returns>
        public override object GetCellContents(string name)
        {
            // throw InvalidNameException if name is null or invalid 
            if (name == null || !RegexVariableCheck(name))
            {
                throw new InvalidNameException();
            }
            // if name isn't in cell dicitonary return empty string
            if (!cells.ContainsKey(name))
            {
                return "";
            }
            // return contents from Cell class constructor
            return cells[name].contents;
            
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerates 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // return key values from cells dictionary
            return cells.Keys;
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///      The method returns a set consisting of name plus the names of all other cells whose value depends, 
        ///      directly or indirectly, on the named cell.
        ///   </para>
        /// 
        ///   <para>
        ///      For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///      set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> newSet = new HashSet<string>();
            newSet.Add(name);
            // if name is null or not valid throw InvalidNameException
            if (name.Equals(null) || !RegexVariableCheck(name))
            {
                throw new InvalidNameException();
            }
            // if cells has name as a key add number to name
            if (cells.ContainsKey(name))
            {
                // if name is a Formula replace its dependees with a empty set
                if (cells[name].contents is Formula)
                {
                    // update cellDependencies with variables in formula
                    cellDependecies.ReplaceDependees(name, new HashSet<string>());
                }
                cells[name].contents = number;
            }
            // if cells does not have name as a key, add name as a key and number as it's value
            else
            {
                cells.Add(name, new Cell(name, number));
            }
            foreach (string s in GetCellsToRecalculate(name))
            {
                newSet.Add(s);
            }
            // return the cell name and all values that depend on the cell name
            return (ISet<string>)newSet;
        }

        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   The method returns a set consisting of name plus the names of all 
        ///   other cells whose value depends, directly or indirectly, on the 
        ///   named cell.
        /// 
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> newSet = new HashSet<string>();
            newSet.Add(name);
            // if text is null throw ArgumentNullException
            if (text.Equals(null))
            {
                throw new ArgumentNullException();
            }
            // if name is null or not valid throw InvalidNameException
            if (name.Equals(null) || !RegexVariableCheck(name))
            {
                throw new ArgumentNullException();
            }
            // if cells has name as a key add text to name
            if (cells.ContainsKey(name))
            {
                // if name is a Formula replace its dependees with a empty set
                if (cells[name].contents is Formula)
                {
                    // update cellDependencies with variables in formula
                    cellDependecies.ReplaceDependees(name, new HashSet<string>());
                }
                cells[name].contents = text;
            }
            // if cells does not have name as a key, add name as a key and text as it's value
            else
            {
                cells.Add(name, new Cell(name, text));
            }
            foreach (string s in GetCellsToRecalculate(name))
            {
                newSet.Add(s);
            }
            // return the cell name and all values that depend on the cell name
            return (ISet<string>)newSet;
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If formula parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name</param>
        /// <param name="formula"> The content of the cell</param>
        /// 
        /// <returns>
        ///   <para>
        ///     The method returns a Set consisting of name plus the names of all other 
        ///     cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///   <para> 
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// 
        /// </returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> newSet = new HashSet<string>();
            newSet.Add(name);
            // if formula is null throw ArgumentNullException
            if (formula.Equals(null))
            {
                throw new ArgumentNullException();
            }
            // if name is null or not valid throw InvalidNameException
            if (name.Equals(null) || !RegexVariableCheck(name))
            {
                throw new InvalidNameException();
            }
            // CircularException is checked somewhere else, don't need to check here

            // if cells has name as a key add formula to name
            if (cells.ContainsKey(name))
            {
                cells[name].contents = formula;
            }
            // if cells does not have name as a key, add name as a key and formula as it's value
            else
            {
                cells.Add(name, new Cell(name, formula));
            }
            foreach (string s in GetCellsToRecalculate(name))
            {
                newSet.Add(s);
            }
            // update cellDependencies with variables in formula
            cellDependecies.ReplaceDependees(name, formula.GetVariables());
            // return the cell name and all values that depend on the cell name
            return (ISet<string>)newSet;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. 
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If the name is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// 
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // if name is null throw ArgumentNullException
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            // if name is invalid throw InvalidNameException
            if (!RegexVariableCheck(name))
            {
                throw new ArgumentNullException();
            }
            // return enumeration of all values that depend on the cell name
            return cellDependecies.GetDependees(name);
        }

        // helper methods
        /// <summary>
        /// Checks to see the given string is a valid variable
        /// </summary>
        /// <param name="name"> string to check if it's a variable </param>
        /// <returns> true if "s" is a variable</returns>
        public bool RegexVariableCheck(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*"))
            {
                return true;
            }
            return false;
        }
    }
}
