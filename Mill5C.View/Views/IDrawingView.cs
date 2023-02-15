using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.View.Window.Renderers;

namespace Mill5C.View.Window.Views
{
    public interface IDrawingView
    {
        IRenderer MaterialRenderer { get; set; }
        IRenderer PathRenderer { get; set; }
        IRenderer CutterRenderer { get; set; }

        IList<IRenderer> AllRenderers { get; }

        object Scene { get; }

        void CleanUp();

        void Activate();
        void Passivate();
    }
}
