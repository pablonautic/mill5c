using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Materials;
using Microsoft.Xna.Framework;
using Mill5C.Core.DataStructures;

namespace Mill5C.View.Window.Renderers.XNA
{
    public class RealTimeOctreeRenderer : OctreeRendererBase
    {
        private const int refreshRate = 10;

        private int refreshCounter;
        
        public RealTimeOctreeRenderer(bool cubes)
            : base(cubes)
        {

        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);
            Transverse();
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
            Engine.Strategy.ReferenceCutter.ConfigurationChanged += Cutter_ConfigurationChanged;
        }

        private void engine_PathCompleted(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {

            Engine.Strategy.ReferenceCutter.ConfigurationChanged -= Cutter_ConfigurationChanged;
        }

        private void Cutter_ConfigurationChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            refreshCounter++;
            if (refreshCounter > refreshRate)
            {
                refreshCounter = 0;
                Transverse();
                RenderingControl.Invoke(new Action(RenderingControl.Invalidate));
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
                base.Visible = value;
                if (Engine != null)
                {
                    Transverse();
                    RenderingControl.Invoke(new Action(RenderingControl.Invalidate));
                }
            }
        }

    }
}
