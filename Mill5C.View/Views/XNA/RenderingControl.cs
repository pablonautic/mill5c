using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mill5C.View.Window.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;

namespace Mill5C.View.Window.Views.XNA
{
    public class RenderingControl : WinFormsGraphicsDevice.GraphicsDeviceControl, IDrawingView
    {
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public List<IXnaDrawable> Drawables { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public Matrix View { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public Matrix World { get;  private set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public Matrix Projection { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public ContentManager ContentManager { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public SpriteBatch SpriteBatch { get; private set; }

        private WCSRenderer wcs;
        private ArcBallCamera arcBallCamera;

        private System.Timers.Timer updateTimer;

        protected override void Initialize()
        {
            ContentManager = new ContentManager(Services, "Content");
            Drawables = new List<IXnaDrawable>();

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            arcBallCamera = new ArcBallCamera();

            World = Matrix.Identity;
            View = arcBallCamera.View;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4f/3f, 1, 10000);

            wcs = new WCSRenderer();
            wcs.Initialize(null, this);

            updateTimer = new System.Timers.Timer(30);
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            updateTimer.Start();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            arcBallCamera.Update();
            View = arcBallCamera.View;
            Invalidate();           
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.White);

            wcs.Draw();

            foreach (var item in Drawables)
                item.Draw();
          
        }

        #region IDrawingView Members

        public IRenderer MaterialRenderer
        {
            get;
            set;
        }

        public IRenderer PathRenderer
        {
            get;
            set;
        }

        public IRenderer CutterRenderer
        {
            get;
            set;
        }

        public IList<IRenderer> AllRenderers
        {
            get
            {
                var list = new List<IRenderer>();                
                if (CutterRenderer != null)
                    list.Add(CutterRenderer);
                if (MaterialRenderer != null)
                    list.Add(MaterialRenderer);
                if (PathRenderer != null)
                    list.Add(PathRenderer);
                return list;
            }
        }

        public object Scene
        {
            get { return this; }
        }

        public void CleanUp()
        {
            Drawables.Clear();
        }

        public void Activate()
        {
            updateTimer.Start();
        }

        public void Passivate()
        {
            updateTimer.Stop();
        }

        #endregion
    }
}
