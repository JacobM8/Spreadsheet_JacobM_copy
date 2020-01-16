using System;
using FormulaEvaluator;

namespace Test_The_Evaluator_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            // Evaluator.Evaluate("(2 + 3 + 3) * 5 + 2 * A4", myLookup);
            if (Evaluator.Evaluate("5+5", null) == 10)
            {
                Console.WriteLine("Happy Day!");
            }
        }
    }
}
