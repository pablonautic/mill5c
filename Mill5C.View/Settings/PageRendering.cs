using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mill5C.Settings;
using Mill5C.View.Window.Renderers;

namespace Mill5C.View.Window.Settings
{
    public partial class PageRendering : WizardPageBase
    {

        public PageRendering()
        {
            InitializeComponent();
        }

        public MaterialRendererType MaterialRenderer 
        {
            get
            {
                if (NoneMatRadioButton.Checked)
                    return MaterialRendererType.None;
                else if (RealTimeMatRadioButton.Checked)
                    return MaterialRendererType.RealTime;
                else if (PostSimulationMatRadioButton.Checked)
                    return MaterialRendererType.PostSimulation;
                else
                    throw new Exception("unreacheable code");
            }
       
        }

        public bool MaterialRendererDrawCubes
        {
            get { return CubesRadioButton.Checked; }
        }

        public CutterRendererType CutterRenderer
        {
            get
            {
                if (NoneCutterRadioButton.Checked)
                    return CutterRendererType.None;
                else if (StandardCutterRadioButton.Checked)
                    return CutterRendererType.Standard;
                else
                    throw new Exception("unreacheable code");
            }
        }

        public PathRendererType PathRenderer
        {
            get
            {
                if (NonePathRadioButton.Checked)
                    return PathRendererType.None;
                else if (StandardPathRadioButton.Checked)
                    return PathRendererType.Standard;
                else
                    throw new Exception("unreacheable code");
            }
        }
    }
}
