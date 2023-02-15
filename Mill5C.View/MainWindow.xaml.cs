using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mill5C.View.Window.Renderers;
using Mill5C.View.Window.Strategies;
using Mill5C.Core.Algorithm;
using Mill5C.Core.Materials;
using Mill5C.Core.Geometry;
using Mill5C.Core.Strategies.Octree;
using Mill5C.Core.Interpolators;
using Mill5C.Settings;
using Microsoft.Win32;
using Mill5C.View.Window.Controllers;
using Mill5C.View.Window.Renderers.WPF;
using Mill5C.View.Window.Renderers.XNA;

namespace Mill5C.View.Window
{
    /// <summary>
    /// Interaction logic for MainWindow, most calls are forwarded to the controller.
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public AppController Controller { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //Controller = new AppController(this, DrawingControl, new WPFRendererFactory());
            Controller = new AppController(this, DrawingControl, new XNARendererFactory());

            DataContext = this;
        }


        private void CloseMenuItemClickHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.BeginSimulation();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.CancelSimulation(); 
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.ResetEngine();
        }

        private void SimulationWizardMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.SimulationSettingsWizard();
        }

        private void SaveCfgMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveSimulationSettings();
        }

        private void OpenCfgMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.OpenSimulationSettings();
        }

        private void SaveResultsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveResults();
        }

        private void OpenResultsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.OpenResults();
        }

        private void TogglePathDisplayMenuItemClickHandler(object sender, RoutedEventArgs e)
        {
            DrawingControl.PathRenderer.Visible = !DrawingControl.PathRenderer.Visible;
        }

        private void ToggleMaterialDisplayMenuItemClickHandler(object sender, RoutedEventArgs e)
        {
            DrawingControl.MaterialRenderer.Visible = !DrawingControl.MaterialRenderer.Visible;
        }

        private void ToggleCutterDisplayMenuItemClickHandler(object sender, RoutedEventArgs e)
        {
            DrawingControl.CutterRenderer.Visible = !DrawingControl.CutterRenderer.Visible;
        }

    }
}
