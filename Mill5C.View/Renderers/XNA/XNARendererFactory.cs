using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mill5C.View.Window.Settings;
using Mill5C.View.Window.Views;

namespace Mill5C.View.Window.Renderers.XNA
{
    public class XNARendererFactory : Mill5C.View.Window.Renderers.IRendererFactory
    {
        public void FillView(
            IDrawingView view,
            MaterialRendererType materialRenderer,
            bool materialRendererDrawCubes,
            CutterRendererType cutterRenderer,
            PathRendererType pathRenderer)
        {

            switch (materialRenderer)
            {
                case MaterialRendererType.None:
                    view.MaterialRenderer = null;
                    break;
                case MaterialRendererType.RealTime:
                    view.MaterialRenderer = new RealTimeOctreeRenderer(materialRendererDrawCubes);
                    break;
                case MaterialRendererType.PostSimulation:
                    view.MaterialRenderer = new PostSimulationOctreeRenderer(materialRendererDrawCubes);
                    break;
                default:
                    break;
            }

            switch (pathRenderer)
            {
                case PathRendererType.None:
                    view.PathRenderer = null;
                    break;
                case PathRendererType.Standard:
                    view.PathRenderer = new PathRenderer();
                    break;
                default:
                    break;
            }

            switch (cutterRenderer)
            {
                case CutterRendererType.None:
                    view.CutterRenderer = null;
                    break;
                case CutterRendererType.Standard:
                    view.CutterRenderer = new CutterRenderer();
                    break;
                default:
                    break;
            }


        }
    }
}
