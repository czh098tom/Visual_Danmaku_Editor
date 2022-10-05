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
        public bool ShowVelocity { get; set; } = true;

        Slider slider;
        Button play;
        Toggle velocity;

        bool isAuto = false;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            play = GetComponentsInChildren<Button>().First(b => b.gameObject.name == "Play");
            velocity = GetComponentsInChildren<Toggle>().First(b => b.gameObject.name == "Velocity");
            play.onClick.AddListener(ModeShift);
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
            ShowVelocity = velocity.isOn;
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
