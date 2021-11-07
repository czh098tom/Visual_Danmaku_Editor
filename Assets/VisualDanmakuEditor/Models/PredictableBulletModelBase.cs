using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Reflection.Utilities;

using VisualDanmakuEditor.Models.BulletPredict;

namespace VisualDanmakuEditor.Models
{
    public abstract class PredictableBulletModelBase
    {
        public const int infinite = int.MaxValue;

        public static readonly PredictableBulletModelBase @default = new SimplePredictableBulletModel()
        {
            LifeTimeBegin = -infinite,
            LifeTimeEnd = -infinite
        };

        public int LifeTimeBegin { get; set; }
        public int LifeTimeEnd { get; set; }

        public abstract BulletPrediction GetPredictionAt(int time);

        public override string ToString()
        {
            return this.ToStringEx(typeof(ValueType));
        }
    }
}
