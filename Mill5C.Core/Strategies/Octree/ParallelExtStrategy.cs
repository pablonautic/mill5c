using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Interpolators;
using Mill5C.Core.DataStructures;
using Mill5C.Core.Cutters;
using System.Threading;
using Mill5C.Core.Strategies.Octree;
using Mill5C.Core.Materials;

namespace Mill5C.Core.Strategies.Octree
{
    /// <summary>
    /// Class representing a strategy utilizing the Parallel Extensions library.
    /// </summary>
    public class ParallelExtStrategy : BasicSingleThreadedStrategy
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets the value indicating at what level of recursion the strategy should switch
        /// to single-threaded.
        /// </summary>
        /// <value>The max parallel depth.</value>
        public int MaxParallelDepth { get; set; }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>The material.</value>
        public new OctreeMaterial Material 
        {
            get { return (OctreeMaterial)base.Material; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelExtStrategy"/> class.
        /// </summary>
        /// <param name="interpolator">The interpolator.</param>
        /// <param name="maxParallelDepth">The max parallel depth.</param>
        public ParallelExtStrategy(IInterpolator interpolator, int maxParallelDepth)
            : base(interpolator)
        {
            MaxParallelDepth = maxParallelDepth;
        }

        /// <summary>
        /// Init this strategy using the given material. Called by the engine.
        /// </summary>
        /// <param name="material">The material.</param>
        public override void Init(Mill5C.Core.Materials.IMaterial material)
        {
            if (!material.GetType().Equals(typeof(OctreeMaterial)))
            {
                throw new Mill5C.Core.Utility.Mill5CException("this class works with OctreeMaterial only"); 
            }
            base.Init(material);
            Material.RecurrenceStrategy = ParallelRecurrence;
        }

        /// <summary>
        /// Use this method to log detailed parameters of the strategy. Called by the engine.
        /// </summary>
        public override void LogDetails()
        {
            if (log.IsInfoEnabled)
                log.Info("Maximum parallel invocation depth = " + MaxParallelDepth);
        }

        private bool ParallelRecurrence(Node node, ICutter cutter, int recurrenceDepth)
        {
            bool white = true;

            if (recurrenceDepth < MaxParallelDepth)
            {              
                Parallel.For(0, 8, i =>
                {
                    if (node[i].Color != NodeColor.White)
                    {
                        Material.EvalNode(node[i], cutter, recurrenceDepth + 1, ParallelRecurrence);
                        white = false;
                    }
                });
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (node[i].Color != NodeColor.White)
                    {
                        Material.EvalNode(node[i], cutter, recurrenceDepth + 1, Material.DefaultRecurrence);
                        white = false;
                    }
                }
            }
            return white;
        }



    }
}
