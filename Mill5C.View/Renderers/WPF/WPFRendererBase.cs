using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Mill5C.Core.Algorithm;
using System.Windows.Media.Media3D;

namespace Mill5C.View.Window.Renderers.WPF
{
    public abstract class WPFRendererBase : IRenderer
    {
        public ModelVisual3D Scene { get; private set; }

        public Dispatcher Dispatcher { get; private set; }

        public Engine Engine { get; private set; }

        public virtual void Initialize(Engine engine, object scene)
        {
            Engine = engine;
            Scene = (ModelVisual3D)scene;
            Dispatcher = Scene.Dispatcher;
        }

        public abstract void AttachEvents(Engine engine);

        public abstract void DetachEvents(Engine engine);

        public abstract bool Visible
        {
            get;
            set;
        }
    }
}
