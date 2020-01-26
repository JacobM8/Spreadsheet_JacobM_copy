﻿// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!// Version 1.1 (9/22/13 11:45 a.m.)
// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works
// (Daniel Kopta) 
// Version 1.2 (9/10/17) 
// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

/// <summary>
///     Author: Jacob Morrison
///     Date: 1/31/2020
///     update this - what the file does
///     I pledge that I did the work myself.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>  
    /// Represents formulas written in standard infix notation using standard precedence  
    /// rules. The allowed symbols are non-negative numbers written using double-precision   
    /// floating-point syntax (without unary preceeding '-' or '+');   
    /// variables that consist of a letter or underscore followed by   
    /// zero or more letters, underscores, or digits; parentheses; and the four operator   
    /// symbols +, -, *, and /.    
    ///   
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is  
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;   
    /// and "x 23" consists of a variable "x" and a number "23".  
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The  
    /// normalizer is used to convert variables into a canonical form, and the validator is used  
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement   
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,  
    /// or digits.)  Their use is described in detail in the constructor and method comments.  
    /// </summary>  
    public class Formula
    {
        /// <summary>    
        /// Creates a Formula from a string that consists of an infix expression written as    
        /// described in the class comment.  If the expression is syntactically invalid,    
        /// throws a FormulaFormatException with an explanatory Message.    
        ///     
        /// The associated normalizer is the identity function, and the associated validator    
        /// maps every string to true.      
        /// </summary>    
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            // this constructor takes the 'String formula' parameter and passes it into the 
            // other constructor that has two func's as parameters
        }

        /// <summary>    
        /// Creates a Formula from a string that consists of an infix expression written as    
        /// described in the class comment.  If the expression is syntactically incorrect,    
        /// throws a FormulaFormatException with an explanatory Message.    
        ///     
        /// The associated normalizer and validator are the second and third parameters,    
        /// respectively.      
        ///     
        /// If the formula contains a variable v such that normalize(v) is not a legal variable,     
        /// throws a FormulaFormatException with an explanatory message.     
        ///     
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,    
        /// throws a FormulaFormatException with an explanatory message.    
        ///     
        /// Suppose that N is a method that converts all the letters in a string to upper case, and    
        /// that V is a method that returns true only if a string consists of one letter followed    
        /// by one digit.  Then:    
        ///     
        /// new Formula("x2+y3", N, V) should succeed    
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false    
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.    
        /// </summary>    
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            // 9.1 in assignment rules
            string normalForm = normalize(formula);
            bool isValidForm = isValid(normalForm);
            // If formula is not a valid form throw the appropriate exception
            if (!isValidForm)
            {
                double number;
                String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
                int invalidTokenCount = 0;
                int leftParenCount = 0;
                int rightParenCount = 0;
                string[] invalidtokens = new string[GetTokens(normalForm).Count()];
                
                foreach (string s in GetTokens(normalForm)){
                    invalidtokens[invalidTokenCount] = s;
                    

                    if (!isValid(s))
                    {
                        throw new FormulaFormatException("the only valid tokens are (, ), +, -, *, /, " +
                            "variables, and decimal real numbers (including scientific notation)");
                    }
                    if (s == "(")
                    {
                        leftParenCount++;
                    }
                    if (s == ")")
                    {
                        rightParenCount++;
                    }
                    if (rightParenCount > leftParenCount)
                    {
                        throw new FormulaFormatException("When reading tokens from left to right, at " +
                            "no point should the number of closing parentheses seen so far be greater " +
                            "than the number of opening parentheses seen so far.");
                    }
                    if (invalidtokens[0] != "(" || !double.TryParse(s, out number) || 
                        invalidtokens[0] != varPattern)
                    {
                        throw new FormulaFormatException("The first token of an expression must be a number," +
                            " a variable, or an opening parenthesis.");
                    }
                    if (invalidtokens.op)

                    invalidTokenCount++;
                }
                // these execeptions are checked outside of the loop because the loops need to be completed
                // before they can be checked.
                if (invalidTokenCount < 1)
                {
                    throw new FormulaFormatException("There must be at least one token");
                }
                if (leftParenCount != rightParenCount)
                {
                    throw new FormulaFormatException("The total number of opening parentheses must " +
                        "equal the total number of closing parentheses.");
                }
                if (invalidtokens[invalidTokenCount] != ")" || 
                    !double.TryParse(invalidtokens[invalidTokenCount], out number) || 
                    invalidtokens[0] != varPattern)
                {
                    throw new FormulaFormatException("The last token of an expression must be a number," +
                        " a variable, or an closing parenthesis.");
                }

            }

        }

        /// <summary>    
        /// Evaluates this Formula, using the lookup delegate to determine the values of    
        /// variables.  When a variable symbol v needs to be determined, it should be looked up    
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to     
        /// the constructor.)    
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters     
        /// in a string to upper case:    
        ///     
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11    
        /// new Formula("x+7").Evaluate(L) is 9    
        ///     
        /// Given a variable symbol as its parameter, lookup returns the variable's value     
        /// (if it has one) or throws an ArgumentException (otherwise).    
        ///     
        /// If no undefined variables or divisions by zero are encountered when evaluating     
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.      
        /// The Reason property of the FormulaError should have a meaningful explanation.    
        ///    
        /// This method should never throw an exception.    
        /// </summary>  
        public object Evaluate(Func<string, double> lookup)
        {
            // see section 10.2 in assignment
            // the object in the method header is saying it is a method that will return an object
            // if I had 'return lookup' it would lookup the value associated with the string and return it as a double

            return null;
        }

        /// <summary>    
        /// Enumerates the normalized versions of all of the variables that occur in this     
        /// formula.  No normalization may appear more than once in the enumeration, even     
        /// if it appears more than once in this Formula.    
        ///     
        /// For example, if N is a method that converts all the letters in a string to upper case:    
        ///     
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"    
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".    
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".    
        /// </summary> 
        public IEnumerable<String> GetVariables()
        {
            return null;
        }

        /// <summary>    
        /// Returns a string containing no spaces which, if passed to the Formula    
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the    
        /// variables in the string should be normalized.    
        ///     
        /// For example, if N is a method that converts all the letters in a string to upper case:    
        ///     
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"    
        /// new Formula("x + Y").ToString() should return "x+Y"    
        /// </summary>    
        public override string ToString()
        {
            return null;
        }

        /// <summary>    
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports    
        /// whether or not this Formula and obj are equal.    
        ///     
        /// Two Formulae are considered equal if they consist of the same tokens in the    
        /// same order.  To determine token equality, all tokens are compared as strings     
        /// except for numeric tokens and variable tokens.    
        /// Numeric tokens are considered equal if they are equal after being "normalized"     
        /// by C#'s standard conversion from string to double, then back to string. This     
        /// eliminates any inconsistencies due to limited floating point precision.    
        /// Variable tokens are considered equal if their normalized forms are equal, as     
        /// defined by the provided normalizer.    
        ///     
        /// For example, if N is a method that converts all the letters in a string to upper case:    
        ///      
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true    
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false    
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false    
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true    
        /// </summary>    
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>    
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.    
        /// Note that if both f1 and f2 are null, this method should return true.  If one is    
        /// null and one is not, this method should return false.    
        /// </summary>    
        public static bool operator ==(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>    
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.    
        /// Note that if both f1 and f2 are null, this method should return false.  If one is    
        /// null and one is not, this method should return true.    
        /// </summary>    
        public static bool operator !=(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>    
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the    
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two     
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.    
        /// </summary>    
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>    
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;    
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore    
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't    
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.    
        /// </summary>    
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens      
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";
            // Overall pattern      
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);
            // Enumerate matching tokens that don't consist solely of white space.      
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>  
    /// Used to report syntactic errors in the argument to the Formula constructor.  
    /// </summary>  
    public class FormulaFormatException : Exception
    {
        /// <summary>    
        /// Constructs a FormulaFormatException containing the explanatory message.    
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>  
    /// Used as a possible return value of the Formula.Evaluate method.  
    /// </summary>  
    public struct FormulaError
    {
        /// <summary>    
        /// Constructs a FormulaError containing the explanatory reason.    
        /// </summary>    
        /// <param name="reason"></param>    
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>    
        ///  The reason why this FormulaError was created.    
        /// </summary>    
        public string Reason
        {
            get; private set;
        }

        // extensions
        /// <summary>
        /// Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
        /// parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="leftParen"></param>
        /// <param name="plus"></param>
        /// <param name="minus"></param>
        /// <param name="multiply"></param>
        /// <param name="divide"></param>
        /// <returns> true or false </returns>
        public static bool OpenPerenOrOperator(this string[] s, string leftParen, string plus, 
            string minus, string multiply, string divide)
        {
            // used to check for a numbrer
            double number;
            // used to check for a variable
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            for (int i = 0; i < s.Length - 1; i++)
            {
                // Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
                // parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
                if ((s[i] == leftParen || s[i] == plus || s[i] == minus || s[i] == multiply || s[i] == divide)
                    && (double.TryParse(s[i + 1], out number) || s[i + 1] == varPattern || s[i + 1] == leftParen)){

                    return true;
                }
            }
            return false;
        }
    }
}