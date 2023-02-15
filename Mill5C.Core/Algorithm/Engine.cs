using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mill5C.Core.Cutters;
using Mill5C.Core.Geometry;
using Mill5C.Core.Interpolators;
using Mill5C.Core.Strategies;
using Mill5C.Core.Materials;
using System.Management;

namespace Mill5C.Core.Algorithm
{
    /// <summary>
    /// The main class for running simulations, being a configurable frame for specific components.
    /// </summary>
    public sealed class Engine
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The constant defining the name of an one-point-path used when no path files are supplied
        /// (usually when using manual simulation strategy).
        /// </summary>
        public const string MockFilename = "mock_path_F12.i";

        /// <summary>
        /// Occurs when a path has been successfully loaded and is ready for processing.
        /// </summary>
        public event PathFileEventHandler PathPrepared;

        /// <summary>
        /// Occurs when processing a path has begun.
        /// </summary>
        public event PathFileEventHandler PathProcessingStarted;

        /// <summary>
        /// Occurs when processing a path has ended.
        /// </summary>
        public event PathFileEventHandler PathCompleted;

        /// <summary>
        /// Occurs when the engine has finished processing all paths.
        /// </summary>
        public event EventHandler WorkComplete;

        /// <summary>
        /// Gets the current strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IStrategy Strategy { get; private set; }

        /// <summary>
        /// Gets the current material.
        /// </summary>
        /// <value>The material.</value>
        public IMaterial Material { get; private set; }

        /// <summary>
        /// Gets the current cutter factory.
        /// </summary>
        /// <value>The cutter factory.</value>
        public ICutterFactory CutterFactory { get; private set; }

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <value>The current path.</value>
        public Path.Path CurrentPath { get; private set; }

