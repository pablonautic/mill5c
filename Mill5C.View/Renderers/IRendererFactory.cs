using System;
using Mill5C.View.Window.Views;
using Mill5C.View.Window.Settings;
namespace Mill5C.View.Window.Renderers
{
    public interface IRendererFactory
    {
        void FillView(
        IDrawingView view,
        MaterialRendererType materialRenderer,
        bool materialRendererDrawCubes,
        CutterRendererType cutterRenderer,
        PathRendererType pathRenderer);
    }
}
