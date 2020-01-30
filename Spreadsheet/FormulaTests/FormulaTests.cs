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
        // the following tests ensure the formula is valid and the correct exception is thrown
        [TestMethod]
        public void TestConstructorIsNotNull()
        {
            string formula = "2 + 2";
            Formula f = new Formula(formula, normalize, isValid);
            Assert.IsNotNull(f);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCanCatchInvalidChar()
        {
            string formula = "2 + x^";
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
        // can create a helper method to get the value of the variable or use lambda
        [TestMethod]
        public void TestSimpleAddWithScientificNotation()
        {
            string formula = "5e-5 + 5e-5";
            Formula f = new Formula(formula, normalize, isValid);
            //Assert.Equals(".0001", f.Evaluate());
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSpecificTokenRuleFail()
        {
            // Specific Token Rule - the only valid tokens are (, ), +, -, *, /, variables, and 
            // decimal real numbers (including scientific notation).
            string formula = "(3! + 3 - 3)";
            // problem with regex to catch !
            Formula f = new Formula(formula, normalize, isValid);
        }

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

        // Helper methods
        /// <summary>
        /// convert every char in "toTest" toUpper 
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns> toTest with all chars in upper case </returns>
        private string normalize(string toTest)
        {
            toTest.ToUpper();
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
