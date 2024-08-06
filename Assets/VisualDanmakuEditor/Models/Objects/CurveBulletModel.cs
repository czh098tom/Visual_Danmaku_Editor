using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.Curve;
using VisualDanmakuEditor.Models.Objects;
using VisualDanmakuEditor.Models.Predictables;

namespace VisualDanmakuEditor.Models.Objects
{
    public class CurveBulletModel : ModelWithFixedStyle
    {
        public string XExpression { get; set; } = "0";
        public string YExpression { get; set; } = "0";
        public List<CurvePoint> TVExpression { get; set; }
        public List<CurvePoint> TRExpression { get; set; }

        private Expression xexp;
        private Expression yexp;
        private (Expression t, Expression v)[] tvexp;
        private (Expression t, Expression r)[] trexp;

        public override PredictableBulletModelBase BuildBulletModelInContext(Func<string, float> indexer, int currentTime)
        {
            xexp = xexp?.ToString() != XExpression ? new Expression(XExpression) : xexp;
            yexp = yexp?.ToString() != YExpression ? new Expression(YExpression) : yexp;

            if (tvexp?.Length != TVExpression.Count) tvexp = new (Expression, Expression)[TVExpression.Count];
            if (trexp?.Length != TRExpression.Count) trexp = new (Expression, Expression)[TRExpression.Count];

            for (int i = 0; i < TVExpression.Count; i++)
            {
                if (tvexp[i].t?.ToString() != TVExpression[i].Time || tvexp[i].v?.ToString() != TVExpression[i].Value)
                {
                    tvexp[i] = (new Expression(TVExpression[i].Time), new Expression(TVExpression[i].Value));
                }
            }
            for (int i = 0; i < TRExpression.Count; i++)
            {
                if (trexp[i].t?.ToString() != TRExpression[i].Time || trexp[i].r?.ToString() != TRExpression[i].Value)
                {
                    trexp[i] = (new Expression(TRExpression[i].Time), new Expression(TRExpression[i].Value));
                }
            }

            (int t, float v)[] tv = new (int, float)[tvexp.Length];
            (int t, float r)[] tr = new (int, float)[trexp.Length];

            for (int i = 0; i < tv.Length; i++)
            {
                tv[i] = (Convert.ToInt32(tvexp[i].t.Calculate(indexer)), tvexp[i].v.Calculate(indexer));
            }
            for (int i = 0; i < tr.Length; i++)
            {
                tr[i] = (Convert.ToInt32(trexp[i].t.Calculate(indexer)), trexp[i].r.Calculate(indexer));
            }

            List<(int t, float v, float r)> tvr = new List<(int t, float v, float r)>();

            for (int i = 0; i < tv.Length; i++)
            {
                tvr.Add((tv[i].t, tv[i].v, 0));
            }
            int itr = 0;
            int itvr = 0;
            List<int> unSupported = new List<int>();
            while (itr < tr.Length && itvr < tvr.Count)
            {
                if (tr[itr].t < tvr[itvr].t)
                {
                    tvr.Insert(itvr,
                        (tr[itr].t,
                        (tr[itr].t - tvr[itvr - 1].t) * (tvr[itvr].v - tvr[itvr - 1].v) / (tvr[itvr].t - tvr[itvr - 1].t) + tvr[itvr - 1].v,
                        tr[itr].r));
                    itr++;
                    itvr++;
                }
                else
                {
                    if (tr[itr].t == tvr[itvr].t)
                    {
                        tvr[itvr] = (tvr[itvr].t, tvr[itvr].v, tr[itr].r);
                        itr++;
                        itvr++;
                    }
                    else
                    {
                        tvr[itvr] = (tvr[itvr].t, 
                            tvr[itvr].v,
                            (tvr[itvr].t - tr[itr - 1].t) * (tr[itr].r - tr[itr - 1].r) / (tr[itr].t - tr[itr - 1].t) + tr[itr - 1].r);
                        itvr++;
                    }
                }
            }
            var lastItvr = itvr - 1;
            while (itvr < tvr.Count)
            {
                tvr[itvr] = (tvr[itvr].t,
                    tvr[itvr].v,
                    tvr[lastItvr].r);
                itvr++;
            }
            while (itr < tr.Length)
            {
                tvr.Add((tr[itr].t, tvr[lastItvr].v, tr[itr].r));
                itr++;
            }

            return new PredictableCurveBulletModel()
            {
                Style = Style,
                Color = Color,
                InitX = xexp.Calculate(indexer),
                InitY = yexp.Calculate(indexer),
                LifeTimeBegin = currentTime,
                LifeTimeEnd = PredictableObjectModelBase.infinite,
                V_R = tvr
            };
        }

        public IEnumerable<string> GetVariables()
        {
            return new Expression(XExpression).GetVariables()
                .Concat(new Expression(YExpression).GetVariables())
                .Concat(TVExpression.SelectMany(c => new Expression(c.Time).GetVariables()
                    .Concat(new Expression(c.Value).GetVariables())))
                .Concat(TRExpression.SelectMany(c => new Expression(c.Time).GetVariables()
                    .Concat(new Expression(c.Value).GetVariables())))
                .Distinct();
        }
    }
}
