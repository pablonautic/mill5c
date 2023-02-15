using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstancedModelSample;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mill5C.Core.DataStructures;
using Mill5C.Core.Materials;

namespace Mill5C.View.Window.Renderers.XNA
{
    public abstract class OctreeRendererBase : XNARendererBase
    {
        public bool Cubes { get; private set; }

        protected InstancedModel cube;

        protected Matrix[] matrices;

        public OctreeRendererBase(bool cubes)
        {
            Cubes = cubes;
        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            string name;

            if (Cubes)
                name = "Cats";
            else
                name = "sphere";

            cube = RenderingControl.ContentManager.Load<InstancedModel>(name);
            cube.SetInstancingTechnique(InstancingTechnique.ShaderInstancing);
            cube.SetEffectParam("LightDirection", new Vector3(0, 0, -1));
        }

        public override void Draw()
        {
            if (matrices == null || !Visible)
                return;

            base.Draw();
           
            cube.DrawInstances(matrices, RenderingControl.View, RenderingControl.Projection);

            RenderingControl.SpriteBatch.Begin();
            RenderingControl.SpriteBatch.DrawString(font, String.Format("Cubes count: {0}", matrices.Length)
                , new Vector2(15, 15), Color.DimGray);
            RenderingControl.SpriteBatch.End();
        }

        protected void Transverse()
        {
            var mat = (OctreeMaterial)Engine.Material;

            List<Matrix> nodeMatrices = new List<Matrix>();
            FillList(mat.Tree.Root, nodeMatrices);
            matrices = nodeMatrices.ToArray();
        }

        protected void FillList(Node node, List<Matrix> nodeMatrices)
        {
            if (node == null || node.Color == NodeColor.White)
                return;

            if (node.Children == null)
            {
                nodeMatrices.Add(Matrix.CreateScale(Cubes ?  2 * node.L : 1.0f / 4.8f * node.L) *
                    Matrix.CreateTranslation(node.Center.X, node.Center.Y, node.Center.Z));
            }
            else
            {
                foreach (var child in node.Children)
                {
                    FillList(child, nodeMatrices);
                }
            }

        }
    }
}
