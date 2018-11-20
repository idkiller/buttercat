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

        bool isJump;

        const float timeDifference = 8; 

        public ButterCat()
        {
            geometry = new SKRect(walkingRect.Left, walkingRect.Top, walkingRect.Right, walkingRect.Bottom);
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

                    geometry = isRunning ?
                        new SKRect(runningRect.Left, runningRect.Top, runningRect.Right, runningRect.Bottom) :
                        new SKRect(walkingRect.Left, walkingRect.Top, walkingRect.Right, walkingRect.Bottom) ;

                        Console.WriteLine(geometry);
                                
                }
            }
        }

        public void Jump()
        {
            isJump = true;
            accel = gravity;
            velocity = -gravity * 60;
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;
            var start = ((int)(count/3) % 6) * geometry.Width;

            if (isRunning)
            {
                if (isJump)
                {
                    geometry.Offset(0, (velocity * timeDifference) + (accel * (timeDifference * timeDifference) / 2));
                    velocity = velocity + (accel * timeDifference);
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
            }
            canvas.DrawImage(IsRunning ? Resource.CatRuns : Resource.CatWalk, new SKRect(start, 0, start + geometry.Width, geometry.Height), geometry);
            count = count + 1 == 18 ? 0 : count + 1;
        }
    }
}
