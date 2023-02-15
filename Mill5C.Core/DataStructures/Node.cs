using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Geometry;

namespace Mill5C.Core.DataStructures
{
    /// <summary>
    /// A single node of an octree.
    /// </summary>
    [Serializable]
    public class Node
    {  
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public NodeColor Color { get; set; }

        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        /// <value>The center.</value>
        public Point3D Center { get; protected set; }

        /// <summary>
        /// Gets or sets the L = a / 2;
        /// </summary>
        /// <value>The L.</value>
        public float L { get; protected set; }

        /// <summary>
        /// Gets the radius of the surrounding sphere.
        /// </summary>
        /// <value>The R.</value>
        public float R { get; private set; }

        /// <summary>
        /// Gets the octree instance owning this node.
        /// </summary>
        /// <value>The owner.</value>
        public Octree Owner { get; private set; }

        /// <summary>
        /// Gets the parent node of this instance.
        /// </summary>
        /// <value>The parent.</value>
        public Node Parent { get; private set; }

        /// <summary>
        /// Gets the children nodes.
        /// </summary>
        /// <value>The children.</value>
        public Node[, ,] Children { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="center">The center.</param>
        /// <param name="l">The l = a/2</param>
        public Node(Octree owner, Point3D center, float l)
        {          
            Center = center;
            L = l;
            Owner = owner;
            Color = NodeColor.Black;
        }

        /// <summary>
        /// Calculates the R.
        /// </summary>
        internal void SetR()
        {
            R = L * Mill5C.Core.Utility.MathHelper.Sqrt3;
        }

        /// <summary>
        /// Splits this instance into 8 child nodes, handles reference setting.
        /// </summary>
        public void Split()
        {
            Children = new Node[2, 2, 2];
            float l = 0.5f * L;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Children[i, j, k] = new Node(Owner, new Point3D(Center.X + (2 * i - 1) * l,
                            Center.Y + (2 * j - 1) * l, Center.Z + (2 * k - 1) * l), l);
                        Children[i, j, k].SetR();
                        Children[i, j, k].Parent = this;
                    }
                }
            }

            Owner.NodeSplitRaise(this);
        }

        /// <summary>
        /// Gets the child node <see cref="Mill5C.Core.DataStructures.Node"/> at the specified [0,7] index.
        /// </summary>
        /// <value></value>
        public Node this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Children[0, 0, 0];
                    case 1:
                        return Children[0, 1, 0];
                    case 2:
                        return Children[1, 0, 0];
                    case 3:
                        return Children[1, 1, 0];
                    case 4:
                        return Children[0, 0, 1];
                    case 5:
                        return Children[0, 1, 1];
                    case 6:
                        return Children[1, 0, 1];
                    case 7:
                        return Children[1, 1, 1];
                    default:
                        throw new IndexOutOfRangeException("index must be < 8 (iz octree)");
                }
            }
        }
    }

}
