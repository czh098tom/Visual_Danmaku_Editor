using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Latticework.Expressions.Token;

namespace Latticework.Expressions
{
    public class Expression
    {
        readonly LinkedList<string> reversedPolandExpr = new LinkedList<string>();

        public Expression(string expression)
        {
            reversedPolandExpr = ExpressionResolver.Default.GetReversedPolandExpression(expression);
        }

        public float Calculate(Func<string, float> variableGetter)
        {
            Stack<float> stack = new Stack<float>();
            foreach (string s in reversedPolandExpr)
            {
                if (IsIdentifierOrNumber(s[0]))
                {
                    if (float.TryParse(s, out float res))
                    {
                        stack.Push(res);
                    }
                    else
                    {
                        try
                        {
                            float f = variableGetter(s);
                            stack.Push(f);
                        }
                        catch (Exception e)
                        {
                            throw new ExpressionCalculatingException($"Variable with name \"{s}\" cannot be found.", e);
                        }
                    }
                }
                else
                {
                    OperatorBase o = OperatorBase.GetOperator(s);
                    LinkedList<float> values = new LinkedList<float>();
                    for (int i = 0; i < o.NumOfOprands; i++)
                    {
                        try
                        {
                            values.AddFirst(stack.Pop());
                        }
                        catch (Exception e)
                        {
                            throw new ExpressionResolvingException(
                                $"Argument count for operator \"{s}\" is not sufficient, "
                                + $"required {o.NumOfOprands}, actual {o.NumOfOprands - i - 1}.", e);
                        }
                    }
                    stack.Push(o.Calculate(values.ToArray()));
                }
            }
            return stack.Peek();
        }

        public override string ToString()
        {
            string str = "";
            foreach (string s in reversedPolandExpr)
            {
                str += s + " ";
            }
            return str;
        }
    }
}
