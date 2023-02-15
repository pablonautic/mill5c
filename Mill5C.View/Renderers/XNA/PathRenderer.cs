using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mill5C.Core.Path;

namespace Mill5C.View.Window.Renderers.XNA
{
    public class PathRenderer : XNARendererBase
    {
        private VertexPositionColor[] vertexData;

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            DrawPath(engine.CurrentPath);
        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathPrepared += engine_PathPrepared;
        }

        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathPrepared -= engine_PathPrepared;
        }

        private void engine_PathPrepared(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            DrawPath(args.Path);
        }

        private void DrawPath(Path path)
        {
            vertexData = new VertexPositionColor[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                vertexData[i] = new VertexPositionColor(
                    new Microsoft.Xna.Framework.Vector3(
                        path[i].Position.X,
                        path[i].Position.Y,
                        path[i].Position.Z), Color.BurlyWood);
            }
        }

        public override void Draw()
        {
            if (!Visible || vertexData.Length <= 1)
                return;

            base.Draw();

            RenderingControl.GraphicsDevice.RenderState.DepthBufferEnable = false;

            RenderingControl.GraphicsDevice.VertexDeclaration = colorVD;

            //RenderingControl.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //RenderingControl.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            //RenderingControl.GraphicsDevice.RenderState.AlphaBlendEnable = true;

            //colorEffect.Alpha = 0.7f;

            colorEffect.Begin();

            foreach (EffectPass pass in colorEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                RenderingControl.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertexData, 0, vertexData.Length - 1);

                pass.End();
            }

            colorEffect.End();
        }
    }
}
