using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using Mill5C.Core.Materials;
using Primitive3DSurfaces;
using System.Windows.Media;
using Mill5C.Core.DataStructures;

namespace Mill5C.View.Window.Renderers.WPF
{
    public abstract class OctreeRendererBase : WPFRendererBase
    {
        protected Dictionary<Node, Primitive3D> data = new Dictionary<Node, Primitive3D>();

        protected ModelVisual3D model;

        protected OctreeMaterial material;

        protected bool cubes;

        protected SolidColorBrush brushWhite, brushBlack, brushGray;
        protected DiffuseMaterial materialWhite, materialBlack, materialGray;

        public OctreeRendererBase(bool drawCubes)
        {
            cubes = drawCubes;

            brushWhite = new SolidColorBrush(Colors.White);
            brushBlack = new SolidColorBrush(Colors.DarkGreen);
            brushGray = new SolidColorBrush(Colors.Gray);

            brushWhite.Opacity = brushGray.Opacity = brushBlack.Opacity = 0.75;

            materialWhite = new DiffuseMaterial(brushWhite);
            materialGray = new DiffuseMaterial(brushGray);
            materialBlack = new DiffuseMaterial(brushBlack);
        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);

            model = new ModelVisual3D();
            Scene.Children.Add(model);

            material = (OctreeMaterial)engine.Material;
        }

        protected void CreatePrimitive(Node node)
        {
            Primitive3D cube;

            if (cubes)
                cube = new Cube3D();
            else
                cube = new Sphere3D();

            model.Children.Add(cube);
            data.Add(node, cube);

            var transf = new Transform3DGroup();
            cube.Transform = transf;

            if (cubes)
                transf.Children.Add(new ScaleTransform3D(node.L, node.L, node.L));
            else
                transf.Children.Add(new ScaleTransform3D(node.R, node.R, node.R));

            transf.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));

            transf.Children.Add(new TranslateTransform3D(node.Center.X, node.Center.Y, node.Center.Z));

            ApplyColor(node, cube);

        }

        protected void ApplyColor(Node node, Primitive3D cube)
        {
            switch (node.Color)
            {
                case NodeColor.White:
                    cube.Material = materialWhite;
                    break;
                case NodeColor.Black:
                    cube.Material = materialBlack;
                    break;
                case NodeColor.Gray:
                    cube.Material = materialGray;
                    break;
                default:
                    throw new Mill5C.Core.Utility.Mill5CException("unreachable code");
            }

        }
    }
}
