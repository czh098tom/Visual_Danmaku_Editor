using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Latticework.UI
{
    [RequireComponent(typeof(CanvasRenderer)), RequireComponent(typeof(RectTransform))]
    public class UILineRenderer : MaskableGraphic
    {
        [SerializeField]
        Vector2[] anchors;
        [SerializeField]
        float thickness = 1f;
        [SerializeField]
        bool isClosedLoop;

        List<Vector2> eCache = new List<Vector2>();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            eCache.Clear();

            eCache.Capacity = anchors.Length;

            var sizeX = rectTransform.rect.width;
            var sizeY = rectTransform.rect.height;

            Vector2 size = new Vector2(sizeX, sizeY);
            float localThickness = thickness / 2;
            for (int i = isClosedLoop ? 0 : 1; i < anchors.Length; i++)
            {
                Vector2 anchor1;
                Vector2 anchor2;

                Vector2 e1;
                Vector2 e2;

                if (!isClosedLoop)
                {
                    anchor1 = anchors[i - 1] * size - size / 2;
                    anchor2 = anchors[i] * size - size / 2;
                }
                else
                {
                    anchor1 = anchors[(i + anchors.Length - 1) % anchors.Length] * size - size / 2;
                    anchor2 = anchors[i] * size - size / 2;
                }

                Vector2 vec = anchor2 - anchor1;

                if (eCache.Count == 0)
                {
                    Vector2? anchorPrev;
                    if (!isClosedLoop)
                    {
                        anchorPrev = i - 2 < 0 ? null : anchors[i - 2] * size - size / 2;
                    }
                    else
                    {
                        anchorPrev = anchors[(i + anchors.Length - 2) % anchors.Length] * size - size / 2;
                    }
                    Vector2? vecPrev = anchorPrev != null ? anchorPrev.Value - anchor1 : null;
                    Vector2? normalizedEdge1 = vecPrev != null ? (vecPrev.Value.normalized + vec.normalized).normalized : null;
                    normalizedEdge1 = normalizedEdge1 != null ? normalizedEdge1.Value * Mathf.Sign(Vector3.Cross(-vecPrev.Value, vec).z)
                        : new Vector2(-vec.y, vec.x);
                    e1 = localThickness * vec.magnitude * normalizedEdge1.Value / Mathf.Abs(Vector3.Cross(vec, normalizedEdge1.Value).z);
                    eCache.Add(e1);
                }
                else
                {
                    e1 = eCache[eCache.Count - 1];
                }

                Vector2? anchorNext;
                if (!isClosedLoop)
                {
                    anchorNext = i + 1 >= anchors.Length ? null : anchors[i + 1] * size - size / 2;
                }
                else
                {
                    anchorNext = anchors[(i + 1) % anchors.Length] * size - size / 2;
                }
                Vector2? vecNext = anchorNext != null ? anchorNext.Value - anchor2 : null;
                Vector2? normalizedEdge2 = vecNext != null ? (vecNext.Value.normalized - vec.normalized).normalized : null;
                normalizedEdge2 = normalizedEdge2 != null ? normalizedEdge2.Value * Mathf.Sign(Vector3.Cross(vec, vecNext.Value).z)
                    : new Vector2(-vec.y, vec.x);
                e2 = localThickness * vec.magnitude * normalizedEdge2.Value / Mathf.Abs(Vector3.Cross(vec, normalizedEdge2.Value).z);
                eCache.Add(e2);

                UIVertex colored = UIVertex.simpleVert;
                colored.color = color;
                UIVertex v1 = colored, v2 = colored, v3 = colored, v4 = colored;
                v1.position = anchor1 + e1;
                v2.position = anchor2 + e2;
                v3.position = anchor2 - e2;
                v4.position = anchor1 - e1;

                vh.Add4Vert(v1, v2, v3, v4);
            }
        }
    }
}
