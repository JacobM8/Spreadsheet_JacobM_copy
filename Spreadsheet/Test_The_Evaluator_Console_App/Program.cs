///
/// <summary>
///     Author: Jacob Morrison
///     Date: 1/17/2020
///     This code tests Evaluator.cs.
///     I pledge that I did the work myself.
/// </summary>
using System;
using FormulaEvaluator;

namespace Test_The_Evaluator_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            // need to define variables when testing them, tester needs to throw exception if variables aren't defined
            // need to be ready for all possible erros, could be divide by zero so throw an exception
            // stack could be empty so maybe it's a bad formula
            // be specific in exception details

            /*
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
            */
            // Test simple parenthesis additon
            if (Evaluator.Evaluate("5 + (6/2)", null) == 3)
            {
                Console.WriteLine("Simple Parenthesis with division works");
            }

        }
    }
}
