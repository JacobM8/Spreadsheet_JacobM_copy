using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
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
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
            Assert.IsNotNull(f);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCanCatchInvalidChar()
        {
            string formula = "2 + x^";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyFormula()
        {
            string formula = "";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        // test number with a decimal
        [TestMethod]
        public void TestSimpleAddWithDecimal()
        {
            // can create a helper method to get the value of the 
            string formula = "2.2 + 2.2";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
            Assert.AreEqual("4.4", f.Evaluate(s => 0));
        }

        // test scientific notation
        [TestMethod]
        public void TestSimpleAddWithScientificNotation()
        {
            string formula = "5e-5 + 5e-5";
            Console.WriteLine(formula);
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
            Console.WriteLine(formula);
            // problem with regex to catch !
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestSpecificTokenRulePass()
        {
            // Specific Token Rule - the only valid tokens are (, ), +, -, *, /, variables, and decimal 
            // real numbers (including scientific notation).
            string formula = "(3 + 3 - 3)";
            Console.WriteLine(formula);
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
            Console.WriteLine(formula);
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
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBalancedParenRuleFail()
        {
            // Balanced Parentheses Rule- The total number of opening parentheses must equal the total 
            // number of closing parentheses.
            string formula = "(3 + 3) * (5 - 3(";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
            //f.leftParenCount;
        }

        [TestMethod]
        public void TestBalancedParenRulePass()
        {
            // Balanced Parentheses Rule- The total number of opening parentheses must equal the total 
            // number of closing parentheses.
            string formula = "(3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingTokenRuleFail()
        {
            // Starting Token Rule - The first token of an expression must be a number, a variable, or 
            // an opening parenthesis.
            string formula = "*3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestStartingTokenRulePass()
        {
            // Starting Token Rule - The first token of an expression must be a number, a variable, or 
            // an opening parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEndTokenRuleFail()
        {
            // Ending Token Rule- The last token of an expression must be a number, a variable, or a 
            // closing parenthesis.
            string formula = "(3 + 3) * (5 - 3+";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestEndTokenRulePass()
        {
            // Ending Token Rule- The last token of an expression must be a number, a variable, or a 
            // closing parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenOperatorFollowRuleFail()
        {
            // Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
            // parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
            string formula = "(3 + ) 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestParenOperatorFollowRulePass()
        {
            // Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
            // parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
            string formula = "(3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraFollowRuleFail()
        {
            // Extra Following Rule - Any token that immediately follows a number, a variable, or a 
            // closing parenthesis must be either an operator or a closing parenthesis.

            string formula = "(3 + 3 ( ) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }

        [TestMethod]
        public void TestExtraFollowRulePass()
        {
            // Extra Following Rule - Any token that immediately follows a number, a variable, or a 
            // closing parenthesis must be either an operator or a closing parenthesis.

            string formula = "(3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
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
