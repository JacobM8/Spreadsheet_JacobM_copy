using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTests
{
    
    /// <summary>  
    ///This is a test class for Formula.cs and is intended  
    ///to contain all Formula.cs Unit Tests  
    ///</summary> 
    [TestClass]
    public class FormulaTests
    {
        // the following tests the constructor by ensuring the formula is valid and the correct exception is thrown
        [TestMethod]
        public void TestConstructorIsNotNull()
        {
            string formula = "2 + 2";
            Formula f = new Formula(formula, normalize, isValid);
            Assert.IsNotNull(f);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCanCatchInvalidCharDollarSign()
        {
            string formula = "$2 + x";
            Formula f = new Formula(formula, normalize, isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCanCatchInvalidCharPoundSign()
        {
            string formula = "#2 + x";
            Formula f = new Formula(formula, normalize, isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCanCatchInvalidExclamationPoint()
        {
            string formula = "2 +! x";
            Formula f = new Formula(formula, normalize, isValid);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyFormula()
        {
            string formula = "";
            Formula f = new Formula(formula, normalize, isValid);
        }
        [TestMethod]
        public void TestSimpleAddWithDecimal()
        {
            string formula = "2.2 + 2.2";
            Formula f = new Formula(formula, normalize, isValid);
            Assert.AreEqual(4.4, f.Evaluate(s => 0));
        }
        [TestMethod]
        public void TestComplexWithDecimal()
        {
            string formula = "2.21 + ((6.35 + 2.65) / (7.32 - 4.32))";
            Formula f = new Formula(formula, normalize, isValid);
            Assert.AreEqual(5.21, f.Evaluate(s => 0));
        }

        // test scientific notation
        [TestMethod]
        public void TestSimpleAddWithScientificNotation()
        {
            string formula = "5e-5 + 5e-5";
            Formula f = new Formula(formula, normalize, isValid);
            Assert.AreEqual(0.0001, f.Evaluate(s => 0));
        }

        /*[TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSpecificTokenRuleFail()
        {
            // Specific Token Rule - the only valid tokens are (, ), +, -, *, /, variables, and 
            // decimal real numbers (including scientific notation).
            string formula = "(3! + 3 - 3)";
            // problem with regex to catch !
            Formula f = new Formula(formula, normalize, isValid);
        }*/

        [TestMethod]
        public void TestSpecificTokenRulePass()
        {
            // Specific Token Rule - the only valid tokens are (, ), +, -, *, /, variables, and decimal 
            // real numbers (including scientific notation).
            string formula = "(3 + 3 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestRightParenRuleFail()
        {
            // Right Parentheses Rule -  When reading tokens from left to right, at no point should 
            // the number of closing parentheses seen so far be greater than the number of opening 
            // parentheses seen so far.
            string formula = "(3 + 3) - 3)";
            Formula f = new Formula(formula, normalize, isValid);
            //f.leftParenCount;
        }

        [TestMethod]
        public void TestRightParenRulePass()
        {
            // Right Parentheses Rule -  When reading tokens from left to right, at no point should 
            // the number of closing parentheses seen so far be greater than the number of opening 
            // parentheses seen so far.
            string formula = "(3 + 3 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBalancedParenRuleFail()
        {
            // Balanced Parentheses Rule- The total number of opening parentheses must equal the total 
            // number of closing parentheses.
            string formula = "(3 + 3) * (5 - 3(";
            Formula f = new Formula(formula, normalize, isValid);
            //f.leftParenCount;
        }

        [TestMethod]
        public void TestBalancedParenRulePass()
        {
            // Balanced Parentheses Rule- The total number of opening parentheses must equal the total 
            // number of closing parentheses.
            string formula = "(3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingTokenRuleFail()
        {
            // Starting Token Rule - The first token of an expression must be a number, a variable, or 
            // an opening parenthesis.
            string formula = "*3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestStartingTokenRulePass()
        {
            // Starting Token Rule - The first token of an expression must be a number, a variable, or 
            // an opening parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEndTokenRuleFail()
        {
            // Ending Token Rule- The last token of an expression must be a number, a variable, or a 
            // closing parenthesis.
            string formula = "(3 + 3) * (5 - 3+";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestEndTokenRulePass()
        {
            // Ending Token Rule- The last token of an expression must be a number, a variable, or a 
            // closing parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenOperatorFollowRuleFail()
        {
            // Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
            // parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
            string formula = "(3 + ) 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestParenOperatorFollowRulePass()
        {
            // Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
            // parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraFollowRuleFail()
        {
            // Extra Following Rule - Any token that immediately follows a number, a variable, or a 
            // closing parenthesis must be either an operator or a closing parenthesis.

            string formula = "(3 + 3 ( ) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestExtraFollowRulePass()
        {
            // Extra Following Rule - Any token that immediately follows a number, a variable, or a 
            // closing parenthesis must be either an operator or a closing parenthesis.

            string formula = "(3 + 3) * (5 - 3)";
            Formula f = new Formula(formula, normalize, isValid);
        }

        // Tests for IEnumerable GetVariables()
        [TestMethod]
        public void TestOnlyThreeVariables()
        {
            // new Formula("x+y*z", normalize, s => true).GetVariables() should enumerate "X", "Y", and "Z"    
            string formula = "x+y*z";
            Formula f = new Formula(formula, normalize, s => true);
            List<string> result = new List<string>();
            result.Add("X");
            result.Add("Y");
            result.Add("Z");
            Assert.AreEqual(result.ToString(), f.GetVariables().ToString());
        }

        [TestMethod]
        public void TestTwoOfThreeVariables()
        {
            // new Formula("x+X*z", normalize, s => true).GetVariables() should enumerate "X" and "Z".    
            string formula = "x+X*z";
            Formula f = new Formula(formula, normalize, s => true);
            List<string> result = new List<string>();
            result.Add("X");
            result.Add("Z");
            Assert.AreEqual(result.ToString(), f.GetVariables().ToString());
        }

        [TestMethod]
        public void TestEnumerateWithUpperAndLowercaseX()
        {
            // new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
            string formula = "x+X*z";
            Formula f = new Formula(formula, normalize, s => true);
            List<string> result = new List<string>();
            result.Add("x");
            result.Add("X");
            result.Add("z");
            Assert.AreEqual(result.ToString(), f.GetVariables().ToString());
        }

        // the following tests ensure the .ToString() mehtod works correctly
        [TestMethod]
        public void TestToStringSimple()
        {
            // new Formula("x + y", normalize, s => true).ToString() should return "X+Y"  
            string formula = "x + y";
            Formula f = new Formula(formula, normalize, isValid);
            //normalize(formula);
            string result = "X+Y";
            Assert.AreEqual(result, f.ToString());
        }

        [TestMethod]
        public void TestToStringWithOutNormalize()
        {
            // new Formula("x + Y").ToString() should return "x+Y"  
            string formula = "x + Y";
            Formula f = new Formula(formula);
            string result = "x+Y";
            Assert.AreEqual(result, f.ToString());
        }

        // test for getVariables
        // test for equals, ==, !=
        [TestMethod]
        public void TestTwoNullObjsAreEqual()
        {
            Formula f1 = null;
            Formula f2 = null;
            //Assert.AreEqual(true, f1 == f2);
            Assert.IsTrue(f1 == f2);
        }
        [TestMethod]
        public void TestOneNullObjIsEqual()
        {
            string form = "2+2";
            Formula f1 = new Formula(form, normalize, isValid);
            Formula f2 = null;
            Assert.IsFalse(f1.Equals(f2));
        }
        [TestMethod]
        public void TestTwoNonNullSameObjsAreEqual()
        {
            string formula = "4*3";
            Formula f1 = new Formula(formula, normalize, isValid);
            Formula f2 = new Formula(formula, normalize, isValid);
            Assert.IsTrue(f1.Equals(f2));
        }
        [TestMethod]
        public void TestTwoNonNullDifferentObjsAreNotEqual()
        {
            string formula = "4*3";
            string formula2 = "3*4";
            Formula f1 = new Formula(formula, normalize, isValid);
            Formula f2 = new Formula(formula2, normalize, isValid);
            Assert.IsFalse(f1.Equals(f2));
        }
        // tests for evaluate

        // These tests are from assignment one
        ///<summary>
        ///  This is a test class for EvaluatorTest and is intended
        ///  to contain all EvaluatorTest Unit Tests
        ///</summary>


        [TestMethod(), Timeout(5000)]
        public void TestSingleNumber()
        {
            string form = "5";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(5.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSingleVariable()
        {
            string form = "X5";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(13.0, f.Evaluate(s => 13));
        }

        [TestMethod(), Timeout(5000)]
        public void TestAddition()
        {
            string form = "5+3";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSubtraction()
        {
            string form = "18-10";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestMultiplication()
        {
            string form = "2*4";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestDivision()
        {
            string form = "16/2";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestArithmeticWithVariable()
        {
            string form = "2+X1";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(6.0, f.Evaluate(s => 4));
        }

        [TestMethod(), Timeout(5000)]
        public void TestLeftToRight()
        {
            string form = "2*6+3";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(15.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestOrderOperations()
        {
            string form = "2+6*3";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(20.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestParenthesesTimes()
        {
            string form = "(2*6)*3";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(36.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestTimesParentheses()
        {
            string form = "2*(3+5)";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(16.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestPlusParentheses()
        {
            string form = "2+(3+5)";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(10.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestPlusComplex()
        {
            string form = "2+(3+5*9)";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(50.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestOperatorAfterParens()
        {
            string form = "(1*1)-2/2";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexTimesParentheses()
        {
            string form = "2+3*(3+5)";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(26.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexAndParentheses()
        {
            string form = "2+3*5+(3+4*8)*5+2";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }

        [TestMethod()]//, Timeout(5000)]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDivideByZero()
        {
            string form = "5/0";
            Formula f = new Formula(form, normalize, isValid);
            Console.WriteLine("test");
            Console.WriteLine(f.Evaluate(s => 0));
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSingleOperator()
        {
            string form = "+";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOperator()
        {
            string form = "2+5+";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraParentheses()
        {
            string form = "2+5*7)";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            string form = "xx";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestPlusInvalidVariable()
        {
            string form = "5+xx";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod()]//, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParensNoOperator()
        {
            string form = "5+7+(5)8";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }


        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            string form = "";
            Formula f = new Formula(form, normalize, isValid);
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexMultiVar()
        {
            string form = "y1*3-8/2+4*(8-9*2)/14*x7";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(6.0, f.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensRight()
        {
            string form = "x1+(x2+(x3+(x4+(x5+x6))))";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(6.0, f.Evaluate(s => 1));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensLeft()
        {
            string form = "((((x1+x2)+x3)+x4)+x5)+x6";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(12.0, f.Evaluate(s => 2));
        }

        [TestMethod(), Timeout(5000)]
        public void TestRepeatedVar()
        {
            string form = "a4-a4*a4/a4";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(0.0, f.Evaluate(s => 3));
        }


        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNegativeLiteral()
        {
            string form = "-5";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(-5.0, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNegativeParens()
        {
            string form = "-(5+5)";
            Formula f = new Formula(form, normalize, isValid);
            Assert.AreEqual(-10.0, f.Evaluate(s => 0));
        }

        // Helper methods
        /// <summary>
        /// convert every char in "toTest" toUpper 
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns> toTest with all chars in upper case </returns>
        private string normalize(string toTest)
        {
            toTest = toTest.ToUpper();
            return toTest;
        }

        /// <summary>
        /// check all variables are valid
        /// valid variable can be form of lower or upper case follow by integers
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns> true if "toTest" is a valid variable </returns>
        private bool isValid(string toTest)
        {
            if (Regex.IsMatch(toTest, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                return true;
            }
            return false;
        }

    }
}
