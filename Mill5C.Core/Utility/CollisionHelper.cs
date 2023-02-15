using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.Utility
{
    /// <summary>
    /// Helper class for calculation interactions between objects.
    /// </summary>
    public class CollisionHelper
    {

        /// <summary>
        /// Intersects a cylinder and a sphere.
        /// </summary>
        /// <param name="sphereCenter">The sphere center.</param>
        /// <param name="sphereR">The sphere R.</param>
        /// <param name="cylinderPosition">The cylinder position (bottom point)</param>
        /// <param name="cylinderOrientation">The cylinder orientation.</param>
        /// <param name="cylinderR">The cylinder R.</param>
        /// <param name="cylinderH">The cylinder H.</param>
        /// <returns></returns>
        public static CollisionType CylinderSphere(Point3D sphereCenter, float sphereR,
            Point3D cylinderPosition, Vector3D cylinderOrientation, float cylinderR, float cylinderH)
        {
            //wektor od spodu walca do środka kuli
            Vector3D cBsC = sphereCenter - cylinderPosition;

            //wspolrzedne punktu rzutu srodka sfery na os walca
            Point3D c = cylinderPosition + cylinderOrientation * Vector3D.Dot(cBsC, cylinderOrientation);

            //kwadrat odleglosci srodka sfery od jej rzutu
            float eSq = Point3D.DistSqr(c, sphereCenter);

            if (eSq > sphereR.Square() && eSq > (cylinderR + sphereR).Square())
            {
                //sfera lezy poza extrudem walca
                return CollisionType.None;
            }
            else
            {   
                //srodek walca
                Point3D o = cylinderPosition + cylinderOrientation * 0.5f * cylinderH;

                float ds = -(o.X * cylinderOrientation.X + o.Y * cylinderOrientation.Y + o.Z * cylinderOrientation.Z);

                //odleglosc srodka kuli od plaszczyny freza przechodzacej przez jego srodek
                float d = Math.Abs(cylinderOrientation.X * sphereCenter.X + cylinderOrientation.Y * sphereCenter.Y +
                    cylinderOrientation.Z * sphereCenter.Z + ds);

                if (d > 0.5f * cylinderH + sphereR)
                {
                    //kula lezy w calosci pod albo nad walcem
                    return CollisionType.None;
                }
                else
                {
                    if (eSq > cylinderR.Square() && d > 0.5f * cylinderH)
                    {
                        //srodek sfery lezy poza extrudem walca i pod nim albo nad nim
                        //sprawdzamy czy rog walca nalezy do niej

                        //Vector3D cBsC = sphereCenter - cylinderPosition;

                        //dwukrotnym crossem znajdujemy wektor prostopadly do orientacji freza
                        Vector3D orientationOrtogonal = cylinderOrientation.Cross(cBsC.Cross(cylinderOrientation));                       
                        orientationOrtogonal.Normalize();
                        //znajdujemy wspolrzedne punktu naroznego
                        Point3D corner = cylinderPosition + orientationOrtogonal * cylinderR;

                        if (Point3D.DistSqr(corner, sphereCenter) < sphereR.Square())
                            return CollisionType.Partial;
                        else
                            return CollisionType.None;

                    }
                    else
                    {
                        //srodek kuli lezy poza exrudem, ale miedzy brzegami walca
                        //to juz jest napewno kolizja

                        //sprawdzamy czy kula lezy wewnatrz walca
                        if ((float)Math.Sqrt(eSq) + sphereR < cylinderR && d + sphereR < 0.5f * cylinderH)
                            return CollisionType.Total;
                        else
                            return CollisionType.Partial;
                    }
                }
            }
        }

        /// <summary>
        /// Intersects two spheres. This operation is not commutative!!
        /// </summary>
        /// <param name="cutterCenter">The cutter center.</param>
        /// <param name="cutterR">The cutter R.</param>
        /// <param name="center2">The center2.</param>
        /// <param name="r2">The r2.</param>
        /// <returns></returns>
        public static CollisionType SphereSphere(Point3D cutterCenter, float cutterR, Point3D center2, float r2)
        {
            float dstSq = cutterCenter.DistSqr(center2);
            if (dstSq > (cutterR + r2).Square())
                return CollisionType.None;
            else
            {
                //collision for sure
                float dst = (float)Math.Sqrt(dstSq);
                if (dst + r2 < cutterR) // sphere inside the cutter
                    return CollisionType.Total;
                else
                    return CollisionType.Partial;
            }
        }
    }
}
