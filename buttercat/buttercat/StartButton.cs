using System;
using SkiaSharp;
using SkiaSharp.Views.Tizen;

namespace buttercat
{
    public class StartButton : Drawable
    {
        SKRect geometry = new SKRect(72, 100, 93 + 215, 200);

        SKImage image;
        public StartButton()
        {
            image = Resource.Start;
        }

        public override SKRect Geometry => geometry;

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;

            canvas.DrawImage(image, geometry);
        }

        protected override void OnTouch(object sender, TouchEventArgs e)
        {
            base.OnTouch(sender, e);
            if (e.State == TouchState.Start)
            {
                image = Resource.Start2;
            }
            else
            {
                image = Resource.Start;
            }
        }
    }
}