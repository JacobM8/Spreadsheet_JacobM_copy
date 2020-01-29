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
            string formula = "2 + x&";
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

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestRightParenRule()
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
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBalancedParenRule()
        {
            // Balanced Parentheses Rule- The total number of opening parentheses must equal the total 
            // number of closing parentheses.
            string formula = "(3 + 3) * (5 - 3(";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
            //f.leftParenCount;
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingTokenRule()
        {
            // Starting Token Rule - The first token of an expression must be a number, a variable, or 
            // an opening parenthesis.
            string formula = "*3 + 3) * (5 - 3)";
            Console.WriteLine(formula);
            Formula f = new Formula(formula, normalize, isValid);
        }



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
