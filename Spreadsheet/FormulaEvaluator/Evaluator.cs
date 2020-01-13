using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public class Evaluator
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
                    IsInt(variableEvaluator(token).ToString());
                }

                // just finished top table in instructions
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

            if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                int operand = valueStack.Pop();
                string op = operatorStack.Pop();
                if (op == "*")
                {
                    valueStack.Push(operand * tokenValue);
                }
                else
                {
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
            if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
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
    }
}
