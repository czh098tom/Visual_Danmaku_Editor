using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Latticework.UnityEngine.UI
{
    public class MiniWindowContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler PointerEnter;
        public event EventHandler PointerExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter?.Invoke(this, null);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit?.Invoke(this, null);
        }
    }
}
