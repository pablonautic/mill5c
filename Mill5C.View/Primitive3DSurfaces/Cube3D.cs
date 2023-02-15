using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Primitive3DSurfaces
{
    public class Cube3D : Primitive3D
    {
        internal override System.Windows.Media.Media3D.Geometry3D Tessellate()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions = new Point3DCollection
            {         
                 new Point3D(-1,-1,-1), new Point3D(1,-1,-1), new Point3D(1,1,-1),
                 new Point3D(-1,-1,-1), new Point3D(1,1,-1), new Point3D(-1,1,-1),
                 new Point3D(1,-1,-1), new Point3D(1,-1,1), new Point3D(1,1,-1),
                 new Point3D(1,1,-1), new Point3D(1,-1,1), new Point3D(1,1,1),
                 new Point3D(-1,-1,-1), new Point3D(1,-1,-1), new Point3D(1,-1,1),
                 new Point3D(-1,-1,-1), new Point3D(-1,-1,1), new Point3D(1,-1,1),
                 new Point3D(-1,1,-1), new Point3D(1,1,-1), new Point3D(-1,1,1),
                 new Point3D(1,1,-1), new Point3D(-1,1,1), new Point3D(1,1,1),
                 new Point3D(-1,-1,-1), new Point3D(-1,-1,1), new Point3D(-1,1,-1),
                 new Point3D(-1,-1,1), new Point3D(-1,1,-1), new Point3D(-1,1,1),
                 new Point3D(-1,-1,1), new Point3D(1,-1,1), new Point3D(1,1,1),
                 new Point3D(-1,-1,1), new Point3D(1,1,1), new Point3D(-1,1,1),
            };

            mesh.TextureCoordinates = new System.Windows.Media.PointCollection 
            { 
                 new Point(1,1), new Point(0,1), new Point(0,0),
                 new Point(1,1), new Point(0,0), new Point(1,0),
                 new Point(1,1), new Point(0,1), new Point(1,0),
                 new Point(1,0), new Point(0,1), new Point(0,0),
                 new Point(1,1), new Point(0,1), new Point(0,0),
                 new Point(1,1), new Point(1,0), new Point(0,0),
                 new Point(1,1), new Point(0,1), new Point(1,0),
                 new Point(0,1), new Point(1,0), new Point(0,0),
                 new Point(0,1), new Point(1,1), new Point(0,0),
                 new Point(1,1), new Point(0,0), new Point(1,0),
                 new Point(0,1), new Point(1,1), new Point(1,0),
                 new Point(0,1), new Point(1,0), new Point(0,0)
            };

            mesh.TriangleIndices = new System.Windows.Media.Int32Collection 
            { 
                2, 1, 0, 5, 4, 3, 8, 7, 6, 11, 10, 9, 12, 13, 14, 17, 16, 15, 20, 
                19, 18, 21, 22, 23, 24, 25, 26, 29, 28, 27, 30, 31, 32, 33, 34, 35 
            };

            mesh.Freeze();
            return mesh;
        }
    }
}
