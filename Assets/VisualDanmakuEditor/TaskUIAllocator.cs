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
        RectTransform parent;

        TaskUI ui = null;

        public void Set(TaskModel model)
        {
            if (model != null)
            {
                if (ui)
                {
                    Destroy(ui.gameObject);
                }
                ui = Instantiate(UIPrototypes.Instance.TaskUI).GetComponent<TaskUI>();
                ui.transform.SetParent(parent, false);
                ui.AssignAndUpdateUI(model);
            }
            else
            {
                if (ui)
                {
                    Destroy(ui.gameObject);
                    ui = null;
                }
            }
        }
    }
}
