using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models
{
    public class BulletPrediction
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Rotation { get; set; }

        public bool ContainsInvalidParameters()
        {
            return float.IsNaN(X) || float.IsNaN(Y) || float.IsNaN(Rotation)
                || float.IsInfinity(X) || float.IsInfinity(Y) || float.IsInfinity(Rotation);
        }
    }
}
