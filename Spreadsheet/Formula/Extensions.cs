using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetUtilities
{
	public static class Extensions
	{
		/// <summary>
		/// Verifies if given string is either an operator ("*", "/", "+", or "-").
		/// </summary>
		/// <param name="s"> string needed to be checked </param>
		/// <returns> returns true if s is an operator </returns>
		public static bool IsOperator(this string s)
		{
			if (s.Equals("*") || s.Equals("/") || s.Equals("+") || s.Equals("-"))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks to see if "s" is a parenthesis
		/// </summary>
		/// <param name="s"> string to be checked </param>
		/// <returns> true if s is a paren</returns>
		public static bool IsParen(this string s)
		{
			if (s.Equals("(") || s.Equals(")"))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks to see oper1 or oper2 is on top of the stack.
		/// </summary>
		/// <param name="s1"> operand or operator to see if it is on top of the stack </param>
		/// <param name="s2"> operand or operator to see if it is on top of the stack </param>
		/// <returns> returns true if s1 or s2 is on top of the stack </returns>
		public static bool HasOnTop<T>(this Stack<T> stack, T oper1, T oper2)
		{
			if (stack.Count > 0)
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