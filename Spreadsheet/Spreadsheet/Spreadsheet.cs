/// <summary>
///     Author: Jacob Morrison
///     Date: 2/14/2020
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
        }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
        }

        public Spreadsheet(String pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
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
                if (originalContents is Formula)
                {
                    cellDependencyGraph.ReplaceDependees(name, ((Formula)originalContents).GetVariables());
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

        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"> 
        ///   If the content parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        ///
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            // check for Null and invalidNames
            NameNullCheck(name);
            ObjectNullCheck(content);
            // if contents can be parsed as a double call SetCellContents(string, double) with contentAsDouble
            double contentAsDouble;
            if (Double.TryParse(content, out contentAsDouble))
            {
                SetCellContents(name, contentAsDouble);
            }
            // if contents starts with "=" call SetCellContents(string, Formula) with "=" removed from the content string
            if (content.StartsWith("="))
            {
                // remove "=" from content
                content = content.Remove(0, 1);
                Formula formulaFromContent = new Formula(content);
                SetCellContents(name, formulaFromContent);
            }
            // otherwise call SetCellContent(string, string) to save the string as the cell contents
            SetCellContents(name, content);

            return new List<string>(GetCellsToRecalculate(name));
        }


        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }
        public override string GetSavedVersion(string filename)
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
