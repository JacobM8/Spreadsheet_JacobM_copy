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
		// extensions
		/// <summary>
		/// Parenthesis/Operator Following Rule - Any token that immediately follows an opening 
		/// parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
		/// </summary>
		/// <param name="s"></param>
		/// <returns> true or false </returns>
		public static bool WhenOpenPerenOrOperator(this string formula)
		{
			// used to verify token is a number in TryParse
			double number;
			for (int i = 0; i < formula.Length - 1; i++)
			{
				// Any token that immediately follows an opening parenthesis or an operator must be either 
				// a number, a variable, or an opening parenthesis.
				if ((formula[i].ToString().Equals("(") || formula[i].ToString().IsOperator()) && (double.TryParse(formula[i + 1].ToString(), out number) ||
					   Regex.IsMatch(formula[i + 1].ToString(), @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || formula[i + 1].ToString().Equals("(")))
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
		public static bool WhenNumOrVarOrCloseParen(this string formula)
		{
			// used to verify token is variable in TryParse
			double number = 0;
			for (int i = 0; i < formula.Length - 1; i++)
			{
				// Any token that immediately follows a number, a variable, or a closing 
				// parenthesis must be either an operator or a closing parenthesis.
				// s[i].CheckVariable() 
				if ((double.TryParse(formula[i].ToString(), out number) || Regex.IsMatch(formula[i].ToString(), @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") ||
					formula[i].ToString().Equals(")")) && (formula[i + 1].ToString().IsOperator() || formula[i + 1].ToString().Equals(")")))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Verifies if given string is either an operator ("*", "/", "+", or "-").
		/// </summary>
		/// <param name="s"> string needed to be checked </param>
		/// <returns> returns true if above statement is correct </returns>
		public static bool IsOperator(this string s)
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