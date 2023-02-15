using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Interpolators;
using Mill5C.Core.Materials;
using Mill5C.Core.Utility;
using Mill5C.Core.DataStructures;
using Mill5C.Core.Cutters;

namespace Mill5C.Core.Strategies.Octree
{
    /// <summary>
    /// This strategy splits the material into parts and assigns this parts to tasks.
    /// After running the task full simulation is performed for every assigned node.
    /// </summary>
    public class BasicMultiThreadedStrategy : IStrategy
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This event should be risen when a particular path is completed.
        /// It is used internally be the Engine and should not be handled.
        /// For external event handling use Engine PathCompleted event.
        /// </summary>
        public event EventHandler PathCompleted;

        /// <summary>
        /// Returns the instance of a currently used cutter.
        /// </summary>
        /// <value>The reference cutter.</value>
        public ICutter ReferenceCutter { get; set;}

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>The material.</value>
        public OctreeMaterial Material { get; private set; }

        /// <summary>
        /// Gets or sets the reference interpolator.
        /// </summary>
        /// <value>The reference interpolator.</value>
        public IInterpolator ReferenceInterpolator { get; private set; }

        /// <summary>
        /// Gets the number of threads designated to run this instance.
        /// </summary>
        /// <value>The threads number.</value>
        public int ThreadsNumber { get; private set; }

