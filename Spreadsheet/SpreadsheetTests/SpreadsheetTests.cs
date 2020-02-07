using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        // test constructor
        [TestMethod]
        public void TestConstructor()
        {
            // Test new constructor is empty
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            // get names of nonemptycells
            /*// use when s is not empty
            foreach (KeyValuePair<string, Cell> entry in cells)
            {

            }*/
            // should have a .MoveNext and if s is empty .MoveNext will return null and can test that it is returning null
            // s.GetNamesOfAllNonemptyCells().MoveNext
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsString()
        {
            // Test if cell content is a string GetCellContent returns a string
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsDouble()
        {
            // Test if cell content is a double GetCellContent returns a double
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsFormula()
        {
            // Test if cell content is a formula GetCellContent returns a formula
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        public void TestSetContentsOfCellAsDouble()
        {
            // Test if cell content is a formula GetCellContent returns a formula
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            s.SetCellContents("A1", d);
            Assert.AreEqual(d, s.GetCellContents("A1"));
        }
    }
}
