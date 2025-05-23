using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.DX;
using Mill5C.View.Window.Renderers;
using Mill5C.View.Window.Views.XNA; // For ArcBallCamera, WCSRenderer, IXnaDrawable
using System.Collections.Generic;
// System.ComponentModel is not strictly needed for this ViewModel's core logic

namespace Mill5C.View.Window.Views.MonoGame
{
    public class Mill5CGameViewModel : MonoGameViewModel, IDrawingView
    {
        // Core fields from old RenderingControl
        public List<IXnaDrawable> Drawables { get; private set; }
        public Matrix View { get; private set; }
        public Matrix World { get; private set; }
        public Matrix Projection { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        private WCSRenderer wcs;
        private ArcBallCamera arcBallCamera;

        // IDrawingView implementation (properties to hold specific renderers)
        public IRenderer MaterialRenderer { get; set; }
        public IRenderer PathRenderer { get; set; }
        public IRenderer CutterRenderer { get; set; }

        public IList<IRenderer> AllRenderers
        {
            get
            {
                var list = new List<IRenderer>();
                if (CutterRenderer != null) list.Add(CutterRenderer);
                if (MaterialRenderer != null) list.Add(MaterialRenderer);
                if (PathRenderer != null) list.Add(PathRenderer);
                return list;
            }
        }

        public object Scene
        {
            // The old RenderingControl returned 'this' (which was the control itself).
            // The 'scene' parameter in XNARendererBase.Initialize was cast to RenderingControl.
            // So, XNARendererBase and its children expect an object that can be cast to RenderingControl
            // to access its properties like GraphicsDevice, ContentManager, View, World, Projection, Drawables.
            // Mill5CGameViewModel now holds these.
            // Crucially, XNARendererBase adds itself to RenderingControl.Drawables.
            // And uses RenderingControl.GraphicsDevice, RenderingControl.ContentManager etc.
            // So, for XNARendererBase to work with minimal changes, it needs an object that *looks like*
            // the old RenderingControl. Mill5CGameViewModel is being designed to be that object.
            get { return this; } 
        }

        public void CleanUp()
        {
            Drawables?.Clear();
            // Dispose renderers if they are IDisposable
            (MaterialRenderer as System.IDisposable)?.Dispose();
            (PathRenderer as System.IDisposable)?.Dispose();
            (CutterRenderer as System.IDisposable)?.Dispose();
            MaterialRenderer = null;
            PathRenderer = null;
            CutterRenderer = null;
        }

        public void Activate()
        {
            // The MonoGamePanel and its ViewModel have their own update loop.
            // This method might be redundant or used for specific game state logic if needed.
        }

        public void Passivate()
        {
            // Similar to Activate, this might be redundant.
        }

        // MonoGameViewModel methods
        public override void Initialize()
        {
            base.Initialize(); // Always good practice

            Drawables = new List<IXnaDrawable>();
            arcBallCamera = new ArcBallCamera(); // Assumes default constructor is fine

            World = Matrix.Identity;
            // View will be set by ArcBallCamera in Update
            // Projection needs GraphicsDevice, set it after GraphicsDevice is initialized,
            // e.g., at the end of Initialize or beginning of LoadContent/Update for first time.
            // For now, deferring to where GraphicsDevice is known to be valid or using a temp value.
            // However, MonoGameViewModel.GraphicsDevice should be available after base.Initialize().
            UpdateProjectionMatrix();


            wcs = new WCSRenderer();
            // Defer wcs.Initialize to LoadContent, as it might load content or need GraphicsDevice.
        }
        
        private void UpdateProjectionMatrix()
        {
            if (GraphicsDevice != null)
            {
                float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
            }
            else
            {
                // Fallback or default if GraphicsDevice not ready (should be rare after Initialize)
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4f / 3f, 1, 10000);
            }
        }


        public override void LoadContent()
        {
            base.LoadContent(); // For any base class content loading

            SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            
            // Initialize WCSRenderer here as GraphicsDevice and Content are ready.
            // The old XNARendererBase.Initialize took (Engine engine, object scene).
            // WCSRenderer (an XNARendererBase) was called with (null, this) from RenderingControl.old.cs.
            // 'this' (Mill5CGameViewModel) is now the 'scene' object.
            // The XNARendererBase expects 'scene' to be castable to something providing GraphicsDevice, ContentManager, etc.
            // This ViewModel now serves that role.
            wcs.Initialize(null, this); // 'this' is the IDrawingView and provides access to Content, GraphicsDevice via itself.
                                        // This also adds 'wcs' to this.Drawables.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            arcBallCamera.Update(); // Update camera state (handles mouse input)
            View = arcBallCamera.View; // Get the updated view matrix

            // Dynamically update projection matrix if viewport changes (e.g. window resize)
            // This might be overkill if not explicitly needed or handled by MonoGame.Forms correctly.
            // For now, assume initial projection is fine or updated elsewhere if needed.
            // UpdateProjectionMatrix(); 

            // Update specific renderers if they have state to update
            (MaterialRenderer as IRenderer)?.Update(gameTime); // Using IRenderer.Update if it exists
            (PathRenderer as IRenderer)?.Update(gameTime);
            (CutterRenderer as IRenderer)?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime); // For any base class drawing

            GraphicsDevice.Clear(Color.White);

            // WCSRenderer is an IXnaDrawable and is in the Drawables list.
            // XNARendererBase.Draw (called by WCSRenderer.Draw) sets up effects with World, View, Projection
            // from the 'RenderingControl' (now 'this' ViewModel).

            foreach (var item in Drawables)
            {
                if (item is IRenderer renderer && !renderer.Visible) // Check visibility if item is also an IRenderer
                    continue;
                item.Draw();
            }
        }
    }
}
