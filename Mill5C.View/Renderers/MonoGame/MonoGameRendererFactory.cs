using Mill5C.View.Window.Renderers;
using Mill5C.View.Window.Settings;
using Mill5C.View.Window.Views;
using Mill5C.View.Window.Views.MonoGame; // Required to cast IDrawingView to Mill5CGameViewModel

namespace Mill5C.View.Window.Renderers.MonoGame
{
    public class MonoGameRendererFactory : IRendererFactory
    {
        public void FillView(
            IDrawingView view, // This is expected to be a Mill5CGameViewModel instance
            MaterialRendererType materialRenderer,
            bool materialRendererDrawCubes,
            CutterRendererType cutterRenderer,
            PathRendererType pathRenderer)
        {
            // The 'view' object here is the Mill5CGameViewModel instance passed from AppController.
            // The renderers (which inherit from MonoGameRendererBase) expect this specific type
            // in their Initialize method's 'scene' parameter to access GameViewModel properties.
            // No explicit cast is needed here if IDrawingView already has the necessary properties,
            // but the renderers' Initialize methods will cast `scene` to `Mill5CGameViewModel`.

            switch (materialRenderer)
            {
                case MaterialRendererType.None:
                    view.MaterialRenderer = null;
                    break;
                case MaterialRendererType.RealTime:
                    // Assuming RealTimeOctreeRenderer is now in Mill5C.View.Window.Renderers.MonoGame
                    view.MaterialRenderer = new RealTimeOctreeRenderer(materialRendererDrawCubes);
                    break;
                case MaterialRendererType.PostSimulation:
                    // Assuming PostSimulationOctreeRenderer is now in Mill5C.View.Window.Renderers.MonoGame
                    view.MaterialRenderer = new PostSimulationOctreeRenderer(materialRendererDrawCubes);
                    break;
                default:
                    view.MaterialRenderer = null;
                    break;
            }

            switch (pathRenderer)
            {
                case PathRendererType.None:
                    view.PathRenderer = null;
                    break;
                case PathRendererType.Standard:
                    // Assuming PathRenderer is now in Mill5C.View.Window.Renderers.MonoGame
                    view.PathRenderer = new PathRenderer();
                    break;
                default:
                    view.PathRenderer = null;
                    break;
            }

            switch (cutterRenderer)
            {
                case CutterRendererType.None:
                    view.CutterRenderer = null;
                    break;
                case CutterRendererType.Standard:
                    // Assuming CutterRenderer is now in Mill5C.View.Window.Renderers.MonoGame
                    view.CutterRenderer = new CutterRenderer();
                    break;
                default:
                    view.CutterRenderer = null;
                    break;
            }
        }
    }
}
