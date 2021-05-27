using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace VisualDanmakuEditor
{
    public class TimeLine : MonoBehaviour
    {
        public static TimeLine Instance { get; private set; }

        public int CurrentTime { get; private set; }
        public int MaxTime { get; set; } = 400;
        Slider slider;
        Button button;

        bool isAuto = false;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(ModeShift);
            Instance = this;
        }

        private void Update()
        {
            slider.maxValue = MaxTime;
            if (isAuto)
            {
                float tEx = Time.deltaTime * 60f;
                if (slider.value + tEx > slider.maxValue) tEx -= slider.maxValue;
                slider.value += tEx;
            }
            CurrentTime = Convert.ToInt32(slider.value);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, Screen.height - 40, 100, 20), CurrentTime.ToString());
        }

        public void ModeShift()
        {
            isAuto = !isAuto;
        }
    }
}
