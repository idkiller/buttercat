using SkiaSharp;
using System.Threading.Tasks;

namespace buttercat
{
    public class Butter : Drawable
    {
        SKRect geometry;
        public override SKRect Geometry => geometry;

        SKRect start = new SKRect(168, -12, 168 + 25, 0);
        SKRect target = new SKRect(168, 288, 168+25, 288 + 12);

        TaskCompletionSource<bool> tcs;

        public Butter()
        {
            IsVisible = false;
        }

        public Task<bool> Animate()
        {
            geometry = start;
            IsVisible = true;
            tcs = new TaskCompletionSource<bool>();

            return tcs.Task;
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            if (IsVisible)
            {
                var canvas = args.Canvas;
                canvas.DrawImage(Resource.Butter, Geometry);
                geometry.Offset(0, 15);

                if (geometry.Location.Y > target.Location.Y)
                {
                    tcs.TrySetResult(true);
                    IsVisible = false;
                }
            }
        }
    }
}
