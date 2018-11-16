using SkiaSharp.Views.Tizen;
using SkiaSharp;

namespace buttercat
{
    public class ButterCat : Drawable
    {
        int count;
        int delay;

        bool isRunning;

        static SKRect runningRect = new SKRect(140, 290, 140+80, 290+68);
        static SKRect walkingRect = new SKRect(144, 288, 144+72, 288+70);

        SKRect geometry;

        public ButterCat()
        {
            geometry = walkingRect;
            Name = "ButterCat";
            isRunning = false;
        }

        public override SKRect Geometry => geometry;

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    geometry = isRunning ? runningRect : walkingRect;
                }
            }
        }

        protected override void OnDraw(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;

            if (delay++ > 4)
            {
                delay = 0;
                count = (count+1) % 6;
            }

            var start = count * Geometry.Width;
            canvas.DrawImage(IsRunning ? Resource.CatRuns : Resource.CatWalk, new SKRect(start, 0, start + Geometry.Width, Geometry.Height), Geometry);
        }
    }
}
