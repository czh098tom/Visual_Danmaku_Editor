using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.UnityEngine.Utilities;

namespace VisualDanmakuEditor.Models
{
    public class BulletObjectPool : ObjPool<BulletObjectPool, BulletObjectPool.RefManager>
    {
        public class RefManager : ObjPoolRefManager<BulletObjectPool, RefManager> 
        {
            public PredictedBullet PredictedBullet { get; private set; }

            protected override void Awake()
            {
                base.Awake();
                PredictedBullet = Instance.GetComponent<PredictedBullet>();
                PredictedBullet.transform.localScale = UnityEngine.Vector3.one;
            }
        }
    }
}
