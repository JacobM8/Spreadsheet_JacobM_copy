/// <summary>
///     Author: Jacob Morrison
///     Date: 1/17/2020
///     This code tests Evaluator.cs.
///     I pledge that I did the work myself.
/// </summary>
using System;
using System.Text.RegularExpressions;
using FormulaEvaluator;
using static FormulaEvaluator.Evaluator;

namespace Test_The_Evaluator_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "&6";
            static bool Check(string s)
            {
                Regex regex = new Regex(@"[a-zA-Z]+\d+");
                Match match = regex.Match(s);
                return match.Success;
            }
            Console.WriteLine(Check(s));
            
            String varPattern = @"[a-zA-Z]+\d+";
            var v = Regex.Match(s, varPattern);
            Console.WriteLine(v);
            if (Check(s))
            {
                Console.WriteLine("match");
            }
            if (!Check(s))
            {
                Console.WriteLine("no match");
            }
            

            // Test single number only
            if (Evaluator.Evaluate("2", null) == 2)
            {
                Console.WriteLine("Single number only works");
            }
            
            // Test only addition
            if (Evaluator.Evaluate("5+5", null) == 10)
            {
                Console.WriteLine("Addition only works");
            }
            
            // Test only subraction
            if (Evaluator.Evaluate("2-1", null) == 1)
            {
                Console.WriteLine("Subtration only works");
            }
            
            // Test only multiplication
            if (Evaluator.Evaluate("2*2", null) == 4)
            {
                Console.WriteLine("Multiplication only works");
            }
            
            // Test only division
            if (Evaluator.Evaluate("6/2", null) == 3)
            {
                Console.WriteLine("Division only works");
            }
            
            // Test simple parenthesis additon
            if (Evaluator.Evaluate("(1+1)", null) == 2)
            {
                Console.WriteLine("Simple Parenthesis with addition works");
            }

            // test trimming whitespace
            if (Evaluator.Evaluate(" 1 + 1", null) == 2)
            {
                Console.WriteLine("trimming whitespace works");
            }

            // test addition and subraction
            if (Evaluator.Evaluate("2+2-1", null) == 3)
            {
                Console.WriteLine("addition and subraction works");
            }

            // test addition and multiplication
            if (Evaluator.Evaluate("2+2*2", null) == 6)
            {
                Console.WriteLine("addition and multiplicaiton works");
            }

            // test addition and division
            if (Evaluator.Evaluate("2+2/2", null) == 3)
            {
                Console.WriteLine("addition and division works");
            }

            // test substraction and multiplication
            if (Evaluator.Evaluate("10-2*2", null) == 6)
            {
                Console.WriteLine("subtraction and multiplication works");
            }

            // test subtraction and division
            if (Evaluator.Evaluate("4-2/2", null) == 3)
            {
                Console.WriteLine("subtraction and division works");
            }

            // test addition, subtraction and multiplication
            if (Evaluator.Evaluate("2+2*2 -1", null) == 5)
            {
                Console.WriteLine("addition, subtraction and multiplicaiton works");
            }

            // test addition, subtraction and division
            if (Evaluator.Evaluate("2+2/2 -1", null) == 2)
            {
                Console.WriteLine("addition, subtraction and division works");
            }

            // test addition, subtraction, multiplication and division
            if (Evaluator.Evaluate("2+2*2-2/2", null) == 5)
            {
                Console.WriteLine("addition, subtraction, multiplication and division works");
            }

            // test addition, subtraction, multiplication, division and parenthesis
            if (Evaluator.Evaluate("(2+2)*2-2/2", null) == 7)
            {
                Console.WriteLine("addition, subtraction, multiplication, division and parenthesis works");
            }

            // test nested parenthesis
            if (Evaluator.Evaluate("(2*(3+2)+2)", null) == 12)
            {
                Console.WriteLine("nested parenthesis works");
            }
            
            // test variables with addition
            Lookup delegateLookup = new Lookup(assignVariables);
            if (Evaluator.Evaluate("x1+y1", delegateLookup) == 5)
            {
                Console.WriteLine("variable additon works");
            }
            else
            {
                Console.WriteLine(Evaluator.Evaluate("x1+ y1", delegateLookup));
            }

            // test variables with addition and mulitiplication
            if (Evaluator.Evaluate("x1*10 + 3", delegateLookup) == 23)
            {
                Console.WriteLine("variable with additon and multiplication works");
            }
            else
            {
                Console.WriteLine(Evaluator.Evaluate("x1* 10", delegateLookup));
            }

            // test divide by 0
            try
            {
                bool result = Evaluator.Evaluate("10 / 0", null) == 0;
                Console.WriteLine(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"divide by 0 throws exception: {ex}");
            }
        }

        // helper method for variables
        public static int assignVariables(string s1)
        {
            if (s1.Equals("x1"))
            {
                return 2;
            }
            return 3;
        }
    }
}
