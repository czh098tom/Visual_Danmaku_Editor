using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Latticework.UnityEngine.UI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent, RequireComponent(typeof(LabelledInput))]
    public class InputLabelRegulator : MonoBehaviour
    {
        [SerializeField]
        string labelContent;
        [SerializeField, Range(0f, 1f)]
        float labelPartition = 0.3f;

        InputField input;
        RectTransform inputTransform;
        Text label;
        RectTransform labelTransform;

        private void Awake()
        {
            input = GetComponentInChildren<InputField>();
            label = GetComponentsInChildren<Text>().First(t => input.textComponent != t);
            inputTransform = input.GetComponent<RectTransform>();
            labelTransform = label.GetComponent<RectTransform>();
        }

        private void Update()
        {
            gameObject.name = labelContent;
            label.text = labelContent;
            labelTransform.anchorMin = Vector2.zero;
            labelTransform.anchorMax = new Vector2(labelPartition, 1f);
            inputTransform.anchorMin = new Vector2(labelPartition, 0f);
            inputTransform.anchorMax = Vector2.one;
        }
    }
}
