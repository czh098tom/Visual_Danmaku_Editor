using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Latticework.UI
{
    public static class VertexHelperExtension
    {
        public static void Add4Vert(this VertexHelper vh, UIVertex v1, UIVertex v2, UIVertex v3, UIVertex v4)
        {
            int c = vh.currentVertCount;

            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);
            vh.AddVert(v4);

            vh.AddTriangle(c, c + 1, c + 2);
            vh.AddTriangle(c, c + 2, c + 3);
        }
    }
}
