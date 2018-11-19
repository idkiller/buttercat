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

        bool isStarted;

        SKRect geometry = new SKRect(0, 0, 360, 360);

        StartButton startButton;
        ButterCat butterCat;
        Butter butter;

        public World(int fps)
        {
            clipPath = new SKPath();
            clipPath.AddCircle(180, 180, 180);
            this.fps = fps;

            startButton = new StartButton();
            butterCat = new ButterCat();
            butter = new Butter();
            Children.Add(startButton);
            Children.Add(butterCat);
            Children.Add(butter);

            startButton.Clicked += (s, e) => {
                startButton.IsVisible = false;
                if (!isStarted)
                {
                    butter.Animate().ContinueWith(t => {
                        butterCat.IsRunning = true;
                        isStarted = true;

                        Clicked += OnClick;
                    });
                }
            };

            Name = "World";
        }

        public SKColor SkyColor { get; set; } = SKColors.SkyBlue;

        public override SKRect Geometry => geometry;

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;

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

        void OnClick(object sender, EventArgs e)
        {
            butterCat.Jump();
        }
    }
}
