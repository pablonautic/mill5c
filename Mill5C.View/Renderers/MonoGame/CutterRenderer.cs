using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mill5C.Core.Strategies.Octree;
using Mill5C.Core.Cutters;

namespace Mill5C.View.Window.Renderers.MonoGame // Changed namespace
{
    public class CutterRenderer : MonoGameRendererBase // Inherit from MonoGameRendererBase
    {
        private Model cylinder, sphere;

        private Matrix scaleCylinder, correctionCylinder, scaleSphere;

        private Matrix[] transforms;
        private Vector3[] colors;

        private BasicMultiThreadedStrategy mtStrategy;

        private bool drawSphere;

        // private const int refreshRate = 10; // Commented out, Invalidate logic removed
        // private int refreshCounter;

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            // Use GameViewModel.Content for loading models
            cylinder = GameViewModel.Content.Load<Model>("Cylinder");
            sphere = GameViewModel.Content.Load<Model>("SphereHighPoly");

            Update(); // This uses Engine.Strategy.ReferenceCutter, ensure Engine is set by base.Initialize

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
                colors[i] = new Vector3(0, 0, 0.5f); // Default color
            }
            
            if (engine.Strategy?.ReferenceCutter != null) // Check for null
            {
                SyncCutter(engine.Strategy.ReferenceCutter);
            }
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
                if (Engine?.Strategy?.ReferenceCutter != null) // Null checks
                {
                    Engine.Strategy.ReferenceCutter.ConfigurationChanged += Cutter_ConfigurationChanged;
                }
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
                if (Engine?.Strategy?.ReferenceCutter != null) // Null checks
                {
                    Engine.Strategy.ReferenceCutter.ConfigurationChanged -= Cutter_ConfigurationChanged;
                }
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
            if (cutter == null || cutter.Id < 0 || cutter.Id >= transforms.Length) return; // Bounds check

            var angle = (float)Math.Acos(
             Mill5C.Core.Geometry.Vector3D.Dot(
                 Mill5C.Core.Geometry.Vector3D.Up,
                 cutter.Orientation));

            var axis3D = Mill5C.Core.Geometry.Vector3D.Cross( // Use full namespace for Vector3D
                Mill5C.Core.Geometry.Vector3D.Up,
                cutter.Orientation);
            
            // Normalize axis in case it's very small or zero
            if (axis3D.GetLengthSquared() < float.Epsilon) {
                 axis3D = Mill5C.Core.Geometry.Vector3D.Up; // Default axis or handle as no rotation
                 angle = 0; // No rotation if axis is zero
            } else {
                axis3D.Normalize();
            }


            transforms[cutter.Id] = Matrix.CreateFromAxisAngle(new Vector3(axis3D.X, axis3D.Y, axis3D.Z), angle) *
                Matrix.CreateTranslation(cutter.Position.X, cutter.Position.Y, cutter.Position.Z);

            // refreshCounter++; // Invalidate logic removed
            // if (refreshCounter > refreshRate)
            // {
            //    refreshCounter = 0;
            //    GameViewModel.HostControl.Invalidate(); // This was RenderingControl.Invoke(new Action(RenderingControl.Invalidate));
            // }                                          // MonoGame.Forms updates via its game loop.
        }

        private void Update()
        {
            if (Engine?.Strategy?.ReferenceCutter == null) return; // Null check

            float h2 = 0.25f * Engine.Strategy.ReferenceCutter.H;
            float r = 0.5f * Engine.Strategy.ReferenceCutter.R;

            scaleCylinder = Matrix.CreateScale(r, r, h2);
            scaleSphere = Matrix.CreateScale(r);
            correctionCylinder = Matrix.CreateTranslation(0, 0, 0.65f * Engine.Strategy.ReferenceCutter.H);
            drawSphere = Engine.Strategy.ReferenceCutter is BallCutter;
        }

        public override void Draw()
        {
            if (!Visible || transforms == null || cylinder == null || (drawSphere && sphere == null)) // Added model null checks
                return;

            base.Draw(); // Sets up effect matrices in MonoGameRendererBase

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i] == default(Matrix)) continue; // Skip if transform not set

                DrawModel(cylinder, colors[i], scaleCylinder * correctionCylinder * transforms[i]);
                if (drawSphere) DrawModel(sphere, colors[i], scaleSphere * transforms[i]);
            }
        }
    }
}
