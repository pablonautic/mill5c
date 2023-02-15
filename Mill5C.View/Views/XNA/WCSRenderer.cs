using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mill5C.View.Window.Views.XNA;
using Microsoft.Xna.Framework;
using Mill5C.View.Window.Renderers.XNA;

namespace Mill5C.View.Window.Views.XNA
{
    public class WCSRenderer : XNARendererBase, IXnaDrawable
    {
        private VertexPositionColor[] axes;

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            axes = new VertexPositionColor[6];
            axes[0] = axes[2] = axes[4] = new VertexPositionColor(Vector3.Zero, Color.White);
            axes[1] = new VertexPositionColor(new Vector3(100, 0, 0), Color.Red);
            axes[3] = new VertexPositionColor(new Vector3(0, 100, 0), Color.Green);
            axes[5] = new VertexPositionColor(new Vector3(0, 0, 100), Color.Blue);
        }

        public override void Draw()
        {
            base.Draw();

            RenderingControl.GraphicsDevice.VertexDeclaration = colorVD;

            colorEffect.Begin();

            foreach (EffectPass pass in colorEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                RenderingControl.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, axes, 0, axes.Length / 2);
                
                pass.End();

            }

            colorEffect.End();
        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            
        }

        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            
        }
    }
}
