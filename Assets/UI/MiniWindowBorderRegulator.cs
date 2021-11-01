using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Latticework.UnityEngine.UI
{
    [RequireComponent(typeof(MiniWindow))]
    [ExecuteInEditMode]
    public class MiniWindowBorderRegulator : MonoBehaviour
    {
        MiniWindow window;
        RectTransform rectTransformInChildren;

        private void Awake()
        {
            window = GetComponent<MiniWindow>();
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransformInChildren = GetComponentsInChildren<RectTransform>().First(rt => rt != rectTransform);
        }

        private void Update()
        {
            rectTransformInChildren.anchorMin = Vector2.zero;
            rectTransformInChildren.anchorMax = Vector2.one;
            rectTransformInChildren.offsetMin = Vector2.one * window.BorderWidth;
            rectTransformInChildren.offsetMax = -Vector2.one * window.BorderWidth;
        }
    }
}
