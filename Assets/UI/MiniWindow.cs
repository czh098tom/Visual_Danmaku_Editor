using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Latticework.UnityEngine.UI
{
    public class MiniWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        enum DragAnchor
        {
            BottomLeft = -4,
            Bottom = -3,
            BottomRight = -2,
            Left = -1,
            Center = 0,
            Right = 1,
            TopLeft = 2,
            Top = 3,
            TopRight = 4
        }

        struct DragInfo
        {
            public DragAnchor Anchor { get; private set; }
            public Vector2 AnchorPos { get; private set; }
            public Vector2 RelPos { get; private set; }

            public DragInfo(DragAnchor anchor, Vector2 anchorPos, Vector2 relPos)
            {
                Anchor = anchor;
                AnchorPos = anchorPos;
                RelPos = relPos;
            }
        }

        [SerializeField]
        Vector2 minSize = new Vector2(800, 600);
        [SerializeField]
        Vector2 maxSize = new Vector2(1024, 768);
        [SerializeField]
        float borderWidth = 5f;

        public float BorderWidth { get => borderWidth; }

        RectTransform canvas;

        RectTransform rectTransform;
        bool isOverThis = false;

        Vector3[] fourCornersArray = new Vector3[4];

        DragInfo? dragInfo = null;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.GetLocalCorners(fourCornersArray);
            if (isOverThis)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    transform.SetAsLastSibling();

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out Vector2 mouse);

                    Vector2 anchoredPosition = rectTransform.anchoredPosition;
                    Rect rect = rectTransform.rect;

                    dragInfo = mouse switch
                    {
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 &&
                            x < anchoredPosition.x - rect.width / 2 + borderWidth &&
                            y > anchoredPosition.y - rect.height / 2 &&
                            y < anchoredPosition.y - rect.height / 2 + borderWidth
                            => new DragInfo(DragAnchor.BottomLeft, rectTransform.offsetMin, mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 + borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 - borderWidth &&
                            y > anchoredPosition.y - rect.height / 2 &&
                            y < anchoredPosition.y - rect.height / 2 + borderWidth
                            => new DragInfo(DragAnchor.Bottom, rectTransform.offsetMin, mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x + rect.width / 2 - borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 &&
                            y > anchoredPosition.y - rect.height / 2 &&
                            y < anchoredPosition.y - rect.height / 2 + borderWidth
                            => new DragInfo(DragAnchor.BottomRight, new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMin.y), mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 &&
                            x < anchoredPosition.x - rect.width / 2 + borderWidth &&
                            y > anchoredPosition.y - rect.height / 2 + borderWidth &&
                            y < anchoredPosition.y + rect.height / 2 - borderWidth
                            => new DragInfo(DragAnchor.Left, rectTransform.offsetMin, mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 + borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 - borderWidth &&
                            y > anchoredPosition.y - rect.height / 2 + borderWidth &&
                            y < anchoredPosition.y + rect.height / 2 - borderWidth
                            => new DragInfo(DragAnchor.Center
                                , anchoredPosition
                                , mouse - anchoredPosition),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x + rect.width / 2 - borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 &&
                            y > anchoredPosition.y - rect.height / 2 + borderWidth &&
                            y < anchoredPosition.y + rect.height / 2 - borderWidth
                            => new DragInfo(DragAnchor.Right, rectTransform.offsetMax, mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 &&
                            x < anchoredPosition.x - rect.width / 2 + borderWidth &&
                            y > anchoredPosition.y + rect.height / 2 - borderWidth &&
                            y < anchoredPosition.y + rect.height / 2
                            => new DragInfo(DragAnchor.TopLeft, new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.y), mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x - rect.width / 2 + borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 - borderWidth &&
                            y > anchoredPosition.y + rect.height / 2 - borderWidth &&
                            y < anchoredPosition.y + rect.height / 2
                            => new DragInfo(DragAnchor.Top, rectTransform.offsetMax, mouse),
                        { x: var x, y: var y } when
                            x > anchoredPosition.x + rect.width / 2 - borderWidth &&
                            x < anchoredPosition.x + rect.width / 2 &&
                            y > anchoredPosition.y + rect.height / 2 - borderWidth &&
                            y < anchoredPosition.y + rect.height / 2
                            => new DragInfo(DragAnchor.TopRight, rectTransform.offsetMax, mouse),
                        _ => null
                    };
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    dragInfo = null;
                }
            }
            if (dragInfo != null)
            {
                DragInfo dragInfo = this.dragInfo.Value;
                Rect r = new Rect(0, 0, Screen.width, Screen.height);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out Vector2 mouse);
                if (r.Contains(Input.mousePosition))
                {
                    Vector2 calculatedOffset = dragInfo.AnchorPos + mouse - dragInfo.RelPos;
                    rectTransform.offsetMin = dragInfo.Anchor switch
                    {
                        DragAnchor.BottomLeft => calculatedOffset,
                        DragAnchor.Bottom or DragAnchor.BottomRight => new Vector2(rectTransform.offsetMin.x, calculatedOffset.y),
                        DragAnchor.Left or DragAnchor.TopLeft => new Vector2(calculatedOffset.x, rectTransform.offsetMin.y),
                        _ => rectTransform.offsetMin
                    };
                    rectTransform.offsetMax = dragInfo.Anchor switch
                    {
                        DragAnchor.TopRight => calculatedOffset,
                        DragAnchor.Top or DragAnchor.TopLeft => new Vector2(rectTransform.offsetMax.x, calculatedOffset.y),
                        DragAnchor.Right or DragAnchor.BottomRight => new Vector2(calculatedOffset.x, rectTransform.offsetMax.y),
                        _ => rectTransform.offsetMax
                    };
                    if (dragInfo.Anchor == DragAnchor.Center) rectTransform.anchoredPosition = mouse - dragInfo.RelPos;
                }
            }
        }

        private void ShowInfo()
        {
            rectTransform.GetLocalCorners(fourCornersArray);
            Debug.Log($@"anchoredPosition: {rectTransform.anchoredPosition}
anchoredPosition3D {rectTransform.anchoredPosition3D}
anchorMax {rectTransform.anchorMax}
anchorMin {rectTransform.anchorMin}
offsetMax {rectTransform.offsetMax}
offsetMin {rectTransform.offsetMin}
pivot {rectTransform.pivot}
rect {rectTransform.rect}
sizeDelta {rectTransform.sizeDelta}
Corner LB {fourCornersArray[0]}
Corner LT {fourCornersArray[1]}
Corner RT {fourCornersArray[2]}
Corner RB {fourCornersArray[3]}");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                isOverThis = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != gameObject)
            {
                if (dragInfo == null) isOverThis = false;
            }
        }
    }
}
