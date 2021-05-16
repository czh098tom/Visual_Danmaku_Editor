using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor.Models
{
    internal class TaskIntepreter
    {
        internal LinkedListNode<AdvancedRepeatModel> Root { get; set; }
        internal string VelocityExpression { get; set; }
        internal string RotationExpression { get; set; }
        internal string XExpression { get; set; }
        internal string YExpression { get; set; }

        internal int Interval { get; set; }

        private LinkedList<PredictableBulletModel> predictableBulletModels;
        private Dictionary<string, float> variables;

        private Expression vexp;
        private Expression rexp;
        private Expression xexp;
        private Expression yexp;

        int currTimeDelay = 0;

        private float Indexer(string s)
        {
            if (variables.ContainsKey(s))
            {
                return variables[s];
            }
            return 0;
        }

        internal IEnumerable<PredictableBulletModel> GetPredictableBulletModels()
        {
            predictableBulletModels = new LinkedList<PredictableBulletModel>();
            variables = new Dictionary<string, float>();

            vexp = new Expression(VelocityExpression);
            rexp = new Expression(RotationExpression);
            xexp = new Expression(XExpression);
            yexp = new Expression(YExpression);

            currTimeDelay = 0;

            LinkedListNode<VariableModelBase> currVariable = null;
            Stack<int> currIterTimes = new Stack<int>();
            Stack<LinkedListNode<AdvancedRepeatModel>> currRepeat = new Stack<LinkedListNode<AdvancedRepeatModel>>();

            if (Root != null)
            {
                currRepeat.Push(Root);
                currIterTimes.Push(-1);
                while (currIterTimes.Count > 0)
                {
                    currIterTimes.Push(currIterTimes.Pop() + 1);
                    currVariable = currRepeat.Peek().Value.First;
                    while (currVariable != null)
                    {
                        variables[currVariable.Value.VariableName] =
                            currVariable.Value.GetVariableValue(currIterTimes.Peek(), currRepeat.Peek().Value.Times, Indexer);
                        currVariable = currVariable.Next;
                    }
                    if (currIterTimes.Peek() < currRepeat.Peek().Value.Times)
                    {
                        if (currRepeat.Peek().Next == null)
                        {
                            GenerateBulletModel();
                        }
                        else
                        {
                            currRepeat.Push(currRepeat.Peek().Next);
                            currIterTimes.Push(-1);
                        }
                    }
                    else
                    {
                        currIterTimes.Pop();
                        currRepeat.Pop();
                    }
                    if (currRepeat.Count == 1) currTimeDelay += Interval;
                }
            }
            else
            {
                GenerateBulletModel();
            }
            return predictableBulletModels;
        }

        private void GenerateBulletModel()
        {
            float r = rexp.Calculate(Indexer);
            float v = vexp.Calculate(Indexer);
            float vx = Convert.ToSingle(v * Math.Cos(r * Math.PI / 180f));
            float vy = Convert.ToSingle(v * Math.Sin(r * Math.PI / 180f));
            predictableBulletModels.AddLast(new PredictableBulletModel()
            {
                LifeTimeBegin = currTimeDelay,
                LifeTimeEnd = PredictableBulletModel.infinite,
                InitX = xexp.Calculate(Indexer),
                InitY = yexp.Calculate(Indexer),
                Rotation = r,
                VX = vx,
                VY = vy
            });
        }
    }
}
