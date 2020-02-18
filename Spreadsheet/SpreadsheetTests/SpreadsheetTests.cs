/// <summary>
///     Author: Jacob Morrison
///     Date: 2/14/2020
///     This file is used to test Spreadsheet.cs
///     I pledge that I did the work myself, exception there are tests at the bottom that were provided by the College Of Engineering at the U of U 
///     that have been modified to test for this assignment 5.
/// </summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;
using SS;
using System;
using System.IO;

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
            Assert.AreEqual(1.0, s.GetCellContents("A1"));
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
            Assert.AreEqual(new Formula("2+2"), s.GetCellContents("A1"));
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
            Assert.AreEqual(32.2, s.GetCellContents("A1"));
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
            Assert.AreEqual(44.0, s.GetCellContents("x1"));
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
            Assert.AreEqual(38.4, s.GetCellContents("x1"));
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
            Assert.AreEqual(new Formula("4+4"), s.GetCellContents("x1"));
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
            Assert.AreEqual(new Formula("3*2"), s.GetCellContents("x1"));
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
            Assert.AreEqual(new Formula("4*4"), s.GetCellContents("x1"));
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
        [ExpectedException(typeof(IOException))]
        public void testSave()
        {
            Spreadsheet s = new Spreadsheet();
            string expectedResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<version version=\"default\">\n  <name xmlns=\"A1\">\n    <content xmlns=\"3\">" +
                "\n      <contents>3</contents>\n    </content>\n  </name>\n</version>";
            string str = "3";
            s.SetContentsOfCell("A1", str);
            s.Save("/NonexistantFile.xml/");
            string load = System.IO.File.ReadAllText("NonexistantFile.xml");
            Assert.AreNotEqual(expectedResult, load);

        }

        // Tests GetSavedVersion
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void TestGetSavedVersion()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.Save("/NonexistantFile.xml/");
            Assert.AreEqual("default", s.GetSavedVersion("/NonexistantFile.xml/"));
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestSaveThrowsException()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save("/some/nonsense/path.xml");
        }

        // Tests for GetCellValue
        [TestMethod]
        public void TestGetCellValueSimpleDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2");
            Assert.AreEqual(2.0, s.GetCellValue("A1"));
        }

        [TestMethod]
        public void TestGetCellValueSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hello");
            Assert.AreEqual("hello", s.GetCellValue("A1"));
        }

        [TestMethod]
        public void TestGetCellValueSimpleFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=2+2");
            Assert.AreEqual(4.0, s.GetCellValue("A1"));
        }

        // use constructor with version to test getsavedversion

        // The Following test were provided by College of Engineering at the U of U and have been modified to run on PS5 Spreadsheet.cs by Jacob Morrison
        /// <summary>
        ///This is a test class for SpreadsheetTest and is intended
        ///to contain all SpreadsheetTest Unit Tests
        ///</summary>
        [TestClass()]
        public class AssignmentFourGradingTests
        {

            // EMPTY SPREADSHEETS
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestEmptyGetNull()
            {
                Spreadsheet s = new Spreadsheet();
                s.GetCellContents(null);
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestEmptyGetContents()
            {
                Spreadsheet s = new Spreadsheet();
                s.GetCellContents("1AA");
            }

            [TestMethod()]
            public void TestGetEmptyContents()
            {
                Spreadsheet s = new Spreadsheet();
                Assert.AreEqual("", s.GetCellContents("A2"));
            }

            // SETTING CELL TO A DOUBLE
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetNullDouble()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, "1.5");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetInvalidNameDouble()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("1A1A", "1.5");
            }

            [TestMethod()]
            public void TestSimpleSetDouble()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", "1.5");
                Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
            }

            // SETTING CELL TO A STRING
            [TestMethod()]
            [ExpectedException(typeof(ArgumentNullException))]
            public void TestSetNullStringVal()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A8", (string)null);
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetNullStringName()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, "hello");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetSimpleString()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("1AZ", "hello");
            }

            [TestMethod()]
            public void TestSetGetSimpleString()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", "hello");
                Assert.AreEqual("hello", s.GetCellContents("Z7"));
            }

            // SETTING CELL TO A FORMULA
            [TestMethod()]
            [ExpectedException(typeof(ArgumentNullException))]
            public void TestSetNullFormVal()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A8", null);
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetNullFormName()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, "=2");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void TestSetSimpleForm()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("1AZ", "=2");
            }

            [TestMethod()]
            public void TestSetGetForm()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", "=3");
                Formula f = (Formula)s.GetCellContents("Z7");
                Assert.AreEqual(new Formula("3"), f);
                Assert.AreNotEqual(new Formula("2"), f);
            }

            // CIRCULAR FORMULA DETECTION
            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void TestSimpleCircular()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=A2");
                s.SetContentsOfCell("A2", "=A1");
            }

            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void TestComplexCircular()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A3", "=A4+A5");
                s.SetContentsOfCell("A5", "=A6+A7");
                s.SetContentsOfCell("A7", "=A1+A1");
            }

            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void TestUndoCircular()
            {
                Spreadsheet s = new Spreadsheet();
                try
                {
                    s.SetContentsOfCell("A1", "=A2+A3");
                    s.SetContentsOfCell("A2", "15");
                    s.SetContentsOfCell("A3", "30");
                    s.SetContentsOfCell("A2", "=A3*A1");
                }
                catch (CircularException e)
                {
                    Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                    throw e;
                }
            }

            // NONEMPTY CELLS
            [TestMethod()]
            public void TestEmptyNames()
            {
                Spreadsheet s = new Spreadsheet();
                Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            }

            [TestMethod()]
            public void TestExplicitEmptySet()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "");
                Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            }

            [TestMethod()]
            public void TestSimpleNamesString()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
            }

            [TestMethod()]
            public void TestSimpleNamesDouble()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "52.25");
                Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
            }

            [TestMethod()]
            public void TestSimpleNamesFormula()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "3.5");
                Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
            }

            [TestMethod()]
            public void TestMixedNames()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("C1", "hello");
                s.SetContentsOfCell("B1", "3.5");
                Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
            }

            // RETURN VALUE OF SET CELL CONTENTS
            [TestMethod()]
            public void TestSetSingletonDouble()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                s.SetContentsOfCell("C1", "5");

                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("A1", "17.2")).SetEquals(new HashSet<string>() { "A1" }));
            }

            [TestMethod()]
            public void TestSetSingletonString()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("C1", "5");
                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("B1", "hello")).SetEquals(new HashSet<string>() { "B1" }));
            }

            [TestMethod()]
            public void TestSetSingletonFormula()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("B1", "hello");
                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("C1", "5")).SetEquals(new HashSet<string>() { "C1" }));
            }

            [TestMethod()]
            public void TestSetChain()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A2", "6");
                s.SetContentsOfCell("A3", "=A2+A4");
                s.SetContentsOfCell("A4", "=A2+A5");
                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("A5", "82.5")).SetEquals(new HashSet<string>() { "A5", "A4", "A3", "A1" }));
            }

            // CHANGING CELLS
            [TestMethod()]
            public void TestChangeFtoD()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A1", "2.5");
                Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
            }

            [TestMethod()]
            public void TestChangeFtoS()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A1", "Hello");
                Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
            }

            [TestMethod()]
            public void TestChangeStoF()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "Hello");
                s.SetContentsOfCell("A1", "23");
                Assert.AreEqual(new Formula("23"), s.GetCellContents("A1"));
                Assert.AreNotEqual(new Formula("24"), s.GetCellContents("A1"));
            }

            // STRESS TESTS
            [TestMethod()]
            public void TestStress1()
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "=B1+B2");
                s.SetContentsOfCell("B1", "=C1-C2");
                s.SetContentsOfCell("B2", "=C3*C4");
                s.SetContentsOfCell("C1", "=D1*D2");
                s.SetContentsOfCell("C2", "=D3*D4");
                s.SetContentsOfCell("C3", "=D5*D6");
                s.SetContentsOfCell("C4", "=D7*D8");
                s.SetContentsOfCell("D1", "=E1");
                s.SetContentsOfCell("D2", "=E1");
                s.SetContentsOfCell("D3", "=E1");
                s.SetContentsOfCell("D4", "=E1");
                s.SetContentsOfCell("D5", "=E1");
                s.SetContentsOfCell("D6", "=E1");
                s.SetContentsOfCell("D7", "=E1");
                s.SetContentsOfCell("D8", "=E1");
                HashSet<String> cells = new HashSet<String>(s.SetContentsOfCell("E1", "0"));
                Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
            }

            // Repeated for extra weight
            [TestMethod()]
            public void TestStress1a()
            {
                TestStress1();
            }
            [TestMethod()]
            public void TestStress1b()
            {
                TestStress1();
            }
            [TestMethod()]
            public void TestStress1c()
            {
                TestStress1();
            }

            [TestMethod()]
            public void TestStress2()
            {
                Spreadsheet s = new Spreadsheet();
                ISet<String> cells = new HashSet<string>();
                for (int i = 1; i < 200; i++)
                {
                    cells.Add("A" + i);
                    Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, ("=A" + (i + 1)))));
                }
            }
            [TestMethod()]
            public void TestStress2a()
            {
                TestStress2();
            }
            [TestMethod()]
            public void TestStress2b()
            {
                TestStress2();
            }
            [TestMethod()]
            public void TestStress2c()
            {
                TestStress2();
            }

            [TestMethod()]
            public void TestStress3()
            {
                Spreadsheet s = new Spreadsheet();
                for (int i = 1; i < 200; i++)
                {
                    s.SetContentsOfCell("A" + i, "=A" + (i + 1));
                }
                try
                {
                    s.SetContentsOfCell("A150", "=A50");
                    Assert.Fail();
                }
                catch (CircularException)
                {
                }
            }

            [TestMethod()]
            public void TestStress3a()
            {
                TestStress3();
            }
            [TestMethod()]
            public void TestStress3b()
            {
                TestStress3();
            }
            [TestMethod()]
            public void TestStress3c()
            {
                TestStress3();
            }

            [TestMethod()]
            public void TestStress4()
            {
                Spreadsheet s = new Spreadsheet();
                for (int i = 0; i < 500; i++)
                {
                    s.SetContentsOfCell("A1" + i, ("=A1" + (i + 1)));
                }
                HashSet<string> firstCells = new HashSet<string>();
                HashSet<string> lastCells = new HashSet<string>();
                for (int i = 0; i < 250; i++)
                {
                    firstCells.Add("A1" + i);
                    lastCells.Add("A1" + (i + 250));
                }
                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("A1249", "25.0")).SetEquals(firstCells));
                Assert.IsTrue(new HashSet<string>(s.SetContentsOfCell("A1499", "0")).SetEquals(lastCells));
            }
            [TestMethod()]
            public void TestStress4a()
            {
                TestStress4();
            }
            [TestMethod()]
            public void TestStress4b()
            {
                TestStress4();
            }
            [TestMethod()]
            public void TestStress4c()
            {
                TestStress4();
            }

            [TestMethod()]
            public void TestStress5()
            {
                RunRandomizedTest(47, 2519);
            }

            [TestMethod()]
            public void TestStress6()
            {
                RunRandomizedTest(48, 2521);
            }

            [TestMethod()]
            public void TestStress7()
            {
                RunRandomizedTest(49, 2526);
            }

            [TestMethod()]
            public void TestStress8()
            {
                RunRandomizedTest(50, 2521);
            }

            /// <summary>
            /// Sets random contents for a random cell 10000 times
            /// </summary>
            /// <param name="seed">Random seed</param>
            /// <param name="size">The known resulting spreadsheet size, given the seed</param>
            public void RunRandomizedTest(int seed, int size)
            {
                Spreadsheet s = new Spreadsheet();
                Random rand = new Random(seed);
                for (int i = 0; i < 10000; i++)
                {
                    try
                    {
                        switch (rand.Next(3))
                        {
                            case 0:
                                s.SetContentsOfCell(randomName(rand), "3.14");
                                break;
                            case 1:
                                s.SetContentsOfCell(randomName(rand), "hello");
                                break;
                            case 2:
                                s.SetContentsOfCell(randomName(rand), randomFormula(rand));
                                break;
                        }
                    }
                    catch (CircularException)
                    {
                    }
                }
                ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
                Assert.AreEqual(size, set.Count);
            }

            /// <summary>
            /// Generates a random cell name with a capital letter and number between 1 - 99
            /// </summary>
            /// <param name="rand"></param>
            /// <returns></returns>
            private String randomName(Random rand)
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
            }

            /// <summary>
            /// Generates a random Formula
            /// </summary>
            /// <param name="rand"></param>
            /// <returns></returns>
            private String randomFormula(Random rand)
            {
                String f = randomName(rand);
                for (int i = 0; i < 10; i++)
                {
                    switch (rand.Next(4))
                    {
                        case 0:
                            f += "+";
                            break;
                        case 1:
                            f += "-";
                            break;
                        case 2:
                            f += "*";
                            break;
                        case 3:
                            f += "/";
                            break;
                    }
                    switch (rand.Next(2))
                    {
                        case 0:
                            f += 7.2;
                            break;
                        case 1:
                            f += randomName(rand);
                            break;
                    }
                }
                return f;
            }

        }
    }
}