        /// <summary>
        /// Gets the output file if one was provided.
        /// </summary>
        /// <value>The output file.</value>
        public string OutputFile { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the simulation is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Engine"/> was canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; private set; }

        private List<string> inputFiles;

        /// <summary>
        /// Gets the list of input files.
        /// </summary>
        /// <value>The input files.</value>
        public IList<string> InputFiles
        {
            get { return inputFiles.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the index of the currently processed path.
        /// </summary>
        /// <value>The index of the current file.</value>
        public int CurrentFileIndex { get; private set; }

        /// <summary>
        /// Gets the name of the currently processed path.
        /// </summary>
        /// <value>The name of the current file.</value>
        public string CurrentFileName { get { return InputFiles[CurrentFileIndex]; } }

        private System.Diagnostics.Stopwatch pathStopwatch, totalStopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class using the
        /// <see cref="DefaultCutterFactory"/> and null output.
        /// </summary>
        /// <param name="strategy">The strategy to use.</param>
        /// <param name="material">The material to use.</param>
        /// <param name="inputFiles">The list of input files. If null or empty a mock file will be used</param>
        public Engine(IStrategy strategy, IMaterial material, params string[] inputFiles)
            : this(strategy, material, new DefaultCutterFactory(), null, inputFiles)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class using the
        /// <see cref="DefaultCutterFactory"/>
        /// </summary>
        /// <param name="strategy">The strategy to use.</param>
        /// <param name="material">The material to use.</param>
        /// <param name="outputFile">Path to save the result. Can be null if no results should be written.</param>
        /// <param name="inputFiles">The list of input files. If null or empty a mock file will be used</param>
        public Engine(IStrategy strategy, IMaterial material, string outputFile, params string[] inputFiles)
            : this(strategy, material, new DefaultCutterFactory(), outputFile, inputFiles)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="strategy">The strategy to use.</param>
        /// <param name="material">The material to use.</param>
        /// <param name="cutterFactory">The cutter factory to use</param>
        /// <param name="outputFile">Path to save the result. Can be null if no results should be written.</param>
        /// <param name="inputFiles">The list of input files. If null or empty a mock file will be used</param>
        public Engine(IStrategy strategy, IMaterial material, ICutterFactory cutterFactory,
            string outputFile, params string[] inputFiles)
        {
            this.inputFiles = new List<string>();
            if (inputFiles == null || inputFiles.Length == 0)
                this.inputFiles.Add(MockFilename);
            else
                this.inputFiles.AddRange(inputFiles);

            ConfigureLogging();

            pathStopwatch = new System.Diagnostics.Stopwatch();
            totalStopwatch = new System.Diagnostics.Stopwatch();

            Strategy = strategy;
            Material = material;
            CutterFactory = cutterFactory;
            OutputFile = outputFile;

            if (log.IsInfoEnabled)
            {
                log.Info("Using strategy of type: " + Strategy.GetType().Name);
                Strategy.LogDetails();
                log.Info("Using material of type: " + Material.GetType().Name);
                Material.LogDetails();
                log.Info("Using cutter factory of type: " + CutterFactory.GetType().Name);
            }

            Strategy.Init(material);

            LoadCurrentFile();
        }

        private void ConfigureLogging()
        {
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string logFile = System.IO.Path.Combine(dir, "log4net.config");
            if (System.IO.File.Exists(logFile))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(logFile));
                if (log.IsDebugEnabled)
                    log.Debug("logging configured successfully");
            }
            if (log.IsInfoEnabled)
            {
                log.Info("Simulation engine started.");
                LogHardwareInfo();
            }
        }

        private void LogHardwareInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject cpu in searcher.Get())
            {
                var name = cpu.GetPropertyValue("Name");
                object numberOfCores = "unknown";
                object numberOfLogicalProcessors = "unknown";
                try
                {
                    numberOfCores = cpu.GetPropertyValue("NumberOfCores");
                }
                catch (ManagementException)
                {     
                }
                try
                {
                    numberOfLogicalProcessors = cpu.GetPropertyValue("NumberOfLogicalProcessors");
                }
                catch (ManagementException)
                {
                }
                log.Info("CPU: " + name + ", " + numberOfCores
                     + " cores, " + numberOfLogicalProcessors
                     + " logical processors.");
            }
            searcher = new ManagementObjectSearcher("Select * from Win32_PhysicalMemory");
            long memory = 0;
            foreach (ManagementObject cpu in searcher.Get())
                memory += long.Parse(cpu.GetPropertyValue("Capacity").ToString());

