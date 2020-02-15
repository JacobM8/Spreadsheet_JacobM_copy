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
            string str = "1";
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            s.SetContentsOfCell("A1", str);
            Assert.AreEqual(1.0.ToString(), s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetCellContentsWhenStringSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            string str = "String Test";
            s.SetContentsOfCell("A1", str);
            Assert.AreEqual(str, s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetCellContentsWhenFormulaSimple()
        {
            Spreadsheet s = new Spreadsheet();
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            string str = "=2+2";
            s.SetContentsOfCell("A1", str);
            Assert.AreEqual("2+2", s.GetCellContents("A1"));
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
            string d = "32.2";
            s.SetContentsOfCell("A1", d);
            Assert.AreEqual(d, s.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet();
            string d = "32.2";
            s.SetContentsOfCell("3L2", d);
            Assert.AreEqual(d, s.GetCellContents("3L2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsNull()
        {
            Spreadsheet s = new Spreadsheet();
            string d = "32.2";
            string str = null;
            s.SetContentsOfCell(str, d);
            Assert.AreEqual(d, s.GetCellContents("3L2"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContained()
        {
            Spreadsheet s = new Spreadsheet();
            string d = "32.2";
            string str = "x1";
            string str1 = "44";
            s.SetContentsOfCell(str, d);
            s.SetContentsOfCell("x1", str1);
            Assert.AreEqual(44.0.ToString(), s.GetCellContents("x1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndItIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            string f = "=2+2";
            string d = "38.4";
            string str = "x1";
            s.SetContentsOfCell(str, f);
            s.SetContentsOfCell("x1", d);
            Assert.AreEqual(d, s.GetCellContents("x1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenCellContentsIsANullString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = null;
            s.SetContentsOfCell("x1", str);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsANullString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = null;
            s.SetContentsOfCell(str, "=2+2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsAnInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet();
            string str = "3x";
            s.SetContentsOfCell(str, "=2+2");
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndItIsAString()
        {
            Spreadsheet s = new Spreadsheet();
            string str = "x1";
            s.SetContentsOfCell(str, "=2+2");
            s.SetContentsOfCell("x1", "=4+4");
            Assert.AreEqual("4+4", s.GetCellContents("x1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAsAStringAndContentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            string f = "=2+2";
            string str1 = "=3*2";
            string str = "x1";
            s.SetContentsOfCell(str, f);
            s.SetContentsOfCell("x1", str1);
            Assert.AreEqual("3*2", s.GetCellContents("x1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellWhenFormulaIsSetAsNull()
        {
            Spreadsheet s = new Spreadsheet();
            string f = null;
            string str = "x1";
            s.SetContentsOfCell(str, f);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsANullStringAndContentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("=2+2");
            string str = null;
            s.SetContentsOfCell(str, "=2+2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellWhenCellNameIsAnInvalidVariableAndConentsIsAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            string f = "=2+2";
            string str = "3x";
            s.SetContentsOfCell(str, f);
        }

        [TestMethod]
        public void TestSetContentsOfCellWhenCellNameIsAlreadyContainedAndContentsAreAFormula()
        {
            Spreadsheet s = new Spreadsheet();
            string f = "=2+2";
            string f1 = "=4*4";
            s.SetContentsOfCell("x1", f);
            s.SetContentsOfCell("x1", f1);
            Assert.AreEqual("4*4", s.GetCellContents("x1"));
        }

        // The following TestMethods test GetNamesOfAllNonemptyCells function
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsReturnsCorrectEnermerable()
        {
            Spreadsheet s = new Spreadsheet();
            string d = "32.2";
            string str = "341.0";
            s.SetContentsOfCell("A1", d);
            s.SetContentsOfCell("A2", d);
            s.SetContentsOfCell("A3", str);
            Spreadsheet s1 = new Spreadsheet();
            s1.SetContentsOfCell("A1", d);
            s1.SetContentsOfCell("A2", d);
            s1.SetContentsOfCell("A3", str);
            Assert.AreEqual(s1.GetNamesOfAllNonemptyCells().ToString(), s.GetNamesOfAllNonemptyCells().ToString());
        }

        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsIsNotNull()
        {
            Spreadsheet s = new Spreadsheet();
            string d = "32.2";
            string str = "341.0";
            s.SetContentsOfCell("A1", d);
            s.SetContentsOfCell("A2", d);
            s.SetContentsOfCell("A3", str);
            Assert.AreNotEqual(null, s.GetNamesOfAllNonemptyCells());
        }

        // Tests for save function
        [TestMethod]
        public void testSave()
        {
            Spreadsheet s = new Spreadsheet();
            string expectedResult = "<?xml version=\"default\"encoding=\" utf-8\"?\n<name>A1\n<content>\"3\"</content></name></spreadsheet>";
            string str = "3";
            s.SetContentsOfCell("A1", str);
            s.Save("filename");
            string load = System.IO.File.ReadAllText("filename");
            Assert.AreEqual(expectedResult, load);

            // another test is to create a new spreadsheet from the loaded version and check if the contents are the same
        }

        // Tests for GetCellValue
        [TestMethod]
        public void TestGetCellValueSimpleFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2");
            Assert.AreEqual(2.0, s.GetCellValue("A1"));
        }
    }
}
