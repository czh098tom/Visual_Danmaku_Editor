using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Latticework.UnityEngine.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(InputLabelRegulator))]
    public class LabelledInput : MonoBehaviour
    {
        Text labelText;

        public string Label { get => labelText.text; }
        public InputField InputComponent { get; private set; }
        public string Value { get => InputComponent.text; set => InputComponent.text = value; }

        private void Awake()
        {
            InputComponent = GetComponentInChildren<InputField>();
            labelText = GetComponentsInChildren<Text>().First(t => InputComponent.textComponent != t);
        }
    }
}
