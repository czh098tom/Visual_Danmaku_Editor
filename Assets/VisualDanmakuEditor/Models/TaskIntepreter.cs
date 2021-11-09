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

        private LinkedList<PredictableBulletModelBase> predictableBulletModels;
        private Dictionary<string, float> variables;

        int currTimeDelay = 0;

        internal TaskIntepreter(TaskModel task)
        {
            this.task = task;
            Root = task.First;
        }

        internal IEnumerable<PredictableBulletModelBase> GetPredictableBulletModels(int maxTime = int.MaxValue)
        {
            predictableBulletModels = new LinkedList<PredictableBulletModelBase>();
            variables = new Dictionary<string, float>();

            currTimeDelay = task.BeginTime;

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

            if (task.Shooter != null && !task.BulletModel.IsGlobalCoord)
            {
                PredictableObjectModelBase obj = task.Shooter.BuildModelInContext(s => 0f, 0);
                foreach (PredictableBulletModelBase bullet in predictableBulletModels)
                {
                    var objPred = obj.GetPredictionAt(bullet.LifeTimeBegin);
                    bullet.InitX += objPred.X;
                    bullet.InitY += objPred.Y;
                }
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
            predictableBulletModels.AddLast(task.BulletModel.BuildBulletModelInContext(Indexer, currTimeDelay));
        }
    }
}
