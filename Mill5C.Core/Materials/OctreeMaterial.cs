using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.DataStructures;
using Mill5C.Core.Utility;
using Mill5C.Core.Geometry;
using Mill5C.Core.Cutters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Mill5C.Core.Materials
{
    using RecurrenceStrategySignature = Func<Node, ICutter, int, bool>;

    /// <summary>
    /// This class defines an octree-based material implementation.
    /// </summary>
    public class OctreeMaterial : IMaterial
    {     
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Occurs when a node has reached it minimal size.
        /// </summary>
        public event EventHandler NodeEpsReached;

        /// <summary>
        /// Return the octree data structure holding the data
        /// </summary>
        /// <value>The tree.</value>
        public Octree Tree { get; private set; }

        /// <summary>
        /// Gets or sets the minimal size of a node (reccurence stop condition).
        /// </summary>
        /// <value>The eps.</value>
        public float Eps { get; set; }

        /// <summary>
        /// Gets or sets the recurrence strategy defining how to perform the going-down.
        /// </summary>
        /// <value>The recurrence strategy.</value>
        public RecurrenceStrategySignature RecurrenceStrategy { get; set; }

        /// <summary>
        /// Gets the value indicating the deepest level of recursion reached so far
        /// (= height of the internal tree)
        /// </summary>
        /// <value>The max recurrence depth reached.</value>
        public int MaxRecurrenceDepthReached { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeMaterial"/> class.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="l">The length the cube side</param>
        /// <param name="eps">The stop condition.</param>
        public OctreeMaterial(Point3D center, float l, float eps)
        {
            Tree = new Octree(center, l);
            Eps = eps;
            RecurrenceStrategy = DefaultRecurrence;
        }

        /// <summary>
        /// Use this method to log detailed parameters of the material. Called by the engine.
        /// </summary>
        public void LogDetails()
        {
            if (log.IsInfoEnabled)
                log.Info("Tree center: " + Tree.Root.Center + ", tree l=" + Tree.Root.L + ", Lstop = " + Eps);
        }

        /// <summary>
        /// Use this method to log summary information. Called by the engine.
        /// </summary>
        public void LogSummary()
        {
            if (log.IsInfoEnabled)
                log.Info("Maximum recurrence depth reached was: " + MaxRecurrenceDepthReached);    
        }

        /// <summary>
        /// Intersects this instance with a specified cutter instance.
        /// </summary>
        /// <param name="cutter">The cutter.</param>
        public void Intersect(ICutter cutter)
        {
            EvalNode(Tree.Root, cutter, 0);
        }

        /// <summary>
        /// Recursively evaluates a node-cutter collision.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="cutter">The cutter.</param>
        /// <param name="recurrenceDepth">The recurrence depth.</param>
        public void EvalNode(Node node, ICutter cutter, int recurrenceDepth)
        {
            EvalNode(node, cutter, recurrenceDepth, RecurrenceStrategy);
        }

        /// <summary>
        /// Recursively evaluates a node-cutter collision.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="cutter">The cutter.</param>
        /// <param name="recurrenceDepth">invocation recurrence depth.</param>
        /// <param name="recurrenceStrategy">The recurrence strategy.</param>
        public void EvalNode(Node node, ICutter cutter, int recurrenceDepth, RecurrenceStrategySignature recurrenceStrategy)
        {
            if (recurrenceDepth > MaxRecurrenceDepthReached)
                MaxRecurrenceDepthReached = recurrenceDepth;

            CollisionType col = cutter.Intersect(node.Center, node.R);

            switch (col)
            {
                case CollisionType.None:
                    //node.Color = NodeColor.Black; // no collision - nothing else to do
                    return;
                case CollisionType.Partial:
                    node.Color = NodeColor.Gray;
                    break;
                case CollisionType.Total:
                    node.Color = NodeColor.White;
                    break;
                default:
                    break;
            }

            if (node.L < Eps)
            {
                if (NodeEpsReached != null)
                    NodeEpsReached(node, EventArgs.Empty);
                return;
            }

            if (node.Children == null)
                node.Split();

            //bool allChildrenAreWhite = Recurrence(node, cutter, recurrenceDepth); //stara wersja

            bool allChildrenAreWhite = recurrenceStrategy.Invoke(node, cutter, recurrenceDepth);

            //if all children are white, mark parent as white
            if (allChildrenAreWhite)
                node.Color = NodeColor.White;
        }

        /// <summary>
        /// Default recurrence method.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="cutter">The cutter.</param>
        /// <param name="recurrenceDepth">The recurrence depth.</param>
        /// <returns>value indicating whether all children are white</returns>
        public bool DefaultRecurrence(Node node, ICutter cutter, int recurrenceDepth)
        {
            bool white = true;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (node.Children[i, j, k].Color != NodeColor.White)
                        {
                            EvalNode(node.Children[i, j, k], cutter, recurrenceDepth + 1);
                            white = false;
                        }
                    }
                }
            }
            return white;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Tree.Reset();
        }

        /// <summary>
        /// Writes this instance to the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            Save(fs);
            fs.Close();
        }

        /// <summary>
        /// Writes this instance to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, Tree);
            bf.Serialize(stream, Eps);
        }

        /// <summary>
        /// Loads data from a file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void Load(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Load(fs);
            fs.Close();
        }

        /// <summary>
        /// Loads data from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Load(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Tree = (Octree)bf.Deserialize(stream);
            Eps = (float)bf.Deserialize(stream);
        }

    }
}
