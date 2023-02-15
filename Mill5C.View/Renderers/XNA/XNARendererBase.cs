using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.View.Window.Views.XNA;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mill5C.Core.Algorithm;

namespace Mill5C.View.Window.Renderers.XNA
{
    public abstract class XNARendererBase : IRenderer, IXnaDrawable
    {
        protected Engine Engine;

        public RenderingControl RenderingControl { get; private set; }
    
        protected VertexDeclaration colorVD, textureVD;

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

            RenderingControl = (RenderingControl)scene;
            RenderingControl.Drawables.Add(this);

            textureVD = new VertexDeclaration(RenderingControl.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            colorVD = new VertexDeclaration(RenderingControl.GraphicsDevice, VertexPositionColor.VertexElements);

            glassTexture = RenderingControl.ContentManager.Load<Texture2D>("Glass");

            font = RenderingControl.ContentManager.Load<SpriteFont>("Font");

            textureEffect = new BasicEffect(RenderingControl.GraphicsDevice, null);

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
            
            colorEffect = new BasicEffect(RenderingControl.GraphicsDevice, null);
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
            colorEffect.World = RenderingControl.World;
            colorEffect.View = RenderingControl.View;
            colorEffect.Projection = RenderingControl.Projection;

            textureEffect.World = RenderingControl.World;
            textureEffect.View = RenderingControl.View;
            textureEffect.Projection = RenderingControl.Projection;

            RenderingControl.GraphicsDevice.RenderState.DepthBufferEnable = true;
        }

        protected void DrawModel(Model model, Vector3 diffuse, Matrix localWorld)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.Alpha = 0.6f;

                    effect.World = 
                        boneTransforms[mesh.ParentBone.Index] * localWorld *
                        RenderingControl.World;

                    effect.View = RenderingControl.View;
                    effect.Projection = RenderingControl.Projection;

                    effect.DiffuseColor = diffuse;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }

                mesh.Draw();
            }
        }

    }
}
