using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Latticework.UnityEngine.Utilities
{
    public static class RectExtension
    {
        public static bool Inside(this Vector2 vector, Rect rect) => rect.Contains(vector);

        public static bool Contains(this Rect rect, float x, float y) => rect.Contains(new Vector2(x, y));

        public static Vector2 ClampInside(this Rect rect, Vector2 vector)
            => new Vector2(Mathf.Clamp(vector.x, rect.xMin, rect.xMax), Mathf.Clamp(vector.y, rect.yMin, rect.yMax));
    }
}
