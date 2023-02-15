using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Media.Media3D
{
    public static class Vector3DExtensions
    {
        public static System.Windows.Media.Media3D.Vector3D ToVector3D(this Mill5C.Core.Geometry.Vector3D vec)
        {
            return new System.Windows.Media.Media3D.Vector3D(vec.X, vec.Y, vec.Z);
        }
    }
}
