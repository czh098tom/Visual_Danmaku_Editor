using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models
{
    public abstract class ObjectModelBase
    {
        public abstract PredictableObjectModelBase BuildModelInContext(Func<string, float> indexer, int currentTime);
    }
}
