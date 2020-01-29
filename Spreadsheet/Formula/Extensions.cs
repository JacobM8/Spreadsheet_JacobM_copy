using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetUtilities
{
	public static class Extensions
	{
		// extensions
		/// <summary>
		/// Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
		/// parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
		/// </summary>
		/// <param name="s"></param>
		/// <returns> true or false </returns>
		public static bool WhenOpenPerenOrOperator(this string[] s)
		{
			// used to verify token is a number in TryParse
			double number;
			// used to verify token is a variable
			String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
			for (int i = 0; i < s.Length - 1; i++)
			{
				// Any token that immediately follows an opening parenthesis or an operator must be either 
				// a number, a variable, or an opening parenthesis.
				if ((s[i].Equals("(") || s[i].IsOperator()) && (double.TryParse(s[i + 1], out number) || 
					s[i + 1].Equals(varPattern) || s[i + 1] == "("))
				{

					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Extra Following Rule - Any token that immediately follows a number, a variable, or a closing 
		/// parenthesis must be either an operator or a closing parenthesis.
		/// </summary>
		/// <param name="s"></param>
		/// <returns> returns true if above statement is correct </returns>
		public static bool WhenNumOrVarOrCloseParen(this string[] s)
		{
			// used to verify token is variable in TryParse
			double number = 0;
			// used to verify token is a variable
			String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
			for (int i = 0; i < s.Length - 1; i++)
			{
				// Any token that immediately follows a number, a variable, or a closing 
				// parenthesis must be either an operator or a closing parenthesis.
				if ((double.TryParse(s[i], out number) || s.Equals(varPattern) || s.Equals(")")) 
					&& (s[i + 1].IsOperator() || s[i + 1].Equals(")")))
				{
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Verifies if given string is either an operator ("*", "/", "+", or "-").
		/// </summary>
		/// <param name="s"></param>
		/// <returns> returns true if above statement is correct </returns>
		static bool IsOperator(this string s)
		{
			if (s.Equals("*") || s.Equals("/") || s.Equals("+") || s.Equals("-"))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Checks to see what is on top of the stack.
		/// </summary>
		/// <param name="s1"> generic type T </param>
		/// <param name="s2"> generic type T </param>
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