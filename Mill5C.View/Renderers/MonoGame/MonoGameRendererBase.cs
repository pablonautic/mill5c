using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.View.Window.Views.MonoGame; 
using Mill5C.View.Window.Views.XNA; // For IXnaDrawable (assuming it remains in Views.XNA for now)
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mill5C.Core.Algorithm;

namespace Mill5C.View.Window.Renderers.MonoGame // Changed namespace
{
    // Renamed class from XNARendererBase to MonoGameRendererBase
    public abstract class MonoGameRendererBase : IRenderer, IXnaDrawable 
    {
        protected Engine Engine;

        public Mill5CGameViewModel GameViewModel { get; private set; }
    
        protected VertexDeclaration colorVD, textureVD; // These are XNA 3.1 types, will need update if problematic in MG

        protected BasicEffect colorEffect, textureEffect;

        protected Texture2D glassTexture;

        protected SpriteFont font;

        public XNARendererBase()
        {
            Visible = true;
        }

        public virtual void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            this.Engine = engine;

            GameViewModel = (Mill5CGameViewModel)scene; // Cast to the new ViewModel type
            GameViewModel.Drawables.Add(this); // Add self to ViewModel's drawable list

            // Access GraphicsDevice and Content via GameViewModel
            textureVD = new VertexDeclaration(GameViewModel.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            colorVD = new VertexDeclaration(GameViewModel.GraphicsDevice, VertexPositionColor.VertexElements);

            glassTexture = GameViewModel.Content.Load<Texture2D>("Glass"); // Use GameViewModel.Content
            font = GameViewModel.Content.Load<SpriteFont>("Font"); // Use GameViewModel.Content

            textureEffect = new BasicEffect(GameViewModel.GraphicsDevice, null);

            textureEffect.TextureEnabled = true;
            textureEffect.Texture = glassTexture;

            textureEffect.DirectionalLight0.Enabled = true;
            textureEffect.DirectionalLight0.DiffuseColor = Vector3.One;
            textureEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            textureEffect.DirectionalLight0.SpecularColor = Vector3.One;

            //textureEffect.DirectionalLight1.Enabled = true;
            //textureEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            //textureEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            //textureEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f); 
            
            colorEffect = new BasicEffect(GameViewModel.GraphicsDevice, null); // Use GameViewModel.GraphicsDevice
            colorEffect.VertexColorEnabled = true;
            colorEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public abstract void AttachEvents(Mill5C.Core.Algorithm.Engine engine);

        public abstract void DetachEvents(Mill5C.Core.Algorithm.Engine engine);

        public virtual bool Visible
        {
            get;
            set;
        }

        public virtual void Draw()
        {
            // Use GameViewModel for matrices
            colorEffect.World = GameViewModel.World;
            colorEffect.View = GameViewModel.View;
            colorEffect.Projection = GameViewModel.Projection;

            textureEffect.World = GameViewModel.World;
            textureEffect.View = GameViewModel.View;
            textureEffect.Projection = GameViewModel.Projection;

            // RenderState is different in MonoGame/XNA 4.0+
            // GraphicsDevice.RenderState.DepthBufferEnable = true; // Old XNA
            GameViewModel.GraphicsDevice.DepthStencilState = DepthStencilState.Default; // MonoGame equivalent

        }

        protected void DrawModel(Model model, Vector3 diffuse, Matrix localWorld)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.Alpha = 0.6f; // Alpha is part of Material in MonoGame BasicEffect

                    effect.World = 
                        boneTransforms[mesh.ParentBone.Index] * localWorld *
                        GameViewModel.World; // Use GameViewModel

                    effect.View = GameViewModel.View; // Use GameViewModel
                    effect.Projection = GameViewModel.Projection; // Use GameViewModel

                    effect.DiffuseColor = diffuse;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true; // This is default in MonoGame BasicEffect
                }

                mesh.Draw();
            }
        }

    }
}
