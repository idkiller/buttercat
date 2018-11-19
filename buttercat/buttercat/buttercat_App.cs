using Tizen.Applications;
using ElmSharp;
using SkiaSharp.Views.Tizen;
using System;
using SkiaSharp;
using System.Threading;
using static ElmSharp.GestureLayer;

namespace buttercat
{
    class App : CoreUIApplication
    {
        AnimatorLoop loop;
        Drawable root;

        GestureLayer gestureLayer;

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

            loop = new AnimatorLoop();
            loop.Processed += (s, e) =>
            {
                view.Invalidate();
            };

            root = new World((int)(1/loop.FrameTime * 3));

            loop.Start();

            gestureLayer = new GestureLayer(conformant);
            gestureLayer.Attach(view);
            gestureLayer.SetTapCallback(GestureType.Tap, GestureState.Start, data => {
                var args = new TouchEventArgs { X = data.X, Y = data.Y, State = TouchState.Start };
                root.DispatchEvent(args);
            });
            gestureLayer.SetTapCallback(GestureType.Tap, GestureState.End, data => {
                var args = new TouchEventArgs { X = data.X, Y = data.Y, State = TouchState.End };
                root.DispatchEvent(args);
            });
            gestureLayer.SetTapCallback(GestureType.Tap, GestureState.Abort, data => {
                var args = new TouchEventArgs { X = data.X, Y = data.Y, State = TouchState.Abort };
                root.DispatchEvent(args);
            });
        }

        void OnRender(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            root.Draw(new DrawEventArgs { Canvas = canvas }, root.Geometry);
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
