using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public class PredictedObject : MonoBehaviour
    {
        public PredictableObjectModelBase ObjectModel { get; set; }

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ShowPrediction();
        }

        private void Update()
        {
            ShowPrediction();
        }

        private void ShowPrediction()
        {
            if (ObjectModel != null)
            {
                int t = TimeLine.Instance.CurrentTime;
                if (t >= ObjectModel.LifeTimeBegin && t < ObjectModel.LifeTimeEnd)
                {
                    spriteRenderer.enabled = true;
                    ObjectPrediction prediction = ObjectModel.GetPredictionAt(t);
                    if (prediction.ContainsInvalidParameters()) return;
                    transform.localPosition = new Vector3(prediction.X, prediction.Y);
                    transform.rotation = Quaternion.Euler(0, 0, prediction.Rotation);
                }
                else
                {
                    spriteRenderer.enabled = false;
                }
            }
        }
    }
}
