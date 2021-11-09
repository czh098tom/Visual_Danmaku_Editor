using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualDanmakuEditor.Models.Predictables;

namespace VisualDanmakuEditor.Models.Objects
{
    public class FixedObject : ObjectModelBase
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override PredictableObjectModelBase BuildModelInContext(Func<string, float> indexer, int currentTime)
        {
            return new PredictableFixedObject()
            {
                LifeTimeBegin = -PredictableObjectModelBase.infinite,
                LifeTimeEnd = PredictableObjectModelBase.infinite,
                InitX = X,
                InitY = Y
            };
        }
    }
}
