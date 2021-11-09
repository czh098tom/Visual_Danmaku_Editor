using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Reflection.Utilities;

namespace VisualDanmakuEditor.Models
{
    public abstract class PredictableObjectModelBase
    {
        public const int infinite = int.MaxValue;

        public static readonly PredictableObjectModelBase @default;

        public float InitX { get; set; }
        public float InitY { get; set; }

        public int LifeTimeBegin { get; set; }
        public int LifeTimeEnd { get; set; }

        public abstract ObjectPrediction GetPredictionAt(int time);

        public override string ToString()
        {
            return this.ToStringEx(typeof(ValueType));
        }
    }
}
