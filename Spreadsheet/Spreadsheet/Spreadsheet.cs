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
using System.Xml;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, Cell> dictionaryOfCells;
        DependencyGraph cellDependencyGraph;

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Creates a an empty spreadsheet (Dictionary) with zero arguments.
        /// default is used when a specified version isn't used
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {
        }

        /// <summary>
        /// Creates a spreadsheet with the given version
        /// </summary>
        /// <param name="isValid"> verifies variables are valid </param>
        /// <param name="normalize"> normalize "n" to "N" </param>
        /// <param name="version"> specified version of spreadsheet </param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// Creates a spreadsheet of the given version from the given file path
        /// </summary>
        /// <param name="pathToFile"> path to file </param>
        /// <param name="isValid"> verifies variables are valid </param>
        /// <param name="normalize"> normalize "n" to "N" </param>
        /// <param name="version"> specified version of spreadsheet </param>
        public Spreadsheet(String pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(s => true, s => s, version)
        {
            dictionaryOfCells = new Dictionary<string, Cell>();
            cellDependencyGraph = new DependencyGraph();
            using (XmlReader reader = XmlReader.Create(pathToFile))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name) 
                        {
                            case "Spreadsheet":
                                if (version == null)
                                {
                                    this.Version = reader["version"];
                                    Console.WriteLine("version: " + reader["cell"]);
                                }
                                break; 
                            case "Cell":
                                string name;
                                reader.Read();
                                name = reader.Value;
                                string contents;
                                reader.Read();
                                contents = reader.Value;
                                this.SetContentsOfCell(name, contents);
                                break;
                        }
                    }
                }
            }
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
                dictionaryOfCells[name].value = contentAsDouble;
            }
            // if contents starts with "=" call SetCellContents(string, Formula) with "=" removed from the content string and set the value
            if (content.StartsWith("="))
            {
                // remove "=" from content
                content = content.Remove(0, 1);
                Formula formulaFromContent = new Formula(content);
                SetCellContents(name, formulaFromContent);
                // evaluate contents of given cell name to set value
                dictionaryOfCells[name].value = formulaFromContent.Evaluate(DelegateLookupHelper);
            }
            // otherwise call SetCellContent(string, string) to save the string as the cell contents
            SetCellContents(name, content);

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            // specific settings for our XML writer
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("version");
                writer.WriteAttributeString("version", Version);
                // write each cell
                foreach (Cell c in dictionaryOfCells.Values)
                {
                    c.WriteXml(writer);
                }
                // end version block
                writer.WriteEndElement();
                // end file
                writer.WriteEndDocument();
            }
        }
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            string fileString = "";
            // use xmlReader has a method to get the version, movetONextAttribute, moveToAttribute, see ms docs
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        fileString = reader.GetAttribute(Version);
                    }
                }
            }
            return fileString; 
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            // if name isnull or invlaide throw InvalidNameException
            NameNullCheck(name);
            RegexVariableCheck(name);
            // return value of given cell name if it exists
            if (dictionaryOfCells.ContainsKey(name))
            {
                return dictionaryOfCells[name].value;
            }
            // else return a FormulaError
            return new FormulaError();
        }

        // helper methods
        // Func<string, double> lookup
        /// <summary>
        /// Delegate lookup, used to lookup variable in dictionaryOfCells
        /// </summary>
        /// <param name="name"> given cell name </param>
        /// <returns> double value of given cell </returns>
        public double DelegateLookupHelper(string name)
        {
            double result;
            if (!(GetCellValue(name) is string))
            {
                throw new ArgumentException();
                
            }
            double.TryParse(dictionaryOfCells[name].contents.ToString(), out result);
            return result; 
        }
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
