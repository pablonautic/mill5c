using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.DataStructures
{
    /// <summary>
    /// Octree data structure.
    /// </summary>
    [Serializable]
    public class Octree
    {
        /// <summary>
        /// Occurs when a node has been split into smaller parts.
        /// </summary>
        public event EventHandler NodeSplit;

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <value>The root.</value>
        public Node Root { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Octree"/> class.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="l">The l = a / 2</param>
        public Octree(Point3D center, float l)
        {
            Root = new Node(this, center, l);
            Root.SetR();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Root = new Node(this, Root.Center, Root.L);
            Root.SetR();
            GC.Collect();
        }

        /// <summary>
        /// Raises the NodeSplit event.
        /// </summary>
        /// <param name="node">The node.</param>
        internal void NodeSplitRaise(Node node)
        {
            if (NodeSplit != null)
                NodeSplit(node, EventArgs.Empty);
        }
    }
}
