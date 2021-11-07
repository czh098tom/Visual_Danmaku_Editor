using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.Bullet
{
    public abstract class ModelWithFixedStyle : BulletModelBase
    {
        public string Style { get; set; } = "ball_mid";
        public string Color { get; set; } = "COLOR_RED";
    }
}
