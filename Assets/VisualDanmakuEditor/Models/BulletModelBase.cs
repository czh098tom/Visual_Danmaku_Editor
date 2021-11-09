using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models
{
    public abstract class BulletModelBase : ObjectModelBase
    {
        public bool IsGlobalCoord { get; set; }

        public abstract PredictableBulletModelBase BuildBulletModelInContext(Func<string, float> indexer, int currentTime);

        public override sealed PredictableObjectModelBase BuildModelInContext(Func<string, float> indexer, int currentTime)
        {
            return BuildBulletModelInContext(indexer, currentTime);
        }
    }
}
