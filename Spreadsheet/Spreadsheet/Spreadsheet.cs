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
        DependencyGraph cellDependencyGraph;

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Creates a an empty constructor (Dictionary) with zero arguments.
        /// default is used when a specified version isn't used
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {
            // constructor will have ":this(formula, s => s, s => true)" like the constructor in formula but the word this will be changed. Google how to call inherited constructor c#
            // TODO: Your zero-argument constructor should create an empty spreadsheet that imposes no extra validity conditions, normalizes every cell name to itself, 
            // and use the name "default" as the version.

            // the string key will be the cell name and the Cell value is the contents of the cell i.e. string, 
            // double, or formula. 
            

        }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
            // TODO: You should add a three-argument constructor to the Spreadsheet class. Just like the zero-argument constructor, it should create an empty spreadsheet. 
            // However, it should allow the user to provide a validity delegate (first parameter), a normalization delegate (second parameter), and a version (third parameter).
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
        }

        public Spreadsheet(String pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
            // TODO: You should add a four-argument constructor to the Spreadsheet class. It should allow the user to provide a string representing a path to a file 
            // (first parameter), a validity delegate (second parameter), a normalization delegate (third parameter), and a version (fourth parameter). It should read a 
            // saved spreadsheet from the file (see the Save method) and use it to construct a new spreadsheet. The new spreadsheet should use the provided validity delegate, 
            // normalization delegate, and version. Do not try to implement loading from file until after we have discussed XML in class. 
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
            // TODO add stuff for pathToFile
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
        ///   Returns the names of all non-empty cells.
        /// </summary>
        /// <returns>
        ///     Returns an Enumerable that can be used to enumerate
        ///     the names of all the non-empty cells in the spreadsheet.  
        /// </returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // return key values from cells dictionary
            return dictionaryOfCells.Keys;
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <requires> 
        ///   The name parameter must be non null and valid
        /// </requires>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            // TODO need to return the list after cells have been recomputed
            // TODO undetstand if you need to take exception checks out of there because they aren't in the XML comment any more

            // if name is null or not valid throw InvalidNameException
            NameNullCheck(name);
            RegexVariableCheck(name);
            // if cells has name as a key add number to name
            if (dictionaryOfCells.ContainsKey(name))
            {
                // if name is a Formula replace its dependees with a empty set
                if (dictionaryOfCells[name].contents is Formula)
                {
                    // update cellDependencies with variables in formula
                    cellDependencyGraph.ReplaceDependees(name, new HashSet<string>());
                }
                dictionaryOfCells[name].contents = number;
            }
            // if cells does not have name as a key, add name as a key and number as it's value
            else
            {
                dictionaryOfCells.Add(name, new Cell(name, number));
            }
            // return the cell name and all values that depend on the cell name
            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <requires> 
        ///   The name parameter must be non null and valid
        /// </requires>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   <para>
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            // TODO need to return the list after cells have been recomputed
            // TODO undetstand if you need to take exception checks out of there because they aren't in the XML comment any more

            // if text is null throw ArgumentNullException
            ObjectNullCheck(text);
            // if name is null or invalid throw exception
            NameNullCheck(name);
            RegexVariableCheck(name);
            if (!text.Equals(""))
            {
                // if cells has name as a key add text to name
                if (dictionaryOfCells.ContainsKey(name))
                {
                    // if name is a Formula replace its dependees with a empty set
                    if (dictionaryOfCells[name].contents is Formula)
                    {
                        // update cellDependencies with variables in formula
                        cellDependencyGraph.ReplaceDependees(name, new HashSet<string>());
                    }
                    dictionaryOfCells[name].contents = text;
                }
                // if cells does not have name as a key, add name as a key and text as it's value
                else
                {
                    dictionaryOfCells.Add(name, new Cell(name, text));
                }
            }
            // return the cell name and all values that depend on the cell name
            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If formula parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <requires> 
        ///   The name parameter must be non null and valid
        /// </requires>
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
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            // TODO need to return the list after cells have been recomputed
            // TODO undetstand if you need to take exception checks out of there because they aren't in the XML comment any more

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
                cellDependencyGraph.ReplaceDependees(name, formula.GetVariables());
            }
            // if cells does not have name as a key, add name as a key and formula as it's value
            else
            {
                dictionaryOfCells.Add(name, new Cell(name, formula));
                cellDependencyGraph.ReplaceDependees(name, formula.GetVariables());
            }
            // try returning GetCellsToRecalculate on the giving name, if circular exception is thrown in GetCellsToRecalculate, 
            // reset cell contents to originalContents and throw exception
            try { return new List<string>(GetCellsToRecalculate(name)); }
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
            return cellDependencyGraph.GetDependents(name);
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            // checks for nulls and invalid
            throw new NotImplementedException();
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
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
