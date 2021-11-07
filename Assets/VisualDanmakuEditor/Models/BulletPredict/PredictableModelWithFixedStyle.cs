using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.BulletPredict
{
    public abstract class PredictableModelWithFixedStyle : PredictableBulletModelBase
    {
        public string Style { get; set; }
        public string Color { get; set; }
    }
}
