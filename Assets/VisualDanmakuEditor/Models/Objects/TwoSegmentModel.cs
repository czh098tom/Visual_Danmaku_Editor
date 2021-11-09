using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

using VisualDanmakuEditor.Models.Predictables;

namespace VisualDanmakuEditor.Models.Objects
{
    public class TwoSegmentModel : ModelWithFixedStyle
    {
        public string XExpression { get; set; } = "0";
        public string YExpression { get; set; } = "0";
        public string Velocity1Expression { get; set; } = "3";
        public string Rotation1Expression { get; set; } = "0";
        public string TimeExpression { get; set; } = "30";
        public string Velocity2Expression { get; set; } = "3";
        public string Rotation2Expression { get; set; } = "0";

        private Expression v1exp;
        private Expression r1exp;
        private Expression texp;
        private Expression v2exp;
        private Expression r2exp;
        private Expression xexp;
        private Expression yexp;

        public override PredictableBulletModelBase BuildBulletModelInContext(Func<string, float> Indexer, int currentTime)
        {
            v1exp = v1exp?.ToString() != Velocity1Expression ? new Expression(Velocity1Expression) : v1exp;
            r1exp = r1exp?.ToString() != Rotation1Expression ? new Expression(Rotation1Expression) : r1exp;
            texp = texp?.ToString() != TimeExpression ? new Expression(TimeExpression) : texp;
            v2exp = v2exp?.ToString() != Velocity2Expression ? new Expression(Velocity2Expression) : v2exp;
            r2exp = r2exp?.ToString() != Rotation2Expression ? new Expression(Rotation2Expression) : r2exp;
            xexp = xexp?.ToString() != XExpression ? new Expression(XExpression) : xexp;
            yexp = yexp?.ToString() != YExpression ? new Expression(YExpression) : yexp;

            float r1 = r1exp.Calculate(Indexer);
            float v1 = v1exp.Calculate(Indexer);
            float vx1 = Convert.ToSingle(v1 * Math.Cos(r1 * Math.PI / 180f));
            float vy1 = Convert.ToSingle(v1 * Math.Sin(r1 * Math.PI / 180f));

            float r2 = r2exp.Calculate(Indexer);
            float v2 = v2exp.Calculate(Indexer);
            float vx2 = Convert.ToSingle(v2 * Math.Cos(r2 * Math.PI / 180f));
            float vy2 = Convert.ToSingle(v2 * Math.Sin(r2 * Math.PI / 180f));

            return new TwoSegmentPredictableModel()
            {
                Style = Style,
                Color = Color,
                LifeTimeBegin = currentTime,
                LifeTimeEnd = PredictableObjectModelBase.infinite,
                InitX = xexp.Calculate(Indexer),
                InitY = yexp.Calculate(Indexer),
                Rotation1 = r1,
                Rotation2 = r2,
                VX1 = vx1,
                VY1 = vy1,
                T1 = Convert.ToInt32(texp.Calculate(Indexer)),
                VX2 = vx2,
                VY2 = vy2
            };
        }
    }
}
