// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>

        
        //instance varible to hold the string form of the formula for use in other methods.
        private string stringFormula;
        public Formula(String formula)
        {
            stringFormula = formula;

            List<String> tokens = GetTokens(formula).ToList<String>();

            if (tokens.Count == 0)
            {
                throw new FormulaFormatException("there must be atleast one token!");
            }

                int parenCount = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!isValidToken(tokens[i]))
                {
                    throw new FormulaFormatException("Detected invalid symbol.");
                }

                if (tokens[i].Equals("(") || isOperator(tokens[i]))
                {
                    if (tokens[i].Equals("("))
                    {
                        parenCount++;
                    }

                    if (i + 1 < tokens.Count && !(isNumber(tokens[i + 1]) || isVar(tokens[i + 1]) || tokens[i + 1].Equals("(")))
                    {
                        throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
                    }
                }

                if (isNumber(tokens[i]) || isVar(tokens[i]) || tokens[i].Equals(")"))
                {
                    if (tokens[i].Equals(")"))
                    {
                        parenCount--;
                        if (parenCount < 0)
                        {
                            throw new FormulaFormatException("There are more closing parenthesis than opening ones.");
                        }
                    }
                    if (i + 1 < tokens.Count && !(isOperator(tokens[i + 1]) || tokens[i + 1].Equals(")")))
                    {
                        throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
                    }
                }

            }
            if (parenCount != 0)
            {
                throw new FormulaFormatException("Balance your parenthesis.");
            }

            String firstToken = tokens.First<String>();
            if (!(isNumber(firstToken) || isVar(firstToken) || firstToken.Equals("(")))
            {
                throw new FormulaFormatException("Check your first item!");
            }
            String lastToken = tokens.Last<String>();
            if (!(isNumber(lastToken) || isVar(lastToken) || lastToken.Equals(")")))
            {
                throw new FormulaFormatException("Check your last item!");
            }

        }

        private static bool isValidToken(string token)
        {
            if (isNumber(token) || isOperator(token) || isVar(token) || isParen(token))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Is the given token an Operator?
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isOperator(string token)
        {
            List<String> operators = new List<string>() { "+", "-", "*", "/" };

            foreach (string item in operators)
            {
                if (token.Equals(item))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Is the given token a vlid double?
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isNumber(string token)
        {
            double result = 0;
            return double.TryParse(token,out result) && result > 0;
        }
        /// <summary>
        /// Is the token a vlid variable?
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isVar(string token)
        {
            if (!char.IsLetter(token[0]))
                return false;

            bool nowNums = false;
            for (int i = 0; i < token.Length; i++)
            {
                if (char.IsNumber(token[i]))
                {
                    nowNums = true;
                }
                if (nowNums == true && !char.IsNumber(token[i]))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Is the given token a parenthesis?
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isParen(string token)
        {
            return (token.Equals("(") || token.Equals(")"));
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            List<String> tokens = GetTokens(this.stringFormula).ToList();

            Stack<double> values = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                string type = readToken(tokens[i]);

                if (type.Equals("number"))
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        if (operators.Peek().Equals("*"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() * double.Parse(tokens[i]));
                        }
                        else if (operators.Peek().Equals("/"))
                        {
                            operators.Pop();
                            if (double.Parse(tokens[i]) == 0)
                            {
                                throw new FormulaEvaluationException("Dividing by Zero.");
                            }
                            values.Push(values.Pop() / double.Parse(tokens[i]));
                        }
                    }
                    else
                    {
                        values.Push(double.Parse(tokens[i]));
                    }
                }
                else if (type.Equals("var"))
                {
                    double varValue;
                    try
                    {
                        varValue = lookup.Invoke(tokens[i]);
                    }
                    catch (UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("A variable is undefined.");
                    }
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        if (operators.Peek().Equals("*"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() * varValue);
                        }
                        else if (operators.Peek().Equals("/"))
                        {
                            operators.Pop();
                            if (varValue == 0)
                            {
                                throw new FormulaEvaluationException("Dividing by Zero.");
                            }
                            values.Push(values.Pop() / varValue);
                        }
                    }
                    else
                    {
                        values.Push(varValue);
                    }

                }
                else if (type.Equals("+-"))
                {
                    if (operators.Count > 0 && (operators.Peek().Equals("+") || operators.Peek().Equals("-")))
                    {
                        if (operators.Peek().Equals("+"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() + values.Pop());
                        }
                        else if (operators.Peek().Equals("-"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() - values.Pop());
                        }
                    }
                    operators.Push(tokens[i]);
                }
                else if (type.Equals("*/"))
                {
                    operators.Push(tokens[i]);
                }
                else if (type.Equals("("))
                {
                    operators.Push(tokens[i]);
                }
                else if (type.Equals(")"))
                {
                    if (operators.Count > 0)
                    {
                        if (operators.Peek().Equals("+"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() + values.Pop());
                        }
                        else if (operators.Peek().Equals("-"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() - values.Pop());
                        }
                    }
                    operators.Pop();
                    if(operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        if (operators.Peek().Equals("*"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() * values.Pop());
                        }
                        else if (operators.Peek().Equals("/"))
                        {
                            operators.Pop();
                            values.Push(values.Pop() / values.Pop());
                        }
                    }
                }
                

            }
            if (operators.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                string lastOperator = operators.Pop();

                if (lastOperator.Equals("+"))
                {
                    return values.Pop() + values.Pop();
                }
                else
                {
                    return values.Pop() - values.Pop();
                }
            }
        }
        private static string readToken(string token)
        {
            if (isNumber(token))
            {
                return "number";
            }

            if (isVar(token))
            {
                return "var";
            }

            if (isOperator(token))
            {
                if (token.Equals("*") || token.Equals("/"))
                {
                    return "*/";
                }
                else
                {
                    return "+-";
                }
            }
            return token;
        }

        
        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
