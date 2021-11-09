using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.Predictables
{
    public class PredictableFixedObject : PredictableObjectModelBase
    {
        public override ObjectPrediction GetPredictionAt(int time)
        {
            return new ObjectPrediction
            {
                X = InitX,
                Y = InitY,
                Rotation = 0
            };
        }
    }
}
