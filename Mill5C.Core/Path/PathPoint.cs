using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.Path
{
    /// <summary>
    /// Represents a single 5C cutter position and orientation
    /// </summary>
    public class PathPoint
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point3D Position { get; set; }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Vector3D Orientation { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current point.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current point.
        /// </returns>
        public override string ToString()
        {
            return ToString(true, true, true, true, true, true);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current point.
        /// </summary>
        /// <param name="writeX">if set to <c>true</c> [write X].</param>
        /// <param name="writeY">if set to <c>true</c> [write Y].</param>
        /// <param name="writeZ">if set to <c>true</c> [write Z].</param>
        /// <param name="writeI">if set to <c>true</c> [write I].</param>
        /// <param name="writeJ">if set to <c>true</c> [write J].</param>
        /// <param name="writeK">if set to <c>true</c> [write K].</param>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current point.
        /// </returns>
        public string ToString(bool writeX, bool writeY, bool writeZ, bool writeI, bool writeJ, bool writeK)
        {
            StringBuilder sb = new StringBuilder();
            Append(sb, writeX, writeY, writeZ, writeI, writeJ, writeK);
            return sb.ToString();
        }

        private void Append(StringBuilder builder, bool writeX, bool writeY, bool writeZ, bool writeI, bool writeJ, bool writeK)
        {
            if (writeX)
                AppendCoordinate(builder, "X", Position.X);
            if (writeY)
                AppendCoordinate(builder, "Y", Position.Y);
            if (writeY)
                AppendCoordinate(builder, "Z", Position.Z);
            if (writeI)
                AppendCoordinate(builder, "I", Orientation.X);
            if (writeJ)
                AppendCoordinate(builder, "J", Orientation.Y);
            if (writeK)
                AppendCoordinate(builder, "K", Orientation.Z);
        }

        private void AppendCoordinate(StringBuilder builder, string label, float value)
        {
            builder.Append(label).Append(value.ToShortString()).Append(" ");
        }

        /// <summary>
        /// Regex pattern for double
        /// </summary>
        private const string FloatPattern = "-?[0-9]+.[0-9]+";

        /// <summary>
        /// Parses a line of text to extract path point information.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="predecessor">The predecessor.</param>
        /// <returns></returns>
        public static PathPoint FromLine(string text, PathPoint predecessor)
        {
            float x, y, z, i, j, k;

            if (!TryParseCoeffecient("X", text, out x))
                x = predecessor.Position.X;

            if (!TryParseCoeffecient("Y", text, out y))
                y = predecessor.Position.Y;


            if (!TryParseCoeffecient("Z", text, out z))
                z = predecessor.Position.Z;


            if (!TryParseCoeffecient("I", text, out i))
                i = predecessor.Orientation.X;


            if (!TryParseCoeffecient("J", text, out j))
                j = predecessor.Orientation.Y;


            if (!TryParseCoeffecient("K", text, out k))
                k = predecessor.Orientation.Z;


            Vector3D dir = new Vector3D(i, j ,k );
            dir.Normalize();

            //there's a big chance that the angle is contant,
            //so we check this to potentially spare some memory
            if (dir.X.IsNear(predecessor.Orientation.X) &&
                dir.Y.IsNear(predecessor.Orientation.Y) &&
                dir.Z.IsNear(predecessor.Orientation.Z))
            {
                dir = predecessor.Orientation;
            }

            return new PathPoint
            {
                Orientation = dir,
                Position = new Point3D
                {
                    X = x,
                    Y = y,
                    Z = z
                }
            };
        }

        private static bool TryParseCoeffecient(string name, string line, out float result)
        {
            result = 0;
            Match m = new Regex(name + FloatPattern).Match(line);
            if (m.Success && SingleExtension.TryParse(line.Substring(m.Index + 1, m.Length - 1), out result))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Return a "zero" point - Position = (0,0,0) and Orientation = (0,0,1).
        /// </summary>
        /// <value>The zero.</value>
        public static PathPoint Zero
        {
            get { return new PathPoint { Position = new Point3D(), Orientation = Vector3D.Up }; }
        }
    }
}
