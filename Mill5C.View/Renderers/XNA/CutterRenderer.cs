using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mill5C.Core.Strategies.Octree;
using Mill5C.Core.Cutters;

namespace Mill5C.View.Window.Renderers.XNA
{
    public class CutterRenderer : XNARendererBase
    {
        private Model cylinder, sphere;

        private Matrix scaleCylinder, correctionCylinder, scaleSphere;

        private Matrix[] transforms;
        private Vector3[] colors;

        private BasicMultiThreadedStrategy mtStrategy;

        private bool drawSphere;

        private const int refreshRate = 10;
        private int refreshCounter;

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            cylinder = RenderingControl.ContentManager.Load<Model>("Cylinder");
            sphere = RenderingControl.ContentManager.Load<Model>("SphereHighPoly");

            Update();

            mtStrategy = engine.Strategy as BasicMultiThreadedStrategy;

            if (mtStrategy != null)
            {
                transforms = new Matrix[mtStrategy.Tasks.Count];
                colors = new Vector3[mtStrategy.Tasks.Count];
            }
            else
            {
                transforms = new Matrix[1];
                colors = new Vector3[1];
            }

            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i] = Matrix.Identity;
                colors[i] = new Vector3(0, 0, 0.5f);
            }

            SyncCutter(engine.Strategy.ReferenceCutter);
        }

        public override void AttachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathProcessingStarted += engine_PathPrepared;
            engine.PathCompleted += engine_PathCompleted;
        }

        public override void DetachEvents(Mill5C.Core.Algorithm.Engine engine)
        {
            engine.PathProcessingStarted -= engine_PathPrepared;
            engine.PathCompleted -= engine_PathCompleted;
        }

        private void engine_PathPrepared(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            Update();

            if (mtStrategy != null)
            {
                foreach (var task in mtStrategy.Tasks)
                    task.Cutter.ConfigurationChanged += Cutter_ConfigurationChanged;
            }
            else
            {
                Engine.Strategy.ReferenceCutter.ConfigurationChanged += Cutter_ConfigurationChanged;
            }
        }

        private void engine_PathCompleted(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            if (mtStrategy != null)
            {
                foreach (var task in mtStrategy.Tasks)
                    task.Cutter.ConfigurationChanged -= Cutter_ConfigurationChanged;
            }
            else
            {
                Engine.Strategy.ReferenceCutter.ConfigurationChanged -= Cutter_ConfigurationChanged;
            }
        }

        private void Cutter_ConfigurationChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            SyncCutter((ICutter)sender);
        }

        private void SyncCutter(ICutter cutter)
        {
            var angle = (float)Math.Acos(
             Mill5C.Core.Geometry.Vector3D.Dot(
                 Mill5C.Core.Geometry.Vector3D.Up,
                 cutter.Orientation));

            var axis = Mill5C.Core.Geometry.Vector3D.Cross(
                Mill5C.Core.Geometry.Vector3D.Up,
                cutter.Orientation);

            transforms[cutter.Id] = Matrix.CreateFromAxisAngle(new Vector3(axis.X, axis.Y, axis.Z), angle) *
                Matrix.CreateTranslation(cutter.Position.X, cutter.Position.Y, cutter.Position.Z);

            refreshCounter++;
            if (refreshCounter > refreshRate)
            {
                refreshCounter = 0;
                RenderingControl.Invoke(new Action(RenderingControl.Invalidate));
            }
        }

        private void Update()
        {
            float h2 = 0.25f * Engine.Strategy.ReferenceCutter.H;
            float r = 0.5f * Engine.Strategy.ReferenceCutter.R;

            scaleCylinder = Matrix.CreateScale(r, r, h2);

            scaleSphere = Matrix.CreateScale(r);

            correctionCylinder = Matrix.CreateTranslation(0, 0, 0.65f * Engine.Strategy.ReferenceCutter.H);

            drawSphere = Engine.Strategy.ReferenceCutter is BallCutter;
        }

        public override void Draw()
        {
            if (!Visible || transforms == null)
                return;

            base.Draw();

            for (int i = 0; i < transforms.Length; i++)
            {
                DrawModel(cylinder, colors[i], scaleCylinder * correctionCylinder * transforms[i]);
                if (drawSphere) DrawModel(sphere, colors[i], scaleSphere * transforms[i]);
            }
         
        }
    }
}
