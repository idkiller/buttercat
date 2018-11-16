using Tizen.Applications;
using ElmSharp;
using SkiaSharp.Views.Tizen;
using System;
using SkiaSharp;
using System.Threading;

namespace buttercat
{
    class App : CoreUIApplication
    {
        AnimatorLoop loop;
        EvasObjectEvent<EvasMouseUpArgs> mouseUp;

        Drawable root;
        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            Window window = new Window("ElmSharpApp")
            {
                AvailableRotations = DisplayRotation.Degree_0 | DisplayRotation.Degree_180 | DisplayRotation.Degree_270 | DisplayRotation.Degree_90
            };
            window.BackButtonPressed += (s, e) =>
            {
                Exit();
            };
            window.Show();

            var conformant = new Conformant(window);
            conformant.Show();

            var view = new SKCanvasView(conformant);

            conformant.SetContent(view);
            view.Show();

            view.PaintSurface += OnRender;

            mouseUp = new EvasObjectEvent<EvasMouseUpArgs>(view, EvasObjectCallbackType.MouseUp, EvasMouseUpArgs.Create);
            mouseUp.On += (s, e) =>
            {
                root.DispatchClicked(e.Point.X, e.Point.Y);
            };

            loop = new AnimatorLoop();
            loop.Processed += (s, e) =>
            {
                view.Invalidate();
            };

            root = new World((int)(1/loop.FrameTime * 3));

            loop.Start();
        }

        void OnRender(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            root.Draw(args, root.Geometry);
        }

        static void Main(string[] args)
        {
            Elementary.Initialize();
            Elementary.ThemeOverlay();
            App app = new App();
            app.Run(args);
        }
    }
}
