using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.Predictables
{
    public class TwoSegmentPredictableModel : PredictableModelWithFixedStyle
    {
        public float VX1 { get; set; }
        public float VY1 { get; set; }
        public int T1 { get; set; }

        public float VX2 { get; set; }
        public float VY2 { get; set; }

        public float Rotation1 { get; set; }
        public float Rotation2 { get; set; }

        public override BulletPrediction GetBulletPredictionAt(int time)
        {
            if (time < LifeTimeBegin || time > LifeTimeEnd) return null;

            BulletPrediction prediction = null;
            if (prediction == null) prediction = new BulletPrediction();
            prediction.Style = Style;
            prediction.Color = Color;
            prediction.X = time - LifeTimeBegin < T1 ? InitX + VX1 * (time - LifeTimeBegin) : InitX + VX1 * T1 + VX2 * (time - LifeTimeBegin - T1);
            prediction.Y = time - LifeTimeBegin < T1 ? InitY + VY1 * (time - LifeTimeBegin) : InitY + VY1 * T1 + VY2 * (time - LifeTimeBegin - T1);
            prediction.Rotation = time - LifeTimeBegin < T1 ? Rotation1 : Rotation2;
            return prediction;
        }
    }
}
