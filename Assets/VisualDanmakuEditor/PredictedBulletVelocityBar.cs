using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VisualDanmakuEditor
{
    [ExecuteAlways]
    public class PredictedBulletVelocityBar : MonoBehaviour
    {
        private static float scaling = 0.1f;

        [SerializeField]
        private float length = 1f;

        public float Length { get => length; set => length = value; }

        SpriteRenderer body;
        SpriteRenderer head;

        private void Awake()
        {
            body = GetComponentsInChildren<SpriteRenderer>().First(s => s.gameObject.name == "Body");
            head = GetComponentsInChildren<SpriteRenderer>().First(s => s.gameObject.name == "Head");
        }

        private void LateUpdate()
        {
            float actual = scaling * Length;
            Vector3 sc = body.transform.localScale;
            sc.x = actual;
            body.transform.localScale = sc;
            body.transform.localPosition = Vector3.right * actual / 2;
            head.transform.localPosition = Vector3.right * actual;
        }
    }
}
