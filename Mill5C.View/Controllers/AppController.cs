using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.View.Window.Views;
using Mill5C.Settings;
using Mill5C.Core.Algorithm;
using Mill5C.View.Window.Strategies;
using Mill5C.Core.Geometry;
using System.Windows;
using Microsoft.Win32;
using Mill5C.Core.Materials;
using Mill5C.View.Window.Settings;
using Mill5C.View.Window.Renderers;

namespace Mill5C.View.Window.Controllers
{
    public class AppController : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public IDrawingView DrawingView { get; private set; }

        private SettingsNode simulationSettings;

        public SettingsNode SimulationSettings 
        {
            get { return simulationSettings; }
            private set
            {
                simulationSettings = value;
                OnPropertyChanged("SimulationSettings");
            }
        }

        public Engine Engine { get; private set; }

        public FrameworkElement Host { get; private set; }

        public IRendererFactory RendererFactory { get; private set; }

        public AppController(FrameworkElement host, IDrawingView drawingView, IRendererFactory rendererFactory)
        {
            Host = host;
            DrawingView = drawingView;
            RendererFactory = rendererFactory;
        }

        public bool BeginSimulation()
        {
            if (Engine != null || PrepareSimulation())
            {
                Engine.Start();
                NofityGUI();
                return true;
            }
            else   
                return false;
        }

        public bool PrepareSimulation()
        {
            if (SimulationSettings == null)
            {
                DisplayError("Simulation settings are not defined. Please load a settings file or use the " +
                    "simulation wizard and try again");
                return false;
            }

            Engine = new GenericObjectFactory<Engine>().Create(SimulationSettings);

            FillAndInitView();

            return true;
        }

        private void FillAndInitView()
        {
            //clean-up previous renderers
            foreach (var renderer in DrawingView.AllRenderers)
                renderer.DetachEvents(Engine);

            DrawingView.CleanUp();

            RendererFactory.FillView(
                DrawingView,
                Properties.Settings.Default.MaterialRendererType,
                Properties.Settings.Default.MaterialRendererDrawCubes,
                Properties.Settings.Default.CutterRendererType,
                Properties.Settings.Default.PathRendererType);

            foreach (var renderer in DrawingView.AllRenderers)
                renderer.Initialize(Engine, DrawingView.Scene);

            foreach (var renderer in DrawingView.AllRenderers)
                renderer.AttachEvents(Engine);

        }

        public bool CancelSimulation()
        {
            Engine.Stop();
            NofityGUI();
            return true;
        }

        public bool ResetEngine()
        {
            Engine.Reset();
            FillAndInitView();
            NofityGUI();
            return true;
        }

        public bool SimulationSettingsWizard()
        {
            SettingsWizard wizard = new SettingsWizard();
            wizard.Page01.ManualControlType = typeof(WPFManualStrategy);
            wizard.Page01.ManualControlArgs = new object[] { Host };

            var renderSettingsPage = new PageRendering();

            wizard.Pages.Add(renderSettingsPage);

            if (!(wizard.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                return false;
            }

            SimulationSettings = wizard.DataDefault;

            Properties.Settings.Default.MaterialRendererType = renderSettingsPage.MaterialRenderer;
            Properties.Settings.Default.MaterialRendererDrawCubes = renderSettingsPage.MaterialRendererDrawCubes;
            Properties.Settings.Default.CutterRendererType = renderSettingsPage.CutterRenderer;
            Properties.Settings.Default.PathRendererType = renderSettingsPage.PathRenderer;
            Properties.Settings.Default.Save();

            PrepareSimulation();

            NofityGUI();

            return true;
        }

        public void DisplayError(string msg)
        {
            MessageBox.Show(msg, "Application error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool SaveSimulationSettings()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML config files|*.xml";
            sfd.FileName = string.Empty;
            if (sfd.ShowDialog().Value)
            {
                try
                {
                    SimulationSettings.Save(sfd.FileName);
                    return true;
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            return false;
        }

        public bool OpenSimulationSettings()
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "XML config files|*.xml";
            sfd.FileName = string.Empty;
            if (sfd.ShowDialog().Value)
            {
                try
                {
                    SimulationSettings = SettingsNode.FromFile(sfd.FileName);
                    PrepareSimulation();
                    NofityGUI();
                    return true;
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            return false;
        }

        public bool SaveResults()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Material files (*.mat)|*.mat";
            sfd.FileName = string.Empty;
            if (sfd.ShowDialog().Value)
            {
                try
                {
                    Engine.Material.Save(sfd.SafeFileName);
                    return true;
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            return false;
        }

        public bool OpenResults()
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Material files (*.mat)|*.mat";
            sfd.FileName = string.Empty;
            if (sfd.ShowDialog().Value)
            {
                try
                {
                    Engine = new Engine(new WPFManualStrategy(Host), new OctreeMaterial(Point3D.Zero, 20, 0.1f));
                    Engine.Material.Load(sfd.SafeFileName);

                    Properties.Settings.Default.MaterialRendererType = MaterialRendererType.PostSimulation;

                    FillAndInitView();

                    Engine.Start();

                    NofityGUI();

                    return true;
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            return false;
        }

        private void NofityGUI()
        {
            OnPropertyChanged("IsWizardEnabled");
            OnPropertyChanged("IsStartEnabled");
            OnPropertyChanged("IsCancelEnabled");
            OnPropertyChanged("IsResetEnabled");
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        public bool IsWizardEnabled
        {
            get { return true; }
        }

        public bool IsStartEnabled
        {
            get { return Engine != null && !Engine.IsRunning && !Engine.Canceled; }
        }

        public bool IsCancelEnabled
        {
            get { return Engine != null && Engine.IsRunning; }
        }

        public bool IsResetEnabled
        {
            get { return Engine != null && Engine.Canceled; }
        }
 
    }
}