        /// <summary>
        /// Gets the list of tasks to be performed by this instance.
        /// </summary>
        /// <value>The tasks.</value>
        public List<Task> Tasks { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicMultiThreadedStrategy"/> class.
        /// </summary>
        /// <param name="interpolator">The interpolator.</param>
        /// <param name="threadsNumber">number of threads designated to run this instance</param>
        public BasicMultiThreadedStrategy(IInterpolator interpolator, int threadsNumber)
        {
            ReferenceInterpolator = interpolator;
            ThreadsNumber = threadsNumber;     
        }

        /// <summary>
        /// Init this strategy using the given material. Called by the engine.
        /// </summary>
        /// <param name="material">The material.</param>
        public void Init(Mill5C.Core.Materials.IMaterial material)
        {
            if (!(material is OctreeMaterial))
                throw new Mill5CException("this class is intended to use with OctreeMaterial only", new ArgumentException());
            Material = (OctreeMaterial)material;

        }

        /// <summary>
        /// Prepares the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="cutter">The cutter.</param>
        public void PreparePath(Mill5C.Core.Path.Path path, ICutter cutter)
        {
            ReferenceInterpolator.Cutter = ReferenceCutter = cutter;
            ReferenceInterpolator.Path = path;
            PrepareTasks();
        }

        /// <summary>
        /// Prepares the tasks.
        /// </summary>
        private void PrepareTasks()
        {
            Tasks = new List<Task>(ThreadsNumber);

            for (int i = 0; i < ThreadsNumber; i++)
            {
                Task task = new Task(ReferenceInterpolator, ReferenceCutter, Material, i);
                Tasks.Add(task);
                task.Cutter.Id = i;
                task.Cutter.ConfigurationChanged += Cutter_ConfigurationChanged;
            }

            List<Node> nodesToAssign = new List<Node>();
            GatherNodesToAssign(Material.Tree.Root, nodesToAssign, ThreadsNumber);

            int taskIdx = 0;
            for (int i = 0; i < nodesToAssign.Count; i++)
            {
                Tasks[taskIdx].AssignedNodes.Add(nodesToAssign[i]);
                taskIdx++;
                taskIdx %= Tasks.Count;
            }
        }

        private void Cutter_ConfigurationChanged(object sender, EventArgs e)
        {
            ReferenceCutter.Position = ReferenceCutter.Position; // raise ConfigurationChanged event
        }

        /// <summary>
        /// Recursively splits the tree and creates a linear list of nodes to be assigned to tasks.
        /// </summary>
        /// <param name="node">The node to split</param>
        /// <param name="nodesToAssign">list of nodes to be filled</param>
        /// <param name="targetCount">The target thread count.</param>
        private void GatherNodesToAssign(Node node, List<Node> nodesToAssign, int targetCount)
        {
            if (nodesToAssign.Count >= targetCount)
                return;

            if (node.Children == null) //if the node is already split, skip it and just assign the nodes
                node.Split();

            nodesToAssign.Remove(node);
            nodesToAssign.AddRange(node.Children.Cast<Node>());

            foreach (var child in node.Children)
            {
                GatherNodesToAssign(child, nodesToAssign, targetCount);
            }
        }

        //old version working for 1,2,4 and 8 threads

        //private void PrepareTasks()
        //{
        //begin from splitting the material so we get 8 nodes
        //    Material.Tree.Root.Split();

        //    Tasks = new List<Task>(ThreadsNumber);

        //    for (int i = 0; i < ThreadsNumber; i++)
        //    {
        //        Task task = new Task(ReferenceInterpolator, ReferenceCutter, Material);
        //        Tasks.Add(task);

        //        int nodesPerThread = 8 / ThreadsNumber;

        //        for (int j = 0; j < nodesPerThread; j++)
        //        {
        //            task.AssignedNodes.Add(Material.Tree.Root[nodesPerThread * i + j]);
        //        }         
        //    }     
        //}

        /// <summary>
        /// Begins the path processing.
        /// </summary>
        public void BeginProcessingPath()
        {
            foreach (var task in Tasks)
            {
                new Action(task.Run).BeginInvoke(new AsyncCallback(TaskCompleteCallback), task);
            }
        }

        private void TaskCompleteCallback(IAsyncResult result)
        {
            if (Tasks.Count(task => !task.IsCompleted) == 0)
            {
                if (PathCompleted != null)
                    PathCompleted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Use this method to log detailed parameters of the strategy. Called by the engine.
        /// </summary>
        public virtual void LogDetails()
        {
            if (log.IsInfoEnabled)
                log.Info("Number of assigned threads: " + ThreadsNumber);
        }

        /// <summary>
        /// Resets this instance to the initial state.
        /// </summary>
        public void Reset()
        {
            foreach (var task in Tasks)
            {
                task.Reset();
            }
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            foreach (var task in Tasks)
            {
                task.Cancel();
            }
        }

        /// <summary>
        /// Gets a value indicating whether cancelation is pending.
        /// </summary>
        /// <value><c>true</c> if [cancel pending]; otherwise, <c>false</c>.</value>
        public bool CancelPending 
        { 
            get 
            {
                return Tasks.Count(task => task.CancelPending) > 0;
            } 
        }

        /// <summary>
        /// Represents a task for a single thread to perform.
        /// </summary>
        public class Task
        {   
            /// <summary>
            /// Gets all nodes assigned to this task.
            /// </summary>
            /// <value>The assigned nodes.</value>
            public List<Node> AssignedNodes { get; private set; }

            /// <summary>
            /// Gets the interpolator.
            /// </summary>
            /// <value>The interpolator.</value>
            public IInterpolator Interpolator { get; private set; }

            /// <summary>
            /// Gets the cutter.
            /// </summary>
            /// <value>The cutter.</value>
            public ICutter Cutter { get; private set; }

            /// <summary>
            /// Gets the material.
            /// </summary>
            /// <value>The material.</value>
            public OctreeMaterial Material { get; private set; }

            /// <summary>
            /// Gets a value indicating whether this instance is completed.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is completed; otherwise, <c>false</c>.
            /// </value>
            public bool IsCompleted { get; private set; }

            /// <summary>
            /// Gets a value indicating whether cancelation is pending.
            /// </summary>
            /// <value><c>true</c> if [cancel pending]; otherwise, <c>false</c>.</value>
            public bool CancelPending { get; private set; }

            /// <summary>
            /// Gets or sets the test id.
            /// </summary>
            /// <value>The id.</value>
            public int Id { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Task"/> class.
            /// </summary>
            /// <param name="referenceInterpolator">The reference interpolator.</param>
            /// <param name="referenceCutter">The reference cutter.</param>
            /// <param name="material">The material.</param>
            /// <param name="id">The task id</param>
            public Task(IInterpolator referenceInterpolator, ICutter referenceCutter, OctreeMaterial material, int id)
            {
                AssignedNodes = new List<Node>();
                Material = material;
                Interpolator = referenceInterpolator.Clone();
                Cutter = referenceCutter.Clone();
                Id = id;

                Interpolator.Cutter = Cutter;
            }

            /// <summary>
            /// Runs this instance.
            /// </summary>
            public void Run()
            {
                if (log.IsDebugEnabled)
                    log.Debug("Started task no " + Id.ToString());

                while (Interpolator.HasMoreSteps)
                {
                    if (CancelPending)
                    {
                        CancelPending = false;
                        return;
                    }
                    Interpolator.Step();
                    foreach (var node in AssignedNodes)
                    {
                        Material.EvalNode(node, Cutter, 0);
                    }
                }
                IsCompleted = true;
            }

            /// <summary>
            /// Cancels this instance.
            /// </summary>
            public void Cancel()
            {
                CancelPending = true;
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public void Reset()
            {
                Interpolator.Reset();
                IsCompleted = false;
            }
        }

    }
}
