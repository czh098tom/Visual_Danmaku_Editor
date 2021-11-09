using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Reflection.Utilities;

namespace VisualDanmakuEditor.Models.Predictables
{
    public class SimplePredictableBulletModel : PredictableModelWithFixedStyle
    {
        public float VX { get; set; }
        public float VY { get; set; }

        public float Rotation { get; set; }

        public override BulletPrediction GetBulletPredictionAt(int time)
        {
            if (time < LifeTimeBegin || time > LifeTimeEnd) return null;

            BulletPrediction prediction = null;
            if (prediction == null) prediction = new BulletPrediction();
            prediction.Style = Style;
            prediction.Color = Color;
            prediction.X = InitX + VX * (time - LifeTimeBegin);
            prediction.Y = InitY + VY * (time - LifeTimeBegin);
            prediction.Rotation = Rotation;
            return prediction;
        }
    }
}
