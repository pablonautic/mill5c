using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Materials;
using Microsoft.Xna.Framework;
using Mill5C.Core.DataStructures;

namespace Mill5C.View.Window.Renderers.MonoGame // Changed namespace
{
    public class RealTimeOctreeRenderer : OctreeRendererBase // Ensure it inherits from OctreeRendererBase in the same namespace
    {
        private const int refreshRate = 10; // This logic might need rethinking without Invalidate
        private int refreshCounter;
        
        public RealTimeOctreeRenderer(bool cubes)
            : base(cubes)
        {
        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);
            if (engine != null) // Ensure engine is not null before accessing Material
            {
                Transverse(); // Transverse can be called if Engine.Material is valid
            }
        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathProcessingStarted += engine_PathPrepared;
            engine.PathCompleted += engine_PathCompleted;
        }


        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathProcessingStarted -= engine_PathPrepared;
            engine.PathCompleted -= engine_PathCompleted;
        }

        private void engine_PathPrepared(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            if (Engine?.Strategy?.ReferenceCutter != null) // Null checks
            {
                Engine.Strategy.ReferenceCutter.ConfigurationChanged += Cutter_ConfigurationChanged;
            }
        }

        private void engine_PathCompleted(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            if (Engine?.Strategy?.ReferenceCutter != null) // Null checks
            {
                Engine.Strategy.ReferenceCutter.ConfigurationChanged -= Cutter_ConfigurationChanged;
            }
        }

        private void Cutter_ConfigurationChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            refreshCounter++;
            if (refreshCounter > refreshRate)
            {
                refreshCounter = 0;
                if (Engine != null) // Ensure engine is available
                {
                    Transverse();
                }
                // RenderingControl.Invoke was removed. Game loop handles redraws.
                // GameViewModel.RequestRedraw(); // Or similar if MonoGame.Forms has such a mechanism
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                bool changed = base.Visible != value;
                base.Visible = value;
                if (changed && Engine != null) // Only call Transverse if visibility actually changed
                {
                    Transverse();
                    // RenderingControl.Invoke was removed.
                }
            }
        }
    }
}
