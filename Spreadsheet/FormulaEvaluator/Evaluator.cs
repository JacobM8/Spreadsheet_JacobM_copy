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

        // tokenValue will be set to the value of token when it's an int in TryPase below
        static int tokenValue = 0;
        static int finalValue = 0;

        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            
            // substrings holds tokens from given expression
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            
            // for loop through substrings
            foreach (string token in substrings)
            {
                // trim whitespaces off front and back of the token
                token.Trim();

                if (token == "")
                {
                    continue;
                }

                // if token is an integer
                if (int.TryParse(token, out tokenValue))
                {
                    IsInt(tokenValue);
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
                    // checks to see if token is a valid variable
                    if (Regex.IsMatch(token, @"[a-zA-Z]+\d+"))
                    {
                        // try catch will catch an exception when there is one from Program.cs
                        try
                        {
                            int variableValue = variableEvaluator(token);
                            IsInt(variableValue);
                        }
                        catch
                        {
                            throw new ArgumentException(String.Format("Lookup function threw exception."));
                        }   
                    }
                }
            }

            // operatorStack is now empty
            if (operatorStack.Count == 0)
            {
                OpStackEmpty();
            }
            // operatorStack has one item left
            else
            {
                OpStackOneRemaining();
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
        private static void IsInt(int tokenValue)
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
                        throw new ArgumentException(String.Format("Invalid formula, Cannot divide by zero"));
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
            if (operatorStack.hasOnTop("+", "-"))
            {
                if(valueStack.Count < 2)
                {
                    throw new ArgumentException(String.Format("Invalid formula, ValueStack has less than 2 tokens."));
                }

                int operand1 = valueStack.Pop();
                int operand2 = valueStack.Pop();
                string currOperator = operatorStack.Pop();
                if (currOperator == "+")
                {
                    valueStack.Push(operand1 + operand2);
                    operatorStack.Push(token);
                }
                else
                {
                    valueStack.Push(operand1 - operand2);
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
            // If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once
            // Apply the popped operator to the popped numbers. Push the result onto the value stack.
            if (operatorStack.hasOnTop("+", "-"))
            {
                if (valueStack.Count < 2)
                {
                    throw new ArgumentException(String.Format("Invalid formula, valueStack.Count > 2 when using IsRightParenthesis"));
                }
                int operand1 = valueStack.Pop();
                int operand2 = valueStack.Pop();
                string currOperator1 = operatorStack.Pop();
                if (currOperator1 == "+")
                {
                    valueStack.Push(operand1 + operand2);                    
                }
                else
                {
                    valueStack.Push(operand1 - operand2);
                }
                // Next, the top of the operator stack should be a '('.Pop it.
                string currOperator2 = operatorStack.Pop();
                if (currOperator2 != "(")
                {
                    throw new ArgumentException(String.Format("Invalid formula, '(' is not in the correct spot"));
                }
                

                // Finally, if * or / is at the top of the operator stack, pop the value stack twice and the 
                // operator stack once. Apply the popped operator to the popped numbers. Push the result onto 
                // the value stack.
                if (operatorStack.hasOnTop("*", "/"))
                {
                    if (valueStack.Count < 2)
                    {
                        throw new ArgumentException(String.Format("Invalid formula, valueStack.Count < 2 when using IsRightParenthesis"));
                    }
                    int operand3 = valueStack.Pop();
                    int operand4 = valueStack.Pop();
                    string currOperator3 = operatorStack.Pop();
                    if (currOperator3 == "*")
                    {
                        valueStack.Push(operand3 * operand4);
                    }
                    else
                    {
                        if (operand4 == 0)
                        {
                            throw new ArgumentException(String.Format("Invalid formula, cannot divide by zero."));
                        }
                        valueStack.Push(operand3 / operand4);
                    }
                }
            }
        }

        /// <summary>
        /// Value stack should contain a single number, pop it and report as the value of the expression
        /// </summary>
        /// <returns> returns finalValue of formula </returns>
        private static int OpStackEmpty()
        {
            //Value stack should contain a single number
            if (valueStack.Count != 1)
            {
                throw new ArgumentException(String.Format("Invalid formula, valueStack.Count != 1"));
            }
            //Pop it and report as the value of the expression
            finalValue = valueStack.Pop();

            return finalValue;
        }

        /// <summary>
        /// There should be exactly one operator on the operator stack, and it should be either + or -. 
        /// There should be exactly two values on the value stack. Apply the operator to the two values and 
        /// report the result as the value of the expression.
        /// </summary>
        /// <returns> returns finalValue of formula </returns>
        private static int OpStackOneRemaining()
        {
            // There should be exactly one operator on the operator stack, and it should be either + or -. There 
            // should be exactly two values on the value stack.
            if (operatorStack.Count != 1)
            {
                throw new ArgumentException(String.Format("Invalid formula, operatorStack.Count != 1"));
            }
            else if (valueStack.Count != 2)
            {
                throw new ArgumentException(String.Format("Invalid formula, valueStack.Count != 2"));
            }
            if (!operatorStack.hasOnTop("+", "-"))
            {
                throw new ArgumentException(String.Format("Invalid formula, operatorStack does not have '+' or '-' as last token"));
            }

            // Apply the operator to the two values and report the result as the value of the expression.
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

            return finalValue;
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
