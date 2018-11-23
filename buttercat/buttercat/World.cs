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
        DistanceLabel distance;
        Random random;

        List<Pipe> pipes = new List<Pipe>();

        float lastPipeTime;

        public World(int fps) : base(null)
        {
            Name = "World";

            clipPath = new SKPath();
            clipPath.AddCircle(180, 180, 180);
            this.fps = fps;

            IsClickable = true;
            startButton = new StartButton(this)
            {
                IsClickable = true
            };
            butterCat = new ButterCat(this);
            butter = new Butter(this);

            startButton.Clicked += (s, e) => {
                startButton.IsVisible = false;
                if (!isStarted)
                {
                    butter.Animate().ContinueWith(t => StartGame());
                }
            };

            random = new Random();

            distance = new DistanceLabel(this)
            {
                IsVisible = false,
                ZOrder = 1
            };
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
                if (lastPipeTime++ > 80)
                {
                    distance.Distance++;
                    if ((r = random.Next(0, 101)) % 30 < 5)
                    {
                        var pipe = new Pipe(this, random.Next(50, 80), r > 30);
                        pipes.Add(pipe);

                        if (r > 60)
                        {
                            var pipe2 = new Pipe(this, random.Next(50, 120), false);
                            pipes.Add(pipe2);
                        }
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
            distance.IsVisible = true;
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
            distance.Distance = 0;
            //distance.IsVisible = false;
        }
    }
}
