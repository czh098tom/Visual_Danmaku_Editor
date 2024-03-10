using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.Predictables;

namespace VisualDanmakuEditor.Models.Predictables
{
    internal class PredictableCurveBulletModel : PredictableModelWithFixedStyle
    {
        public List<(int, float, float)> V_R { get; set; }

        public override BulletPrediction GetBulletPredictionAt(int time)
        {
            if (time < LifeTimeBegin || time > LifeTimeEnd) return null;
            var prediction = new BulletPrediction
            {
                Style = Style,
                Color = Color,
                X = InitX,
                Y = InitY,
            };
            int t = time - LifeTimeBegin;

            for (int i = 0; i < V_R.Count - 1; i++)
            {
                float v = V_R[i].Item2;
                float a = (V_R[i + 1].Item2 - V_R[i].Item2) / (V_R[i + 1].Item1 - V_R[i].Item1);
                float phi = Convert.ToSingle(V_R[i].Item3 * Math.PI / 180);
                float omega = Convert.ToSingle((V_R[i + 1].Item3 - V_R[i].Item3) / (V_R[i + 1].Item1 - V_R[i].Item1) * Math.PI / 180);
                if (t >= V_R[i].Item1 && t < V_R[i + 1].Item1)
                {
                    prediction.X += X_t(t - V_R[i].Item1, a, v, omega, phi);
                    prediction.Y += Y_t(t - V_R[i].Item1, a, v, omega, phi);
                    prediction.Rotation = (t - V_R[i].Item1) / (float)(V_R[i + 1].Item1 - V_R[i].Item1) 
                        * (V_R[i + 1].Item3 - V_R[i].Item3) + V_R[i].Item3;
                    return prediction;
                }
                else
                {
                    prediction.X += X_t(V_R[i + 1].Item1 - V_R[i].Item1, a, v, omega, phi);
                    prediction.Y += Y_t(V_R[i + 1].Item1 - V_R[i].Item1, a, v, omega, phi);
                }
            }
            float vx = V_R[V_R.Count - 1].Item2 * Convert.ToSingle(Math.Cos(V_R[V_R.Count - 1].Item3 * Math.PI / 180));
            float vy = V_R[V_R.Count - 1].Item2 * Convert.ToSingle(Math.Sin(V_R[V_R.Count - 1].Item3 * Math.PI / 180));
            prediction.X += vx * (t - V_R[V_R.Count - 1].Item1);
            prediction.Y += vy * (t - V_R[V_R.Count - 1].Item1);
            prediction.Rotation = V_R[V_R.Count - 1].Item3;
            return prediction;
        }

        private float X_t(int t, float a, float v, float omega, float phi)
        {
            if (omega == 0)
            {
                return Convert.ToSingle((v * t + a * t * t / 2) * Math.Cos(phi));
            }
            return Convert.ToSingle((omega * Math.Sin(phi + omega * t) * (a * t + v) + a * Math.Cos(phi + omega * t)
                - omega * v * Math.Sin(phi) - a * Math.Cos(phi)) / omega / omega);
        }

        private float Y_t(int t, float a, float v, float omega, float phi)
        {
            if (omega == 0)
            {
                return Convert.ToSingle((v * t + a * t * t / 2) * Math.Sin(phi));
            }
            return Convert.ToSingle((-omega * Math.Cos(phi + omega * t) * (a * t + v) + a * Math.Sin(phi + omega * t)
                + omega * v * Math.Cos(phi) - a * Math.Sin(phi)) / omega / omega);
        }
    }
}
