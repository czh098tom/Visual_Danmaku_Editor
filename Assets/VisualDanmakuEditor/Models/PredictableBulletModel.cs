using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Reflection.Utilities;

namespace VisualDanmakuEditor.Models
{
    public struct PredictableBulletModel
    {
        public const int infinite = int.MaxValue;

        public static readonly PredictableBulletModel @default = new PredictableBulletModel()
        {
            LifeTimeBegin = -infinite,
            LifeTimeEnd = -infinite
        };

        public string Style { get; set; }
        public string Color { get; set; }

        public int LifeTimeBegin { get; set; }
        public int LifeTimeEnd { get; set; }

        public float InitX { get; set; }
        public float InitY { get; set; }

        public float VX { get; set; }
        public float VY { get; set; }

        public float Rotation { get; set; }

        public BulletPrediction GetPredictionAt(int time)
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

        public override string ToString()
        {
            return this.ToStringEx(typeof(ValueType));
        }
    }
}
