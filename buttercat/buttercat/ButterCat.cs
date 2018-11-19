using SkiaSharp.Views.Tizen;
using SkiaSharp;
using System;

namespace buttercat
{
    public class ButterCat : Drawable
    {
        int count;

        bool isRunning;

        static SKRect runningRect = new SKRect(140, 290, 140+80, 290+68);
        static SKRect walkingRect = new SKRect(144, 288, 144+72, 288+70);

        float velocity;
        const float gravity = 0.025f;
        float accel;

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

        bool isJump;

        public void Jump()
        {
            isJump = true;
            accel = gravity;
            velocity = -gravity * 80;
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;
            var start = ((int)(count/3) % 6) * geometry.Width;

            if (isJump)
            {
                geometry.Offset(0, (velocity * 16) + (accel * (16 * 16) / 2));
                velocity = velocity + (accel * 16);
            }

            if (geometry.Bottom > runningRect.Bottom)
            {
                geometry = runningRect;
                isJump = false;
            }

            if (geometry.Top < 0)
            {
                geometry.Offset(0, -geometry.Top);
            }
            canvas.DrawImage(IsRunning ? Resource.CatRuns : Resource.CatWalk, new SKRect(start, 0, start + Geometry.Width, Geometry.Height), Geometry);
            count = count + 1 == 18 ? 0 : count + 1;
        }
    }
}
