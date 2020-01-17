///
/// <summary>
///     Author: Jacob Morrison
///     Date: 1/17/2020
///     This code recieves a string and determines if it is a formula and evaluates it.
///     I pledge that I did the work myself.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        static Stack<int> valueStack = new Stack<int>();
        static Stack<string> operatorStack = new Stack<string>();

        // tokenValue will be set value of token in TryPase below
        static int tokenValue = 0;

        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            int finalValue = 0;
            // substrings holds tokens from given expression
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            
            // for loop through substrings
            foreach (string token in substrings)
            {
                // trim whitespaces off front and back of the token
                token.Trim();

                // if token is an integer
                if (int.TryParse(token, out tokenValue))
                {
                    IsInt(token);
                }
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
                else if(token == ")")
                {
                    IsRightParenthesis(token);
                }
                // if token is a variable
                else
                {
                    // need to check to make sure it is a variable and throw an exception if not
                    // example could get an "@" symbol
                    IsInt(variableEvaluator(token).ToString());
                }
            }

            // operatorStack is now empty
            if (operatorStack.Count == 0)
            {
                //Value stack should contain a single number
                //Pop it and report as the value of the expression
                finalValue = valueStack.Pop();
            }
            // operatorStack has one item left
            else
            {
                // There should be exactly one operator on the operator stack, and it should be either + or -.There 
                // should be exactly two values on the value stack.Apply the operator to the two values and report 
                // the result as the value of the expression.
                int vSFinalValue1 = valueStack.Pop();
                int vSFinalValue2 = valueStack.Pop();

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

            return finalValue;
        }

        // helper methods

        /// <summary>
        /// Determines if token is an integer and does the following:
        /// If * or / is at the top of the operator stack, pop the value stack, pop the operator stack, 
        /// and apply the popped operator to the popped number and t. Push the result onto the value stack.
        /// Otherwise, pushes token onto the value stack.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static void IsInt(string token)
        {
            // If * or / is at the top of the operator stack, pop the value stack, pop the operator stack, 
            // and apply the popped operator to the popped number and tokenValue. Push the result onto the value 
            // stack.

            if (operatorStack.hasOnTop("*", "/"))
            {
                if (valueStack.Count == 0)
                {
                    throw new ArgumentException(String.Format("Invalid formula, valueStack is empty"));
                }
                int operand = valueStack.Pop();
                string op = operatorStack.Pop();

                if (op == "*")
                {
                    valueStack.Push(operand * tokenValue);
                }
                else
                {
                    if (tokenValue == 0)
                    {
                        throw new ArgumentException(String.Format("Can not divide by zero"));
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
        /// <param name="token"></param>
        private static void IsPlusOrMinus(string token)
        {
            //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack 
            // once, then apply the popped operator to the popped numbers, then push the result onto the value 
            // stack.
            if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
            {
                if(valueStack.Count > 2)
                {
                    throw new ArgumentException(String.Format("Invalid formula, ValueStack has less than 2 tokens."));
                }

                int operand1 = valueStack.Pop();
                int operand2 = valueStack.Pop();
                if (operatorStack.Peek() == "+")
                {
                    operatorStack.Pop();
                    valueStack.Push(operand1 + operand2);
                }
                else
                {
                    operatorStack.Pop();
                    valueStack.Push(operand1 - operand2);
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
        /// <param name="token"></param>
        private static void IsMultiplyOrDivide(string token)
        {
            // Push token onto the operator stack
            operatorStack.Push(token);
        }

        /// <summary>
        /// Determines if token is "(" and does the following: Push t onto the operator stack.
        /// </summary>
        /// <param name="token"></param>
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
        /// <param name="token"></param>
        private static void IsRightParenthesis(string token)
        {
            // If + or - is at the top of the operator stack, pop the value stack twice and the operator stack 
            // once. Apply the popped operator to the popped numbers. Push the result onto the value stack.
            if (operatorStack.hasOnTop("+", "-"))
            {
                //exact same code as when checking for + or -, find way to reduce repetitive code
                int operand1 = valueStack.Pop();
                int operand2 = valueStack.Pop();
                if (operatorStack.Pop() == "+")
                {
                    operatorStack.Pop();
                    valueStack.Push(operand1 + operand2);
                }
                else
                {
                    operatorStack.Pop();
                    valueStack.Push(operand1 - operand2);
                }
                // Next, the top of the operator stack should be a '('.Pop it.
                operatorStack.Pop();

                // Finally, if * or / is at the top of the operator stack, pop the value stack twice and the 
                // operator stack once. Apply the popped operator to the popped numbers. Push the result onto 
                // the value stack.
                int operand3 = valueStack.Pop();
                int operand4 = valueStack.Pop();
                if (operatorStack.Pop() == "*")
                {
                    operatorStack.Pop();
                    valueStack.Push(operand3 * operand4);
                }
                else
                {
                    operatorStack.Pop();
                    valueStack.Push(operand3 / operand4);
                }
            }
        }

        // extensions

        /// <summary>
        /// Checks to see what is on top of the stack.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns> returns true if s1 or s2 is on top of the stack </returns>
        public static bool hasOnTop<T>(this Stack<T> stack, T oper1, T oper2)
        {
            if(stack.Count > 0)
            {
                return (stack.Peek().Equals(oper1) || stack.Peek().Equals(oper2));
            }
            else
            {
                return false;
            }
        }

    }
}
