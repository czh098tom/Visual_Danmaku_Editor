using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions.Operator;

using static Latticework.Expressions.Token;

namespace Latticework.Expressions
{
    public class ExpressionResolver
    {
        public static ExpressionResolver Default { get; private set; } = new ExpressionResolver();

        private LinkedList<string> reversedPolandExpr;
        private string expression;

        private StringBuilder token;
        private Stack<string> @operator;
        private char last;
        private bool first;
        private bool canBeNegative;
        int i;

        public LinkedList<string> GetReversedPolandExpression(string expr)
        {
            expression = expr ?? throw new ExpressionResolvingException("Expression is empty.",
                    new InvalidOperationException("Argument \"expression\" cannot be null."));
            reversedPolandExpr = new LinkedList<string>();
            token = new StringBuilder();
            @operator = new Stack<string>();

            last = ' ';
            first = true;
            canBeNegative = true;
            for (i = 0; i < expression.Length; i++)
            {
                if (IsIdentifierBoundary(i))
                {
                    if (token.Length != 0)
                    {
                        HandleToken();
                        token.Clear();
                    }
                    if (!char.IsWhiteSpace(expression[i])) token.Append(expression[i]);
                }
                else
                {
                    token.Append(expression[i]);
                }
                last = expression[i];
                first = false;
            }
            if (token.Length != 0)
            {
                HandleToken();
            }
            while (@operator.Count > 0)
            {
                reversedPolandExpr.AddLast(@operator.Pop());
            }
            if (reversedPolandExpr.Count == 0) reversedPolandExpr.AddLast("0");
            return reversedPolandExpr;
        }

        private void HandleToken()
        {
            if (IsIdentifierOrNumber(token[0]))
            {
                canBeNegative = false;
                reversedPolandExpr.AddLast(token.ToString());
            }
            else if (token[0] == '(')
            {
                canBeNegative = true;
                @operator.Push(token.ToString());
            }
            else if (token[0] == ')')
            {
                canBeNegative = false;
                try
                {
                    while (@operator.Peek() != "(")
                    {
                        reversedPolandExpr.AddLast(@operator.Pop());
                    }
                    @operator.Pop();
                }
                catch (InvalidOperationException ioe)
                {
                    throw new ExpressionResolvingException("Brackets mismatched.", ioe);
                }
            }
            else
            {
                OperatorBase thisOperator = OperatorBase.GetOperator(token.ToString());
                if (canBeNegative && thisOperator is SubstractionOperator)
                {
                    if (reversedPolandExpr.Count > 0)
                    {
                        reversedPolandExpr.AddBefore(reversedPolandExpr.Last, "0");
                    }
                    else
                    {
                        reversedPolandExpr.AddFirst("0");
                    }
                }
                if (@operator.Count > 0 && @operator.Peek() != "(" &&
                    thisOperator.Priority >= OperatorBase.GetOperator(@operator.Peek()).Priority)
                {
                    while (@operator.Count > 0 && @operator.Peek() != "("
                        && thisOperator.Priority >= OperatorBase.GetOperator(@operator.Peek()).Priority)
                    {
                        reversedPolandExpr.AddLast(@operator.Pop());
                    }
                    @operator.Push(token.ToString());
                }
                else
                {
                    @operator.Push(token.ToString());
                }
            }
        }

        private bool IsIdentifierBoundary(int i)
        {
            return !first
                && (char.IsWhiteSpace(expression[i]) || last == '(' || last == ')' || expression[i] == '(' || expression[i] == ')'
                || !IsSameCharType(last, expression[i]));
        }

        private static bool IsSameCharType(char a, char b)
        {
            return (IsIdentifierOrNumber(a) && IsIdentifierOrNumber(b))
                || (IsOperator(a) && IsOperator(b));
        }
    }
}
