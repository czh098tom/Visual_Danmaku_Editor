using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

using Latticework.Graph;
using Latticework.UnityEngine.Utilities;

namespace Assets.VisualDanmakuEditor.Graph
{
    using Graph = Latticework.Graph.Graph;

    public class GraphProvider : Singleton<GraphProvider>
    {
        public Graph Graph { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeBlockDef();
            Graph = new Graph();
        }

        private void InitializeBlockDef()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "Blocks/BlockTypes.json"));
                Definition<BlockDefinition>.LoadDefinition(sr.ReadToEnd());
            }
            finally
            {
                sr?.Close();
            }
            try
            {
                sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "Blocks/PinTypes.json"));
                Definition<PinDefinition>.LoadDefinition(sr.ReadToEnd());
            }
            finally
            {
                sr?.Close();
            }
        }
    }
}
