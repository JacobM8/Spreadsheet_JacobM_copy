/// <summary>
///     Author: Jacob Morrison
///     Date: 2/7/2020
///     This file is used to test Spreadsheet.cs
///     I pledge that I did the work myself.
/// </summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;
using SS;
using System;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        // The following TestMehod test the constructor
        [TestMethod]
        public void TestConstructor()
        {
            // Test new constructor is empty
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
        }

        // The following TestMethod's test the GetCellContent and SetCellContent methods
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsWhenNull()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            s.GetCellContents(null);
        }

        [TestMethod]
        public void TestGetCellContentsWhenDoubleSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            s.SetCellContents("A1", 1);
            Assert.AreEqual(1.0, s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetCellContentsWhenStringSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            string str = "String Test";
            s.SetCellContents("A1", str);
            Assert.AreEqual(str, s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetCellContentsWhenFormulaSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            Formula f = new Formula("2+2");
            s.SetCellContents("A1", f);
            Assert.AreEqual(f, s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetCellContentsWhenKeyIsNotContainedSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            Assert.AreEqual("", s.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsWhenInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            string var = "3m2";
            s.GetCellContents(var);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsString()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        public void TestSetContentsOfCellAsDouble()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            s.SetCellContents("A1", d);
            Assert.AreEqual(d, s.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            s.SetCellContents("3L2", d);
            Assert.AreEqual(d, s.GetCellContents("3L2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsNull()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            string str = null;
            s.SetCellContents(str, d);
            Assert.AreEqual(d, s.GetCellContents("3L2"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContained()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            string str = "x1";
            s.SetCellContents(str, d);
            s.SetCellContents("x1", 44);
            Assert.AreEqual(44.0, s.GetCellContents("x1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndItIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            double d = 38.4;
            string str = "x1";
            s.SetCellContents(str, f);
            s.SetCellContents("x1", d);
            Assert.AreEqual(d, s.GetCellContents("x1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenCellContentsIsANullString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = null;
            s.SetCellContents("x1", str);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenCellNameIsANullString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = null;
            s.SetCellContents(str, "2+2");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenCellNameIsAnInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet();
            string str = "3x";
            s.SetCellContents(str, "2+2");
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndItIsAString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = "x1";
            s.SetCellContents(str, "2+2");
            s.SetCellContents("x1", "4+4");
            Assert.AreEqual("4+4", s.GetCellContents("x1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAsAStringAndContentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            string str1 = "3*2";
            string str = "x1";
            s.SetCellContents(str, f);
            s.SetCellContents("x1", str1);
            Assert.AreEqual(str1, s.GetCellContents("x1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenFormulaIsSetAsNull()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = null;
            string str = "x1";
            s.SetCellContents(str, f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenCellNameIsANullStringAndContentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            string str = null;
            s.SetCellContents(str, "2+2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsAnInvalidVariableAndConentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            string str = "3x";
            s.SetCellContents(str, f);
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndContentsAreAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            Formula f1 = new Formula("4*4");
            s.SetCellContents("x1", f);
            s.SetCellContents("x1", f1);
            Assert.AreEqual(f1, s.GetCellContents("x1"));
        }

        // The following TestMethods test GetNamesOfAllNonemptyCells function
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsReturnsCorrectEnermerable()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            s.SetCellContents("A1", d);
            s.SetCellContents("A2", d);
            s.SetCellContents("A3", 341.0);
            Spreadsheet s1 = new Spreadsheet();
            s1.SetCellContents("A1", d);
            s1.SetCellContents("A2", d);
            s1.SetCellContents("A3", 341.0);
            Assert.AreEqual(s1.GetNamesOfAllNonemptyCells().ToString(), s.GetNamesOfAllNonemptyCells().ToString());
        }

        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsIsNotNull()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 32.2;
            s.SetCellContents("A1", d);
            s.SetCellContents("A2", d);
            s.SetCellContents("A3", 341.0);
            Assert.AreNotEqual(null, s.GetNamesOfAllNonemptyCells());
        }
    }
}
