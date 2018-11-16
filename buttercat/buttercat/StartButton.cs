using SkiaSharp;
using SkiaSharp.Views.Tizen;

namespace buttercat
{
    public class StartButton : Drawable
    {
        SKRect geometry = new SKRect(72, 100, 93 + 215, 200);
        public override SKRect Geometry => geometry;

        protected override void OnDraw(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;

            canvas.DrawImage(Resource.Start, geometry);
        }
    }
}