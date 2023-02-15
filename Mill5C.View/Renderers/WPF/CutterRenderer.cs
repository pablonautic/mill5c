using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Primitive3DSurfaces;
using Mill5C.Core.Algorithm;
using Mill5C.Core.Utility;
using System.Windows.Media;
using System.Windows.Threading;
using Mill5C.Core.Interpolators;

namespace Mill5C.View.Window.Renderers.WPF
{
    public class CutterRenderer : WPFRendererBase
    {
        private ModelVisual3D model;

        private AxisAngleRotation3D orientation;

        private TranslateTransform3D position;

        private ScaleTransform3D scale, scale2;

        private TranslateTransform3D correction;

        public CutterRenderer()
        {
        }

        public override void Initialize(Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            model = new ModelVisual3D();
            Scene.Children.Add(model);

            var cylinder = new Cylinder3D();
            model.Children.Add(cylinder);

            cylinder.Material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));

            var transf = new Transform3DGroup();
            cylinder.Transform = transf;

            float h2 = 0.5f * engine.Strategy.ReferenceCutter.H;
            float r = engine.Strategy.ReferenceCutter.R;

            scale = new ScaleTransform3D(
                r,
                h2,
                r);

            transf.Children.Add(scale);

            correction = new TranslateTransform3D(0, h2, 0);

            transf.Children.Add(correction);

            transf.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));

            orientation = new AxisAngleRotation3D(); //tu by moglo byc ustawione
            transf.Children.Add(new RotateTransform3D(orientation));

            var initialPos = engine.Strategy.ReferenceCutter.Position;

            position = new TranslateTransform3D(initialPos.X,initialPos.Y,initialPos.Z);
            transf.Children.Add(position);

            var sphere = new Sphere3D();
            model.Children.Add(sphere);

            sphere.Material = cylinder.Material;

            var transf2 = new Transform3DGroup();
            sphere.Transform = transf2;

            scale2 = new ScaleTransform3D(
                r,
                r,
                r);

            transf2.Children.Add(scale2);
            transf2.Children.Add(position);
        }

        public override void AttachEvents(Engine engine)
        {
            Engine.PathProcessingStarted += engine_PathLoaded;
        }

        public override void DetachEvents(Engine engine)
        {
            Engine.PathProcessingStarted -= engine_PathLoaded;
        }

        private void engine_PathLoaded(object sender, PathFileEventArgs args)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                float h2 = 0.5f * Engine.Strategy.ReferenceCutter.H;
                float r = Engine.Strategy.ReferenceCutter.R;

                scale.ScaleX = scale.ScaleZ = r;
                scale.ScaleY = h2;

                correction.OffsetY = h2;

                if (Engine.Strategy.ReferenceCutter is Mill5C.Core.Cutters.BallCutter)
                    scale2.ScaleX = scale2.ScaleY = scale2.ScaleZ = r;
                else
                    scale2.ScaleX = scale2.ScaleY = scale2.ScaleZ = 0;
            }));

            Engine.Strategy.ReferenceCutter.ConfigurationChanged += new EventHandler(ReferenceCutter_ConfigurationChanged);
        }

        private void ReferenceCutter_ConfigurationChanged(object sender, EventArgs e)
        {
            Mill5C.Core.Cutters.ICutter cutt = Engine.Strategy.ReferenceCutter;

            Dispatcher.Invoke(new Action(delegate
            {
                position.OffsetX = cutt.Position.X;
                position.OffsetY = cutt.Position.Y;
                position.OffsetZ = cutt.Position.Z;

                orientation.Angle = MathHelper.ToDegrees((float)Math.Acos(
                     Mill5C.Core.Geometry.Vector3D.Dot(
                         Mill5C.Core.Geometry.Vector3D.Up,
                         cutt.Orientation)));

                orientation.Axis = Mill5C.Core.Geometry.Vector3D.Cross(
                    Mill5C.Core.Geometry.Vector3D.Up,
                    cutt.Orientation).ToVector3D();
            }));
        }

        public override bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
