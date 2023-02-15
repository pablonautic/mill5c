using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;
using Mill5C.Core.Utility;

namespace Mill5C.Core.Cutters
{
    /// <summary>
    /// Base class for creating cutters.
    /// </summary>
    public abstract class ICutter
    {
        /// <summary>
        /// Occurs when either the position or orientation of the cutter has changed.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        private Point3D position;

        /// <summary>
        /// Gets the position of the cutter.
        /// </summary>
        /// <value>The position.</value>
        public Point3D Position
        {
            get { return position; }
            internal set 
            { 
                position = value;
                ConfigurationChangedRaise();
            }
        }

        private Vector3D orientation;

        /// <summary>
        /// Gets the orientation of the cutter.
        /// </summary>
        /// <value>The orientation.</value>
        public Vector3D Orientation
        {
            get { return orientation; }
            internal set 
            { 
                orientation = value;
                orientation.Normalize();
                ConfigurationChangedRaise();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ICutter"/> class.
        /// </summary>
        public ICutter()
        {
            Position = new Point3D();
            Orientation = Vector3D.Up;
        }

        /// <summary>
        /// Gets or sets the R.
        /// </summary>
        /// <value>The R.</value>
        public float R { get; set; }

        /// <summary>
        /// Gets or sets the H.
        /// </summary>
        /// <value>The H.</value>
        public float H { get; set; }

        /// <summary>
        /// Gets or sets the optional id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Intersects the specified sphere center.
        /// </summary>
        /// <param name="sphereCenter">The sphere center.</param>
        /// <param name="sphereR">The sphere R.</param>
        /// <returns></returns>
        public abstract CollisionType Intersect(Point3D sphereCenter, float sphereR);

        /// <summary>
        /// Raises the ConfigurationChanged event.
        /// </summary>
        protected void ConfigurationChangedRaise()
        {
            if (ConfigurationChanged != null)
                ConfigurationChanged(this, null);
        }

        /// <summary>
        /// Helper method for cloning instances of the cutter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T CloneHelper<T>() where T : ICutter, new()
        {
            return new T
            {
                Position = Position.Clone(),
                Orientation = Orientation.Clone(),
                H = H,
                R = R
            };
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public abstract ICutter Clone();

    }
}
