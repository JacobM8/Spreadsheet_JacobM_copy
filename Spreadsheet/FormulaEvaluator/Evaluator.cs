using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    class Evaluator
    {
        Stack<string> valueStack = new Stack<string>();
        Stack<string> operatorStack = new Stack<string>();

        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            int finalValue = 0;
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

           
            return finalValue;
        }
        // go through substrings and put into valueStack or operatorStack accordingly and perform the right 
        // action according to the table in the instructions
    }
}
