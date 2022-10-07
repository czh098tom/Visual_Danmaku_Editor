using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public class TaskUIAllocator : MonoBehaviour
    {
        [SerializeField]
        Mask viewPort;

        ScrollRect scrollRect;

        TaskUI ui = null;
        GameObject content = null;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            content = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        }

        public void Set(TaskModel model)
        {
            if (model != null)
            {
                if (ui)
                {
                    Destroy(ui.gameObject);
                }
                else
                {
                    Destroy(content);
                }
                ui = Instantiate(UIPrototypes.Instance.TaskUI).GetComponent<TaskUI>();
                ui.transform.SetParent(viewPort.transform, false);
                scrollRect.content = ui.GetComponent<RectTransform>();
                content = ui.gameObject;
                ui.AssignAndUpdateUI(model);
            }
            else
            {
                if (ui)
                {
                    Destroy(ui.gameObject);
                    ui = null;
                    content = Instantiate(UIPrototypes.Instance.EmptyContent);
                    content.transform.SetParent(viewPort.transform, false);
                }
            }
        }
    }
}
