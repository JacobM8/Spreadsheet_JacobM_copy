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
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, Cell> dictionaryOfCells;
        DependencyGraph cellDependecyGraph;
        /// <summary>
        /// Creates a an empty constructor (Dictionary) with zero arguments.
        /// </summary>
        public Spreadsheet()
        {
            // the string key will be the cell name and the Cell value is the contents of the cell i.e. string, 
            // double, or formula. 
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependecyGraph = new DependencyGraph();
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
            NameNullCheck(name);
            RegexVariableCheck(name);
            // if name isn't in cell dicitonary return empty string
            if (!dictionaryOfCells.ContainsKey(name))
            {
                return "";
            }
            // return contents from Cell class constructor
            return dictionaryOfCells[name].contents;
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerates 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // return key values from cells dictionary
            return dictionaryOfCells.Keys;
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
            // if name is null or not valid throw InvalidNameException
            NameNullCheck(name);
            RegexVariableCheck(name);
            HashSet<string> newSet = new HashSet<string>();
            newSet.Add(name);
            // if cells has name as a key add number to name
            if (dictionaryOfCells.ContainsKey(name))
            {
                // if name is a Formula replace its dependees with a empty set
                if (dictionaryOfCells[name].contents is Formula)
                {
                    // update cellDependencies with variables in formula
                    cellDependecyGraph.ReplaceDependees(name, new HashSet<string>());
                }
                dictionaryOfCells[name].contents = number;
            }
            // if cells does not have name as a key, add name as a key and number as it's value
            else
            {
                dictionaryOfCells.Add(name, new Cell(name, number));
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
            // if text is null throw ArgumentNullException
            ObjectNullCheck(text);
            // if name is null or invalid throw exception
            NameNullCheck(name);
            RegexVariableCheck(name);
            HashSet<string> newSet = new HashSet<string>();
            if (!text.Equals(""))
            {
                newSet.Add(name);
                // if cells has name as a key add text to name
                if (dictionaryOfCells.ContainsKey(name))
                {
                    // if name is a Formula replace its dependees with a empty set
                    if (dictionaryOfCells[name].contents is Formula)
                    {
                        // update cellDependencies with variables in formula
                        cellDependecyGraph.ReplaceDependees(name, new HashSet<string>());
                    }
                    dictionaryOfCells[name].contents = text;
                }
                // if cells does not have name as a key, add name as a key and text as it's value
                else
                {
                    dictionaryOfCells.Add(name, new Cell(name, text));
                }
                foreach (string s in GetCellsToRecalculate(name))
                {
                    newSet.Add(s);
                }
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
            // if formula is null throw ArgumentNullException
            ObjectNullCheck(formula);
            // if name is null or invalid throw exception
            NameNullCheck(name);
            RegexVariableCheck(name);
            // copy of original cell contents to reset value if circular exception is thrown
            object originalContents = null;
            // if cells has name as a key add formula to name and set originalContents before changing
            if (dictionaryOfCells.ContainsKey(name))
            {
                originalContents = dictionaryOfCells[name].contents;
                dictionaryOfCells[name].contents = formula;
                cellDependecyGraph.ReplaceDependees(name, formula.GetVariables());
            }
            // if cells does not have name as a key, add name as a key and formula as it's value
            else
            {
                dictionaryOfCells.Add(name, new Cell(name, formula));
                cellDependecyGraph.ReplaceDependees(name, formula.GetVariables());
            }
            // try returning GetCellsToRecalculate on the giving name, if circular exception is thrown in GetCellsToRecalculate, 
            // reset cell contents to originalContents and throw exception
            try { return new HashSet<string>(GetCellsToRecalculate(name));  }
            catch
            {
                if (originalContents != null)
                {
                    dictionaryOfCells[name].contents = originalContents;
                }
                throw new CircularException();
            }
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
            // if name is null or invalid throw exception
            NameNullCheck(name);
            RegexVariableCheck(name);
            // return enumeration of all values that depend on the cell name
            return cellDependecyGraph.GetDependents(name);
        }

        // helper methods
        /// <summary>
        /// Throws InvalidNameException if given name is not a valid variable, otherwise returns true.
        /// </summary>
        /// <param name="name"> string to check if it's a variable </param>
        /// <returns> true if "s" is a variable</returns>
        public bool RegexVariableCheck(string name)
        {
            // return true if given string matches correct variable check
            if (Regex.IsMatch(name, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*"))
            {
                return true;
            }
            throw new InvalidNameException();
        }
        /// <summary>
        /// Throws InvalidNameException if given name is equal to null, otherwise returns false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool NameNullCheck(string name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            return false;
        }
        /// <summary>
        /// Throws ArgumentNullException if given object is null, otherwise returns false.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool ObjectNullCheck(object text)
        {
        if (text == null)
            {
                throw new ArgumentNullException();
            }
            return false;
        }
    }
}
