using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Mill5C.Core.Algorithm;
using Mill5C.Core.DataStructures;
using System.Windows.Threading;
using Primitive3DSurfaces;
using System.Windows.Media;
using Mill5C.Core.Materials;

namespace Mill5C.View.Window.Renderers.WPF
{
    public class RealTimeOctreeRenderer : OctreeRendererBase
    {
        
        public RealTimeOctreeRenderer(bool drawCubes)
             : base(drawCubes)
        {
        }

        public override void Initialize(Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            CreatePrimitive(material.Tree.Root);
        }

        public override void AttachEvents(Engine engine)
        {
            material.Tree.NodeSplit += new EventHandler(Material_NodeSplit);
            material.NodeEpsReached += new EventHandler(material_NodeEpsReached);
        }

        private void material_NodeEpsReached(object sender, EventArgs e)
        {
            Node n = sender as Node;
            Dispatcher.Invoke((Action)delegate
            {
                ApplyColor(n, data[n]);
            });
        }

        private void Material_NodeSplit(object sender, EventArgs e)
        {
            Node n = sender as Node;
            Dispatcher.Invoke((Action)delegate
            {
                DrawNode(n, model);
            });
        }

        private void DrawNode(Node node, ModelVisual3D parent)
        {
            var cub = data[node];
            var trs = cub.Transform as Transform3DGroup;
            var scale = trs.Children[0] as ScaleTransform3D;
            if (cubes)
                scale.ScaleX = scale.ScaleY = scale.ScaleZ = node[0].L;
            else
                scale.ScaleX = scale.ScaleY = scale.ScaleZ = node[0].R;

            var trans = trs.Children[2] as TranslateTransform3D;
            trans.OffsetX = node[0].Center.X;
            trans.OffsetY = node[0].Center.Y;
            trans.OffsetZ = node[0].Center.Z;

            ApplyColor(node[0], cub);

            data.Remove(node);
            data.Add(node[0], cub);

            for (int i = 1; i < 8; i++)
            {
                CreatePrimitive(node[i]);
            }
        }



        public override void DetachEvents(Engine engine)
        {

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
