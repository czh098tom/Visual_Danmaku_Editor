using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Reflection.Utilities;

using VisualDanmakuEditor.Models.Predictables;

namespace VisualDanmakuEditor.Models
{
    public abstract class PredictableBulletModelBase : PredictableObjectModelBase
    {
        public static new readonly PredictableBulletModelBase @default = new SimplePredictableBulletModel()
        {
            LifeTimeBegin = -infinite,
            LifeTimeEnd = -infinite
        };

        public abstract BulletPrediction GetBulletPredictionAt(int time);

        public override sealed ObjectPrediction GetPredictionAt(int time)
        {
            return GetBulletPredictionAt(time);
        }
    }
}
