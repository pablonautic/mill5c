using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.Path
{
    /// <summary>
    /// Represents a single miller path
    /// </summary>
    public class Path : List<PathPoint>
    {
        /// <summary>
        /// Gets the source file.
        /// </summary>
        /// <value>The source file.</value>
        public string SourceFile { get; private set; }

        /// <summary>
        /// Saves the path to a specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            Save(fs);
            fs.Close();
        }

        /// <summary>
        /// Saves the path to a specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine("N0 " + this[0].ToString());
                for (int i = 1; i < Count; i++)
                {
                    sw.WriteLine(string.Format("N{0} {1}", i, 
                        this[i].ToString(
                            !this[i].Position.X.IsNear(this[i-1].Position.X),
                            !this[i].Position.Y.IsNear(this[i-1].Position.Y),
                            !this[i].Position.Z.IsNear(this[i-1].Position.Z),
                            !this[i].Orientation.X.IsNear(this[i-1].Orientation.X),
                            !this[i].Orientation.Y.IsNear(this[i-1].Orientation.Y),
                            !this[i].Orientation.Z.IsNear(this[i-1].Orientation.Z))));
                }
            }
        }

        /// <summary>
        /// Load a path from a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static Path FromFile(string file)
        {
            Path path = new Path { SourceFile = file };

            using (StreamReader sr = new StreamReader(file))
            {
                PathPoint prevPoint = PathPoint.Zero;

                //bool read = false;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    //if (line.Contains("G01")) // GOTO command
                    //    read = true;

                    //if (read)
                    {
                        PathPoint pp = PathPoint.FromLine(line, prevPoint);
                        path.Add(pp);
                        prevPoint = pp;
                    }
                }
            }

            if (path.Count == 0)
                throw new Mill5C.Core.Utility.Mill5CException("file " + path.SourceFile + " does not " +
                    "contain any path data");

            return path;
        }

        /// <summary>
        /// Return a mock path containing a single point [ new Point3D(0,0,100), Orientation = Vector3D.Up ].
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static Path Mock(string file)
        {
            Path path = new Path { SourceFile = file };
            path.Add(new PathPoint { Position = new Point3D(0,0,100), Orientation = Vector3D.Up });
            return path;
        }
    }
}
