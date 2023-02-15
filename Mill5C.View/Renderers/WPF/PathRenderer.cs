using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using _3DTools;
using Mill5C.Core.Path;
using Mill5C.Core.Algorithm;
using System.Windows.Threading;

namespace Mill5C.View.Window.Renderers.WPF
{
    public class PathRenderer: WPFRendererBase
    {
        #region IRenderer Members

        private ScreenSpaceLines3D lines;

        public override void Initialize(Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            lines = new ScreenSpaceLines3D();
            lines.Thickness = 1;
            lines.Color = System.Windows.Media.Colors.BurlyWood;
            Scene.Children.Add(lines);

            DrawPath(engine.CurrentPath);
        }

        public override void AttachEvents(Engine engine)
        {
            engine.PathPrepared += PathPreparedHandler;
        }

        public override void DetachEvents(Engine engine)
        {
            engine.PathPrepared -= PathPreparedHandler;
        }

        private void PathPreparedHandler(object sender, Mill5C.Core.Algorithm.PathFileEventArgs args)
        {
            DrawPath(args.Path);
        }

        private void DrawPath(Path path)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                lines.Points.Clear();
                lines.Points.Add(new Point3D(path[0].Position.X, path[0].Position.Y, path[0].Position.Z));
                for (int i = 1; i < path.Count-1; i++)
                {
                    lines.Points.Add(new Point3D(path[i].Position.X, path[i].Position.Y, path[i].Position.Z));
                    lines.Points.Add(new Point3D(path[i].Position.X, path[i].Position.Y, path[i].Position.Z));
                }
                lines.Points.Add(new Point3D(
                    path[path.Count - 1].Position.X, path[path.Count - 1].Position.Y, path[path.Count - 1].Position.Z));
            }));
        }

        private bool visible = true;

        public override bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (visible)
                {
                    //lines.Transform = new TranslateTransform3D(0, 0, 0);
                    Scene.Children.Add(lines);
                  
                }
                else
                {
                    //lines.Transform = new TranslateTransform3D(0, 0, 10000);
                    Scene.Children.Remove(lines);
                }
            }
        }

        #endregion
    }
}
