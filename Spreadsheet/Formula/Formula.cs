// Skeleton written by Joe Zachary for CS 3500, September 2013
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>  
    /// Represents formulas written in standard infix notation using standard precedence  
    /// rules. The allowed symbols are non-negative numbers written using double-precision   
    /// doubleing-point syntax (without unary preceeding '-' or '+');   
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
        static Stack<double> valueStack;
        static Stack<string> operatorStack;

        // tokenValue will be set to the value of token when it's a doubleing point value in TryPase below
        static double tokenValue = 0;
        // Final value of formula
        static double finalValue = 0;
        // master formula
        //public string[] masterFormula;
        private string finalFormula;

        public delegate int Lookup(String variable_name);
        //public Func<string, string> normalize = delegate (string s)
        //    { return s.ToUpper(); };
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
            if (formula.Equals("") || formula.Equals(null))
            {
                throw new FormulaFormatException("Formula cannot be empty or null");
            }
            // variables are incremented appropiately to check for exceptions
            int tokenCount = 0;
            int leftParenCount = 0;
            int rightParenCount = 0;
            double scientificNum = 0;

            // the following foreach loop and if statements will ensure there is a valid formula
            // checks will not catch errors like divide by 0
            foreach (string s in GetTokens(formula)){
                string temp = s;
                

                if (s == "(")
                {
                    leftParenCount++;
                }
                if (s == ")")
                {
                    rightParenCount++;
                }
                // ensures formula has correct ordering of parenthesis
                if (rightParenCount > leftParenCount)
                {
                    throw new FormulaFormatException("When reading tokens from left to right, at " +
                        "no point should the number of closing parentheses seen so far be greater " +
                        "than the number of opening parentheses seen so far.");
                }
                
                // checks token to ensure there is no invalid operand  
                if (Regex.IsMatch(s, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
                {
                    if (!isValid(s))
                    {
                        throw new FormulaFormatException("the only valid tokens are (, ), +, -, *, /, " +
                            "variables, and decimal real numbers (including scientific notation)");
                    }
                    // if variable is in scientific notation convert to double then call ToString to add it to finalFormula
                    //normalize if its a variable
                    temp = normalize(s);
                }
                // convert scientific notation to regular number
                if (!s.IsOperator())
                {
                    NumberFormatInfo info;
                    info = NumberFormatInfo.CurrentInfo;
                    Decimal num;
                    Decimal.TryParse(s, System.Globalization.NumberStyles.Float, info, out num);
                    temp = num.ToString();
                }
                // add to final formula
                finalFormula += temp;

                tokenCount++;
            }
            // these execeptions are checked outside of the loop because the loop need to be completed
            // before they can be checked.

            // ensures proper tokens follow a "(" or operator
            if (!finalFormula.WhenOpenPerenOrOperator())
            {
                throw new FormulaFormatException("Any token that immediately follows an opening parenthesis " +
                    "or an operator must be either a number, a variable, or an opening parenthesis.");
            }
            // ensures proper token follow a number, variable, or ")"
            if (!finalFormula.WhenNumOrVarOrCloseParen())
            {
                throw new FormulaFormatException("Any token that immediately follows a number, a variable, or " +
                    "a closing parenthesis must be either an operator or a closing parenthesis.");
            }
            // ensures formula starts with correct tokens
             if (!StartOrEndTokenRule(finalFormula[0].ToString(), "("))
            {
                throw new FormulaFormatException("The first token of an expression must be a number, a " +
                    "variable, or an opening parenthesis.");
            }
            // ensures formula ends with correct tokens
            if (!StartOrEndTokenRule(finalFormula[tokenCount - 1].ToString(), ")"))
            {
                throw new FormulaFormatException("The last token of an expression must be a number," +
                    " a variable, or an closing parenthesis.");
            }
            // ensures there is at least one valid token in formula
            if (tokenCount < 1)
            {
                throw new FormulaFormatException("There must be at least one token");
            }
            // ensures formula has same number of open and close parenthesis
            if (leftParenCount != rightParenCount)
            {
                throw new FormulaFormatException("The total number of opening parentheses must " +
                    "equal the total number of closing parentheses.");
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
            // example of lookup being used is lookup("string"), it will lookup the string and see if it 
            // can convert it into a double.

            // the object in the method header is saying it is a method that will return an object
            // if I had 'return lookup' it would lookup the value associated with the string and return it as a double

            // The Evaluate method should use the evaluation algorithm from the first assignment, 
            // modified to deal with double-precision doubleing point numbers instead of integers and 
            // to take account of the normalization delegate (see below).
            //
            // Because the constructor throws an exception when it encounters a syntactically incorrect 
            // formula, the only problems that the Evaluate method needs to worry about are undefined 
            // variables and division by zero.
            
            valueStack = new Stack<double>();
            operatorStack = new Stack<string>();

            // for loop through masterFormula
            foreach (string token in GetTokens(finalFormula))
            {
                // if token is blank
                if (token.Equals("") || token.Equals(" "))
                {
                    continue;
                }

                // trim whitespaces off front and back of the token
                token.Trim();

                // if token is an doubleing point
                if (double.TryParse(token, out tokenValue))
                {
                    IsDouble(tokenValue);
                }

                // ***if token is in scientific notation
                // use regex to see if it's a double
                // convert to double with Double.TryParse("scientific notation number", System.Globalization.NumberStyles.double, out Mydouble);
                // then call Isdouble()


                // if token is a "+" or "-"
                else if (token == "+" || token == "-")
                {
                    IsPlusOrMinus(token);
                }
                // if token is a "*" or "/"
                else if (token == "*" || token == "/")
                {
                    IsMultiplyOrDivide(token);
                }
                // if token is left parenthesis "("
                else if (token == "(")
                {
                    IsLeftParenthesis(token);
                }
                // if token is right parenthesis
                else if (token == ")")
                {
                    IsRightParenthesis();
                }
                // if token is a variable
                else
                {
                    // checks to see if token is a valid variable
                    if (Regex.IsMatch(token, @"[a-zA-Z]+\d+"))
                    {
                        // try catch will catch an exception when there is one from Program.cs
                        try
                        {
                            double variableValue = lookup(token);
                            IsDouble(variableValue);
                        }
                        catch
                        {
                            throw new ArgumentException("Lookup function threw exception.");
                        }
                    }
                }
            }

            // if operatorStack is empty
            if (operatorStack.Count == 0)
            {
                OpStackEmpty();
            }
            // if operatorStack has one item left
            else if (operatorStack.Count == 1)
            {
                OpStackOneRemaining();
            }

            return finalValue;
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
            List<string> returnList = new List<string>();
            // check if each token is a variable, if it is compare to masterFormula, if it is lower case
            // "normalize" it by converting to upper case.
            foreach (string s in GetTokens(finalFormula))
            {
                returnList.Add(s);
            }
            // return the items that were enumerated
            return returnList;
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
            return finalFormula;
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
        /// eliminates any inconsistencies due to limited doubleing point precision.    
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
        // helper methods
        /// <summary>
        /// Determines if token is an integer and does the following:
        /// If * or / is at the top of the operator stack, pop the value stack, pop the operator stack, 
        /// and apply the popped operator to the popped number and t. Push the result onto the value stack.
        /// Otherwise, pushes token onto the value stack.
        /// </summary>
        /// <param name="s"> integer </param>
        private static void IsDouble(double tokenValue)
        {
            // If * or / is at the top of the operator stack, pop the value stack, pop the operator stack, 
            // and apply the popped operator to the popped number and tokenValue. 
            // Push the result onto the value stack.
            if (operatorStack.HasOnTop("*", "/"))
            {
                if (valueStack.Count == 0)
                {
                    throw new ArgumentException("Invalid formula, valueStack is empty");
                }
                double operand = valueStack.Pop();
                string op = operatorStack.Pop();

                if (op == "*")
                {
                    valueStack.Push(operand * tokenValue);
                }
                else
                {
                    if (tokenValue == 0)
                    {
                        throw new ArgumentException("Invalid formula, Cannot divide by zero");
                    }
                    valueStack.Push(operand / tokenValue);
                }
            }
            // Otherwise, push tokenValue onto the value stack.
            else
            {
                valueStack.Push(tokenValue);
            }

        }

        /// <summary>
        /// Determines if token is "+" or "-" and does the following:
        /// If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once, 
        /// then apply the popped operator to the popped numbers, then push the result onto the value stack.
        /// Otherwise, Pushes token onto the operator stack
        /// </summary>
        /// <param name="token"> string </param>
        private static void IsPlusOrMinus(string token)
        {
            //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack 
            // once, then apply the popped operator to the popped numbers, then push the result onto the value 
            // stack.
            if (operatorStack.HasOnTop("+", "-"))
            {
                if (valueStack.Count < 2)
                {
                    throw new ArgumentException("Invalid formula, ValueStack has less than 2 tokens.");
                }

                double operand1 = valueStack.Pop();
                double operand2 = valueStack.Pop();
                string currOperator = operatorStack.Pop();
                if (currOperator == "+")
                {
                    valueStack.Push(operand1 + operand2);
                    operatorStack.Push(token);
                }
                else
                {
                    valueStack.Push(operand2 - operand1);
                    operatorStack.Push(token);
                }
            }
            // Push token onto the operator stack
            else
            {
                operatorStack.Push(token);
            }
        }

        /// <summary>
        ///  Determines if token is "*" or "/" and does the following: Push token onto the operator stack.
        /// </summary>
        /// <param name="token"> string </param>
        private static void IsMultiplyOrDivide(string token)
        {
            // Push token onto the operator stack
            operatorStack.Push(token);
        }

        /// <summary>
        /// Determines if token is "(" and does the following: Push t onto the operator stack.
        /// </summary>
        /// <param name="token"> string </param>
        private static void IsLeftParenthesis(string token)
        {
            // Push token onto the operator stack
            operatorStack.Push(token);
        }

        /// <summary>
        /// Dermines if token is ")" and does the following: If + or - is at the top of the operator stack, pop the 
        /// value stack twice and the operator stack once. Apply the popped operator to the popped numbers. Push the 
        /// result onto the value stack.
        /// 
        /// Next, the top of the operator stack should be a '('. Pop it.
        /// 
        /// Finally, if * or / is at the top of the operator stack, pop the value stack twice and the operator 
        /// stack once. Apply the popped operator to the popped numbers. Push the result onto the value stack.
        /// </summary>
        /// <param name="token"> string </param>
        private static void IsRightParenthesis()
        {
            if (valueStack.Count < 2 && !operatorStack.Contains("("))
            {
                throw new ArgumentException("Invalid formula, valueStack.Count > 2 when using IsRightParenthesis");
            }
            // If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once
            // Apply the popped operator to the popped numbers. Push the result onto the value stack.
            if (operatorStack.HasOnTop("+", "-"))
            {
                double operand1 = valueStack.Pop();
                double operand2 = valueStack.Pop();
                string currOperator1 = operatorStack.Pop();
                if (currOperator1 == "+")
                {
                    valueStack.Push(operand1 + operand2);
                }
                else
                {
                    valueStack.Push(operand2 - operand1);
                }

                // in order to calculate correctly the operatorStack should not be empty and
                // it needs to have "(" on top.
                if (operatorStack.Count == 0 || operatorStack.Peek() != "(")
                {
                    throw new ArgumentException("Invalid formula, '(' is not in the correct spot");
                }
            }
            // The top of the operator stack should be a '('. Pop it.
            string currOperator2 = operatorStack.Pop();

            // Finally, if * or / is at the top of the operator stack, pop the value stack twice and the 
            // operator stack once. Apply the popped operator to the popped numbers. Push the result onto 
            // the value stack.
            if (operatorStack.HasOnTop("*", "/"))
            {
                if (valueStack.Count < 2)
                {
                    throw new ArgumentException("Invalid formula, valueStack.Count < 2 when using IsRightParenthesis");
                }
                double operand3 = valueStack.Pop();
                double operand4 = valueStack.Pop();
                string currOperator3 = operatorStack.Pop();
                if (currOperator3 == "*")
                {
                    valueStack.Push(operand3 * operand4);
                }
                else
                {
                    if (operand4 == 0)
                    {
                        throw new ArgumentException("Invalid formula, cannot divide by zero.");
                    }
                    valueStack.Push(operand4 / operand3);
                }
            }
        }

        /// <summary>
        /// Value stack should contain a single number, pop it and report as the value of the expression
        /// </summary>
        /// <returns> returns finalValue of formula </returns>
        private static void OpStackEmpty()
        {
            //Value stack should contain a single number
            if (valueStack.Count != 1)
            {
                throw new ArgumentException("Invalid formula, valueStack.Count != 1");
            }
            //Pop it and report as the value of the expression
            finalValue = valueStack.Pop();
        }

        /// <summary>
        /// There should be exactly one operator on the operator stack, and it should be either + or -. 
        /// There should be exactly two values on the value stack. Apply the operator to the two values and 
        /// report the result as the value of the expression.
        /// </summary>
        /// <returns> returns finalValue of formula </returns>
        private static void OpStackOneRemaining()
        {
            // There should be exactly one operator on the operator stack, and it should be either + or -. There 
            // should be exactly two values on the value stack.
            if (operatorStack.Count != 1)
            {
                throw new ArgumentException("Invalid formula, operatorStack.Count != 1");
            }
            else if (valueStack.Count != 2)
            {
                throw new ArgumentException("Invalid formula, valueStack.Count != 2");
            }
            if (!operatorStack.HasOnTop("+", "-"))
            {
                throw new ArgumentException("Invalid formula, operatorStack does not have '+' or '-' as last token");
            }

            // Apply the operator to the two values and report the result as the value of the expression.
            double vSFinalValue1 = valueStack.Pop();
            double vSFinalValue2 = valueStack.Pop();

            if (operatorStack.Peek() == "+")
            {
                operatorStack.Pop();
                finalValue = vSFinalValue2 + vSFinalValue1;
            }
            else
            {
                operatorStack.Pop();
                finalValue = vSFinalValue2 - vSFinalValue1;
            }
        }

        /// <summary>
        /// Starting Token Rule - The first token of an expression must be a number, a variable, or 
        /// an opening parenthesis.
        /// 
        /// Ending Token Rule- The last token of an expression must be a number, a variable, or a 
        /// closing parenthesis.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"> is "(" if checking Start Token Rule or ")" if checking End Token Rule</param>
        /// <returns></returns>
        static bool StartOrEndTokenRule(string s, string r)
        {
            bool openParen = false;
            bool num = false;
            bool var = false;
            double number = 0;

            if (s.Equals(r))
            {
                openParen = true;
            }
            if (double.TryParse(s, out number))
            {
                num = true;
            }
            if (Regex.IsMatch(s, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                var = true;
            }
            if (openParen == true || num == true || var == true)
            {
                return true;
            }
            return false;
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
    }
}