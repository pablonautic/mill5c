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
using Mill5C.Core.Cutters;
using Mill5C.Core.Geometry;
using Mill5C.Core.Utility;
using Primitive3DSurfaces;
using Mill5C.View.Window.Renderers;
using Mill5C.Core.Algorithm;
using Mill5C.View.Window.Views;

namespace Mill5C.View.Window.Views.WPF
{
    /// <summary>
    /// Interaction logic for DrawingControl.xaml
    /// </summary>
    public partial class DrawingControl : UserControl, IDrawingView
    {

        public DrawingControl()
        {
            InitializeComponent();
        }

        public IList<IRenderer> AllRenderers
        {
            get
            {
                var list = new List<IRenderer>();
                if (PathRenderer != null)
                    list.Add(PathRenderer);
                if (CutterRenderer != null)
                    list.Add(CutterRenderer);
                if (MaterialRenderer != null)
                    list.Add(MaterialRenderer);
                return list;
            }
        }

        public IRenderer MaterialRenderer
        {
            get;
            set;
        }

        public IRenderer PathRenderer
        {
            get;
            set;
        }

        public IRenderer CutterRenderer
        {
            get;
            set;
        }

        public object Scene { get { return Scene2; } }


        public void CleanUp()
        {
            
        }

        public void Activate()
        {
            
        }

        public void Passivate()
        {
            
        }

    }
}
