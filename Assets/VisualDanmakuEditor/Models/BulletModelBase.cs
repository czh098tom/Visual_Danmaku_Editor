using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models
{
    public abstract class BulletModelBase
    {
        public abstract PredictableBulletModelBase BuildModelInContext(Func<string, float> Indexer, int currentTime);
    }
}
