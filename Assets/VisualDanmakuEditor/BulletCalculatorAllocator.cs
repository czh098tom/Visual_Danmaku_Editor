using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public class BulletCalculatorAllocator : MonoBehaviour
    {
        [SerializeField]
        GameObject bulletCalculatorAndPool;

        List<BulletCalculator> calculators = new List<BulletCalculator>();

        public IReadOnlyList<BulletCalculator> Calculators { get => calculators; }

        private void Awake()
        {
            calculators.AddRange(GetComponentsInChildren<BulletCalculator>());
        }

        public BulletCalculator Add(TaskModel task = null)
        {
            BulletCalculator calc = Instantiate(bulletCalculatorAndPool).GetComponent<BulletCalculator>();
            if (task != null) calc.AssignAndUpdateUI(task);
            calc.transform.SetParent(transform);
            calc.transform.localScale = Vector3.one;
            return calc;
        }

        public void Remove(BulletCalculator calc)
        {
            calculators.Remove(calc);
            Destroy(calc.gameObject);
        }
    }
}
