using SkiaSharp.Views.Tizen;
using SkiaSharp;
using System;

namespace buttercat
{
    public class World : Drawable
    {
        SKPath clipPath;

        int count = 0;

        int fps;

        SKRect geometry = new SKRect(0, 0, 360, 360);

        public World(int fps)
        {
            clipPath = new SKPath();
            clipPath.AddCircle(180, 180, 180);
            this.fps = fps;

            var startButton = new StartButton();
            var buttercat = new ButterCat();
            Children.Add(startButton);
            Children.Add(buttercat);

            startButton.Clicked += (s, e) => {
                buttercat.IsRunning = true;
            };

            Name = "World";
        }

        public SKColor SkyColor { get; set; } = SKColors.SkyBlue;

        public override SKRect Geometry => geometry;
        protected override void OnDraw(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.ClipPath(clipPath);

            count = (count + 1) % fps;

            using (var paint = new SKPaint())
            {
                paint.Color = SkyColor;
                paint.Style = SKPaintStyle.Fill;
                canvas.DrawPaint(paint);
            }

            var start = Resource.Background.Width / fps * count;
            canvas.DrawImage(Resource.Background,
                new SKRect(start, 0, Resource.Background.Width, Resource.Background.Height),
                new SKRect(0, 0, Geometry.Right - start, Geometry.Bottom));
            if (start > 0)
            {
                canvas.DrawImage(Resource.Background,
                    new SKRect(0, 0, start, Resource.Background.Height),
                    new SKRect(Geometry.Right - start, 0, Geometry.Right, Geometry.Bottom));
            }
        }
    }
}
