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
        private readonly TaskModel task;

        private LinkedListNode<AdvancedRepeatModel> Root { get; set; }

        private LinkedList<PredictableBulletModel> predictableBulletModels;
        private Dictionary<string, float> variables;

        private Expression vexp;
        private Expression rexp;
        private Expression xexp;
        private Expression yexp;

        private Expression intexp;
        private Expression int2exp;

        int currTimeDelay = 0;

        internal TaskIntepreter(TaskModel task)
        {
            this.task = task;
            Root = task.First;
        }

        internal IEnumerable<PredictableBulletModel> GetPredictableBulletModels(int maxTime = int.MaxValue)
        {
            predictableBulletModels = new LinkedList<PredictableBulletModel>();
            variables = new Dictionary<string, float>();

            vexp = new Expression(task.BulletModel.VelocityExpression);
            rexp = new Expression(task.BulletModel.RotationExpression);
            xexp = new Expression(task.BulletModel.XExpression);
            yexp = new Expression(task.BulletModel.YExpression);

            intexp = new Expression(task.Interval);
            int2exp = new Expression(task.Interval2);

            currTimeDelay = 0;

            LinkedListNode<VariableModelBase> currVariable = null;
            Stack<int> currIterTimes = new Stack<int>();
            Stack<int> currRepeatTimes = new Stack<int>();
            Stack<LinkedListNode<AdvancedRepeatModel>> currRepeat = new Stack<LinkedListNode<AdvancedRepeatModel>>();

            if (Root != null)
            {
                currRepeat.Push(Root);
                currRepeatTimes.Push(Convert.ToInt32(Math.Floor(new Expression(Root.Value.Times).Calculate(Indexer))));
                currIterTimes.Push(-1);
                while (currIterTimes.Count > 0)
                {
                    currIterTimes.Push(currIterTimes.Pop() + 1);
                    currVariable = currRepeat.Peek().Value.First;
                    while (currVariable != null)
                    {
                        variables[currVariable.Value.VariableName] =
                            currVariable.Value.GetVariableValue(currIterTimes.Peek(), currRepeatTimes.Peek(), Indexer);
                        currVariable = currVariable.Next;
                    }
                    if (currIterTimes.Peek() < currRepeatTimes.Peek())
                    {
                        if (currRepeat.Peek().Next == null)
                        {
                            GenerateBulletModel();
                        }
                        else
                        {
                            currRepeatTimes.Push(Convert.ToInt32(Math.Floor(new Expression(currRepeat.Peek().Next.Value.Times).Calculate(Indexer))));
                            currRepeat.Push(currRepeat.Peek().Next);
                            currIterTimes.Push(-1);
                        }
                    }
                    else
                    {
                        currIterTimes.Pop();
                        currRepeat.Pop();
                        currRepeatTimes.Pop();
                    }
                    //if (currRepeat.Count == 2) currTimeDelay += Convert.ToInt32(int2exp.Calculate(Indexer));
                    //if (currRepeat.Count == 1) currTimeDelay += Convert.ToInt32(intexp.Calculate(Indexer));
                    if (currRepeat.Count > 0)
                    {
                        currTimeDelay += Convert.ToInt32(new Expression(currRepeat.Peek().Value.Interval).Calculate(Indexer));
                    }
                    if (currTimeDelay > maxTime) break;
                }
            }
            else
            {
                GenerateBulletModel();
            }
            return predictableBulletModels;
        }

        private float Indexer(string s)
        {
            if (variables.ContainsKey(s))
            {
                return variables[s];
            }
            return 0;
        }

        private void GenerateBulletModel()
        {
            float r = rexp.Calculate(Indexer);
            float v = vexp.Calculate(Indexer);
            float vx = Convert.ToSingle(v * Math.Cos(r * Math.PI / 180f));
            float vy = Convert.ToSingle(v * Math.Sin(r * Math.PI / 180f));
            predictableBulletModels.AddLast(new PredictableBulletModel()
            {
                Style = task.BulletModel.Style,
                Color = task.BulletModel.Color,
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
