using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.Graph;

namespace VisualDanmakuEditor.Graph
{
    public class BlockDisplayer : MonoBehaviour
    {
        public Block BlockRef { get; }

        private void Update()
        {
            transform.localPosition = new Vector3(BlockRef.X, BlockRef.Y, 0);
        }
    }
}
