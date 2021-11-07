using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

using VisualDanmakuEditor.Models.BulletPredict;

namespace VisualDanmakuEditor.Models.Bullet
{
    public class SimpleBulletModel : ModelWithFixedStyle
    {
        public string XExpression { get; set; } = "0";
        public string YExpression { get; set; } = "0";
        public string VelocityExpression { get; set; } = "3";
        public string RotationExpression { get; set; } = "0";

        private Expression vexp;
        private Expression rexp;
        private Expression xexp;
        private Expression yexp;

        public override PredictableBulletModelBase BuildModelInContext(Func<string, float> Indexer, int currentTime)
        {
            vexp = vexp?.ToString() != VelocityExpression ? new Expression(VelocityExpression) : vexp;
            rexp = rexp?.ToString() != RotationExpression ? new Expression(RotationExpression) : rexp;
            xexp = xexp?.ToString() != XExpression ? new Expression(XExpression) : xexp;
            yexp = yexp?.ToString() != YExpression ? new Expression(YExpression) : yexp;

            float r = rexp.Calculate(Indexer);
            float v = vexp.Calculate(Indexer);
            float vx = Convert.ToSingle(v * Math.Cos(r * Math.PI / 180f));
            float vy = Convert.ToSingle(v * Math.Sin(r * Math.PI / 180f));

            return new SimplePredictableBulletModel()
            {
                Style = Style,
                Color = Color,
                LifeTimeBegin = currentTime,
                LifeTimeEnd = PredictableBulletModelBase.infinite,
                InitX = xexp.Calculate(Indexer),
                InitY = yexp.Calculate(Indexer),
                Rotation = r,
                VX = vx,
                VY = vy
            };
        }
    }
}
