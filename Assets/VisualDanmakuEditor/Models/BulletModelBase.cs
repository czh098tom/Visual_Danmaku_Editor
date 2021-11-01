using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models
{
    public class BulletModelBase
    {
        public string Style { get; set; } = "ball_mid";
        public string Color { get; set; } = "COLOR_RED";

        public string XExpression { get; set; } = "0";
        public string YExpression { get; set; } = "0";
        public string VelocityExpression { get; set; } = "3";
        public string RotationExpression { get; set; } = "0";
    }
}
