using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; // Added for Color
using Microsoft.Xna.Framework.Graphics;
using Mill5C.Core.Path;

namespace Mill5C.View.Window.Renderers.MonoGame // Changed namespace
{
    public class PathRenderer : MonoGameRendererBase // Inherit from MonoGameRendererBase
    {
        private VertexPositionColor[] vertexData;

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);
            if (engine.CurrentPath != null) // Ensure CurrentPath is not null
            {
                DrawPath(engine.CurrentPath);
            }
            else
            {
                vertexData = new VertexPositionColor[0]; // Initialize to empty if no path
            }
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
            if (path == null || path.Count == 0)
            {
                vertexData = new VertexPositionColor[0];
                return;
            }

            vertexData = new VertexPositionColor[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                vertexData[i] = new VertexPositionColor(
                    new Microsoft.Xna.Framework.Vector3(
                        path[i].Position.X,
                        path[i].Position.Y,
                        path[i].Position.Z), Color.BurlyWood); // Microsoft.Xna.Framework.Color
            }
        }

        public override void Draw()
        {
            if (!Visible || vertexData == null || vertexData.Length <= 1) // Added null check for vertexData
                return;

            base.Draw(); // Sets up effect matrices

            // Set DepthStencilState (DepthBufferEnable = false equivalent)
            GameViewModel.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            
            // Set BlendState for alpha blending if needed (colorEffect.Alpha is used)
            // GameViewModel.GraphicsDevice.BlendState = BlendState.AlphaBlend; 
            // BasicEffect's Alpha property should work with the standard BlendState.AlphaBlend or NonPremultiplied.
            // Default BlendState is Opaque, so if alpha is less than 1, it might not show as expected without this.
            // For now, let's assume an explicit BlendState is good if alpha is used.
            GameViewModel.GraphicsDevice.BlendState = BlendState.AlphaBlend;


            // VertexDeclaration is not typically set directly in MonoGame when using BasicEffect.
            // GameViewModel.GraphicsDevice.VertexDeclaration = colorVD; // Obsolete

            colorEffect.Alpha = 0.7f; // Set alpha on the effect

            colorEffect.CurrentTechnique.Passes[0].Apply(); // Apply the effect pass

            GameViewModel.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.LineStrip, 
                vertexData, 
                0, 
                vertexData.Length - 1);
            
            // Reset states if they were changed and might affect other renderers
            GameViewModel.GraphicsDevice.DepthStencilState = DepthStencilState.Default; // Reset to default
            GameViewModel.GraphicsDevice.BlendState = BlendState.Opaque; // Reset to default
        }
    }
}
