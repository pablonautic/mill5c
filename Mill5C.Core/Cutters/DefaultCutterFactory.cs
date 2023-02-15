using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mill5C.Core.Utility;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.Cutters
{
    /// <summary>
    /// This class is responsible for creating cutter instances based on patterns.
    /// Basic implementation works for FlatCutter ('F') and BallCutter ('B').
    /// Any additional types can be registered using the RegisterCutterType method.
    /// </summary>
    public class DefaultCutterFactory : Mill5C.Core.Cutters.ICutterFactory
    {
        /// <summary>
        /// Dictonary mapping cutter strings (eg. "B") to types.
        /// </summary>
        protected Dictionary<string, Type> cutters = new Dictionary<string, Type>();

        /// <summary>
        /// Gets or sets the height of created cutters.
        /// </summary>
        /// <value>The H.</value>
        public float H { get; set; }

        /// <summary>
        /// Gets or sets the initial position. Defaults to (0,0,150).
        /// </summary>
        /// <value>The initial position.</value>
        public Point3D InitialPosition { get; set; }

        /// <summary>
        /// Gets or sets the initial orientation. Default to (0,0,1).
        /// </summary>
        /// <value>The initial orientation.</value>
        public Vector3D InitialOrientation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCutterFactory"/> class.
        /// </summary>
        public DefaultCutterFactory()
        {
            RegisterCutterType("F", typeof(FlatCutter));
            RegisterCutterType("K", typeof(BallCutter));

            H = 100;
            InitialPosition = new Point3D(0, 0, 150);
            InitialOrientation = Vector3D.Up;
        }

        /// <summary>
        /// The main method of this class. Searches the filename for a LetterNumber pattern,
        /// and creates a cutter of this type. Method is case-insensitive.
        /// Throws exception if matching failes. Example patterns: K12, f6.
        /// </summary>
        /// <param name="filename">The filename to search</param>
        /// <returns></returns>
        public virtual ICutter CreateCutter(string filename)
        {
            String intPattern = "[0-9]+";

            foreach (var symbol in cutters.Keys)
            {
                Match m = new Regex(symbol + intPattern).Match(filename.ToUpper());
                if (m.Success)
                {
                    int d;
                    if (int.TryParse(filename.Substring(m.Index + 1, m.Length - 1), out d))
                    {
                        ICutter cutt = Activator.CreateInstance(cutters[symbol]) as ICutter;
                        cutt.H = H;
                        cutt.R = d / 2;
                        cutt.Position = InitialPosition;
                        cutt.Orientation = InitialOrientation;
                        return cutt;
                    }
                }
            }

            throw new Mill5CException("could not determine cutter type, filename: "
                + filename);
        }

        /// <summary>
        /// Registers the type of the cutter and the symbol used by it, for example 'F'.
        /// </summary>
        /// <param name="symbol">The symbol (usually one letter)</param>
        /// <param name="type">The type (must subclass CutterBase)</param>
        public void RegisterCutterType(string symbol, Type type)
        {
            if (!type.IsSubclassOf(typeof(ICutter)))
                throw new Mill5CException("registered cutter type " + type.ToString() +
                    " must be a subclass of CutterBase");

            cutters.Add(symbol, type);
        }
    }
}
