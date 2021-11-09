using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;
using VisualDanmakuEditor.Porting;

namespace Assets
{
    class A<T> { }

    class PortionTest : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log(DefaultExportingClassMapper.GetMappersOfType(typeof(LinearVariable))[0].Bind(new LinearVariable()
            {
                VariableName = "x",
                Begin = "0",
                End = "10",
                IsPrecisely = false
            }));
        }
    }
}
