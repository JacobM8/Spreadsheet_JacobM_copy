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
        /// <summary>
        /// Creates a an empty constructor (Dictionary) with zero arguments.
        /// </summary>
        public Spreadsheet()
        {
            // the string will be the cell name and Cell is the contents of the cell i.e. string, double, formula. 
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
            if (name == null || !RegexCheck(name))
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

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }

        // helper methods
        public bool RegexCheck(string name)
        {
            // TODO update regex formula to include "_"
            if (Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                return true;
            }
            return false;
        }
    }
}
