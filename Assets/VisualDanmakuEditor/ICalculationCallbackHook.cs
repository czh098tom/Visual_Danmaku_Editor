using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor
{
    public interface ICalculationCallbackHook
    {
        Action Calculate { get; set; }
    }
}
