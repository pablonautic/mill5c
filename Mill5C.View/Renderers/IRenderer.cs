using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.Core.Algorithm;
using System.Windows.Media.Media3D;

namespace Mill5C.View.Window.Renderers
{
    public interface IRenderer
    {
        void Initialize(Engine engine, object scene);

        void AttachEvents(Engine engine);

        void DetachEvents(Engine engine);

        bool Visible { get; set; }
    }
}
