using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Mill5C.Core.DataStructures;
using Primitive3DSurfaces;
using System.Windows.Media;
using Mill5C.Core.Algorithm;
using System.Windows.Threading;
using Mill5C.Core.Materials;

namespace Mill5C.View.Window.Renderers.WPF
{
    public class PostSimulationOctreeRenderer : OctreeRendererBase
    {
        public PostSimulationOctreeRenderer(bool drawCubes)
             : base(drawCubes)
        {
        }

        public override void Initialize(Engine engine, object scene)
        {
            base.Initialize(engine, scene);
        }

        public override void AttachEvents(Engine engine)
        {
            engine.WorkComplete += new EventHandler(Strategy_PathComplete);
        }

        public override void DetachEvents(Engine engine)
        {
            engine.WorkComplete -= new EventHandler(Strategy_PathComplete);
        }

        private void Strategy_PathComplete(object sender, EventArgs e)
        {
            Render();
        }

        public void Render()
        {
            Dispatcher.Invoke(new Action(delegate
            {
                DrawNode(material.Tree.Root, model);
            }));
        }

        private void DrawNode(Node node, ModelVisual3D parent)
        {
            if (node.Color == NodeColor.White)
                return;

            if (node.Children == null)
            {
                CreatePrimitive(node);
            }
            else
            {
                foreach (var child in node.Children)
                {
                    DrawNode(child, parent);
                }
            }

        }

        public override bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
