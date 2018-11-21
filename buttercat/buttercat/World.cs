using SkiaSharp.Views.Tizen;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Random random;

        List<Pipe> pipes = new List<Pipe>();

        float lastPipeTime;

        public World(int fps)
        {
            Name = "World";

            clipPath = new SKPath();
            clipPath.AddCircle(180, 180, 180);
            this.fps = fps;

            IsClickable = true;
            startButton = new StartButton()
            {
                IsClickable = true
            };
            butterCat = new ButterCat();
            butter = new Butter();
            Children.Add(startButton);
            Children.Add(butterCat);
            Children.Add(butter);

            startButton.Clicked += (s, e) => {
                startButton.IsVisible = false;
                if (!isStarted)
                {
                    butter.Animate().ContinueWith(t => StartGame());
                }
            };

            random = new Random();
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

            if (isStarted)
            {
                foreach (var child in pipes.ToArray())
                {
                    child.Move(-1);
                    if (child.Geometry.Right <= 0)
                    {
                        Children.Remove(child);   
                        pipes.Remove(child);
                    }
                    
                    /*
                    if (child.Geometry.Left < butterCat.Geometry.Right &&
                        child.Geometry.Right > butterCat.Geometry.Left &&
                        child.Geometry.Top < butterCat.Geometry.Bottom &&
                        child.Geometry.Bottom > butterCat.Geometry.Top)
                    {
                        EndGame();
                    }
                    */
                    if (child.Geometry.IntersectsWith(butterCat.Geometry))
                    {
                        EndGame();
                    }
                }

                int r;
                if (lastPipeTime++ > 55 && (r = random.Next(0, 101)) % 30 < 5)
                {
                    var pipe = new Pipe(random.Next(50, 100), r > 30);
                    pipes.Add(pipe);
                    Children.Add(pipe);

                    if (r > 60)
                    {
                        var pipe2 = new Pipe(random.Next(50, 120), false);
                        pipes.Add(pipe2);
                        Children.Add(pipe2);
                    }
                    lastPipeTime = 0;
                }
            }
        }

        void OnClick(object sender, EventArgs e)
        {
            butterCat.Jump();
        }

        void StartGame()
        {
            butterCat.State = CatState.Running;
            isStarted = true;
            Clicked += OnClick;
        }

        void EndGame()
        {
            isStarted = false;
            Clicked -= OnClick;
            butterCat.Surprise().ContinueWith(t =>
            {
                startButton.IsVisible = true;

                foreach (var pipe in pipes.ToArray())
                {
                    pipes.Remove(pipe);
                    Children.Remove(pipe);
                }
                butterCat.State = CatState.Walking;
            });
        }
    }
}
