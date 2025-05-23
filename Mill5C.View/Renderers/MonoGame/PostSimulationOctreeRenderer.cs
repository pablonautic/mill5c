using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics; // Not strictly needed if base handles all Graphics types
using Mill5C.Core.Materials;
using Mill5C.Core.DataStructures;
using Microsoft.Xna.Framework;
using InstancedModelSample; // CRITICAL_DEPENDENCY_ALERT

namespace Mill5C.View.Window.Renderers.MonoGame // Changed namespace
{
    // Ensure it inherits from OctreeRendererBase from the same (MonoGame) namespace
    public class PostSimulationOctreeRenderer : OctreeRendererBase 
    {
           
        public PostSimulationOctreeRenderer(bool cubes)
            : base(cubes)
        {
        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.WorkComplete += new EventHandler(Strategy_WorkComplete);
        }

        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.WorkComplete -= new EventHandler(Strategy_WorkComplete);
        }
     
        private void Strategy_WorkComplete(object sender, EventArgs e)
        {
            Transverse();
        }

    }
}
