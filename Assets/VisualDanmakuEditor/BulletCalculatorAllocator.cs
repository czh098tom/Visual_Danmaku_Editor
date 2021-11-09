using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public BulletCalculator Add()
        {
            BulletCalculator calc = Instantiate(bulletCalculatorAndPool).GetComponent<BulletCalculator>();
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