            log.Info("Total physical memory: " + memory.ToString() + " bytes.");
        }

        /// <summary>
        /// Begins the simulation. No-op if it's already running or was cancelled and not reset.
        /// </summary>
        public void Start()
        {
            if (IsRunning || Canceled)
                return;
            Strategy.PathCompleted += StrategyPathCompletedHandler;
            IsRunning = true;
            totalStopwatch.Start();
            PrepareAndProcessPath(true);
        }

        /// <summary>
        /// Stops the simulation No-op if it's not running.
        /// </summary>
        public void Stop()
        {
            if (!IsRunning)
                return;
            Strategy.Cancel();
            IsRunning = false;           
            Canceled = true;

            if (log.IsInfoEnabled)
                log.Info("Simulation cancelled");
        }

        private void PrepareAndProcessPath(bool firstPath)
        {
            if (!firstPath)
                LoadCurrentFile();

            pathStopwatch.Start();
            Strategy.BeginProcessingPath();
            PathProcessingStartedRise();
        }

        private void LoadCurrentFile()
        {
            if (CurrentFileName != MockFilename)
                CurrentPath = Path.Path.FromFile(CurrentFileName);
            else
                CurrentPath = Path.Path.Mock(CurrentFileName);

            GC.Collect(); //collect previous path 

            Strategy.PreparePath(CurrentPath, CutterFactory.CreateCutter(CurrentFileName));
            PathPreparedRise();
        }

        private void StrategyPathCompletedHandler(object sender, EventArgs e)
        {
            pathStopwatch.Stop();
            LogTime(pathStopwatch.Elapsed, "Path " + CurrentFileName);
            pathStopwatch.Reset();

            PathCompletedRise();

            if (HasMoreFiles)
            {
                CurrentFileIndex++;
                PrepareAndProcessPath(false);
            }
            else
            {
                totalStopwatch.Stop();
                LogTime(totalStopwatch.Elapsed, "Simulation");
                Material.LogSummary();
                if (log.IsInfoEnabled)
                    log.Info("Peek memory usage was: " + System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64 + " bytes.");
                Strategy.PathCompleted -= StrategyPathCompletedHandler;

                if (OutputFile != null)
                {
                    try
                    {
                        Material.Save(OutputFile);
                        log.Info("Results saved successfully in " + OutputFile);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while writing output file", ex);
                    }
                }

                IsRunning = false;

                if (WorkComplete != null)
                    WorkComplete(this, EventArgs.Empty);

                lock (this)
                {
                    System.Threading.Monitor.PulseAll(this);
                }
            }
        }

        private void LogTime(TimeSpan time, string eventName)
        {
            if (log.IsInfoEnabled)
                log.Info(eventName + " completed in " + time.ToString() + ".");
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            if (IsRunning)
                return;

            if (log.IsInfoEnabled)
                log.Info("Engine reset");

            Canceled = false;

            Strategy.Reset();
            Material.Reset();

            pathStopwatch.Reset();
            totalStopwatch.Reset();

            CurrentFileIndex = 0;
            LoadCurrentFile();

            GC.Collect();
        }

        /// <summary>
        /// Suspends the calling thread until the engine finishes it's work and notifies all waiting threads.
        /// </summary>
        public void Wait()
        {
            lock (this)
            {
                System.Threading.Monitor.Wait(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are more files pending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has more files; otherwise, <c>false</c>.
        /// </value>
        public bool HasMoreFiles
        {
            get
            {
                return CurrentFileIndex < InputFiles.Count - 1;
            }
        }

        private void PathPreparedRise()
        {
            if (log.IsInfoEnabled)
            {
                log.Info("File " + CurrentFileName + " prepared for processing.");
                log.Info("Using cutter of type: " + Strategy.ReferenceCutter.GetType().Name +
                    ", R = " + Strategy.ReferenceCutter.R);
            }

            if (PathPrepared != null)
                PathPrepared(this, new PathFileEventArgs
                {
                    Filename = CurrentFileName,
                    Path = CurrentPath,
                    ReferenceCutter = Strategy.ReferenceCutter
                });
        }

        private void PathProcessingStartedRise()
        {
            if (log.IsInfoEnabled)
                log.Info("File " + CurrentFileName + " started");

            if (PathProcessingStarted != null)
                PathProcessingStarted(this, new PathFileEventArgs
                {
                    Filename = CurrentFileName,
                    Path = CurrentPath,
                    ReferenceCutter = Strategy.ReferenceCutter
                });
        }

        private void PathCompletedRise()
        {
            if (PathCompleted != null)
                PathCompleted(this, new PathFileEventArgs
                {
                    Filename = CurrentFileName,
                    Path = CurrentPath,
                    ReferenceCutter = Strategy.ReferenceCutter
                });
        }
    }

    /// <summary>
    /// Delegate declaration for path-related events.
    /// </summary>
    public delegate void PathFileEventHandler(object sender, PathFileEventArgs args);

    /// <summary>
    /// EventArgs declaration for path-related events.
    /// </summary>
    public class PathFileEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the file related with the event.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the path related with the event.
        /// </summary>
        /// <value>The path.</value>
        public Path.Path Path { get; set; }

        /// <summary>
        /// Gets or sets the reference cutter related with the event.
        /// </summary>
        /// <value>The reference cutter.</value>
        public ICutter ReferenceCutter { get; set; }
    }

}
