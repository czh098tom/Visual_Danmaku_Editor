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
        static readonly Func<string, float> emptyGetter = s => 0;

        readonly string original;
        readonly LinkedList<string> reversedPolandExpr = new LinkedList<string>();

        public string Original => original;

        public bool IsConstant { get; private set; }

        public Expression(string expression)
        {
            original = expression;
            reversedPolandExpr = ExpressionResolver.Default.GetReversedPolandExpression(expression);
            IsConstant = true;
            foreach (string s in reversedPolandExpr)
            {
                if (!OperatorBase.HasOperator(s) && !OperatorBase.HasFunction(s) && !float.TryParse(s, out float _))
                {
                    IsConstant = false;
                }
            }
        }

        public float Calculate()
        {
            if (!IsConstant) throw new InvalidOperationException("Expression is not a constant");
            return Calculate(emptyGetter);
        }

        public float Calculate(Func<string, float> variableGetter)
        {
            Stack<float> valueStack = new Stack<float>();
            Stack<string> identifierStack = new Stack<string>();

            List<float> values = new List<float>(10);
            List<string> identifiers = new List<string>(10);
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
                    values.Clear();
                    identifiers.Clear();
                    foreach (ExpressionCalculatingException ecex in o.OperandCounter(valueStack, identifierStack))
                    {
                        if (ecex == null)
                        {
                            values.Add(valueStack.Pop());
                            identifiers.Add(identifierStack.Pop());
                        }
                        else
                        {
                            throw ecex;
                        }
                    }
                    Stack<float> result = o.CalculateWithIdentifiers(values, identifiers);
                    while (result.Count > 0)
                    {
                        valueStack.Push(result.Pop());
                        identifierStack.Push("");
                    }
                }
            }
            return valueStack.Peek();
        }

        public override string ToString()
        {
            return original;
        }

        public string ToReversedPolandExprString()
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
