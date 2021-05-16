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
            if (isAuto)
            {
                slider.value += Time.deltaTime * 60f;
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
