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
            Stack<float> valueStack = new Stack<float>();
            Stack<string> identifierStack = new Stack<string>();
            foreach (string s in reversedPolandExpr)
            {
                if (IsIdentifierOrNumber(s[0]))
                {
                    if (float.TryParse(s, out float res))
                    {
                        valueStack.Push(res);
                    }
                    else
                    {
                        try
                        {
                            float f = variableGetter(s);
                            valueStack.Push(f);
                        }
                        catch (Exception e)
                        {
                            throw new ExpressionCalculatingException($"Variable with name \"{s}\" cannot be found.", e);
                        }
                    }
                    identifierStack.Push(s);
                }
                else
                {
                    OperatorBase o = OperatorBase.GetOperator(s);
                    LinkedList<float> values = new LinkedList<float>();
                    LinkedList<string> identifiers = new LinkedList<string>();
                    for (int i = 0; i < o.NumOfOprands; i++)
                    {
                        try
                        {
                            values.AddFirst(valueStack.Pop());
                            identifiers.AddFirst(identifierStack.Pop());
                        }
                        catch (Exception e)
                        {
                            throw new ExpressionResolvingException(
                                $"Argument count for operator \"{s}\" is not sufficient, "
                                + $"required {o.NumOfOprands}, actual {o.NumOfOprands - i - 1}.", e);
                        }
                    }
                    valueStack.Push(o.CalculateWithIdentifiers(values.ToArray(), identifiers.ToArray()));
                    identifierStack.Push("");
                }
            }
            return valueStack.Peek();
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
