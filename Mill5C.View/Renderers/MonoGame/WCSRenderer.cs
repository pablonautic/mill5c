using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
// Using statement for Views.XNA might still be needed if IXnaDrawable is there.
// Or if IXnaDrawable is moved to a common namespace.
using Mill5C.View.Window.Views.XNA; 

// Namespace is changed to match its new location and purpose
namespace Mill5C.View.Window.Renderers.MonoGame 
{
    // Inherits from MonoGameRendererBase now
    public class WCSRenderer : MonoGameRendererBase, IXnaDrawable 
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
            base.Draw(); // This sets up the effects (World, View, Projection)

            // VertexDeclaration is not typically set directly in MonoGame when using BasicEffect.
            // BasicEffect handles this if VertexColorEnabled is true.
            // GameViewModel.GraphicsDevice.VertexDeclaration = colorVD; // Obsolete for BasicEffect

            colorEffect.CurrentTechnique.Passes[0].Apply(); // Apply the effect pass

            // Use GameViewModel.GraphicsDevice
            GameViewModel.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.LineList, 
                axes, 
                0, 
                axes.Length / 2);
        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            
        }

        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            
        }
    }
}
