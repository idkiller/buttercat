using SkiaSharp.Views.Tizen;
using SkiaSharp;
using System;
using System.Threading.Tasks;

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
        SKImage image;
        CatState state;
        TaskCompletionSource<bool> surprisingDone;

        public ButterCat()
        {
            Name = "ButterCat";
            SetState(CatState.Walking);
            Slow = 3;
        }

        public override SKRect Geometry => geometry;

        public int Slow { get; set; }

        public CatState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    SetState(value);
                }
            }
        }

        public void Jump()
        {
            isJump = true;
            accel = gravity;
            velocity = -gravity * 60;
        }

        public Task Surprise()
        {
            surprisingDone = new TaskCompletionSource<bool>();
            State = CatState.Surprising;
            return surprisingDone.Task;
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            switch (State)
            {
                case CatState.Walking:
                    WalkingDraw(args.Canvas);
                    break;
                case CatState.Running:
                    RunningDraw(args.Canvas);
                    break;
                case CatState.Surprising:
                    SurprisingDraw(args.Canvas);
                    break;
            }
        }

        void SetState(CatState catState)
        {
            state = catState;
            switch(state)
            {
                case CatState.Walking:
                    image = Resource.CatWalk;
                    geometry = new SKRect(walkingRect.Left, walkingRect.Top, walkingRect.Right, walkingRect.Bottom);
                    break;
                case CatState.Running:
                    image = Resource.CatRuns;
                    geometry = new SKRect(runningRect.Left, runningRect.Top, runningRect.Right, runningRect.Bottom);
                    break;
                case CatState.Surprising:
                    image = Resource.CatRuns;
                    break;
            }
        }

        void WalkingDraw(SKCanvas canvas)
        {
            var start = ((int)(count/Slow) % 6) * geometry.Width;
            canvas.DrawImage(image, new SKRect(start, 0, start + geometry.Width, geometry.Height), geometry);
            count = count + 1 == (Slow*6) ? 0 : count + 1;
        }

        void RunningDraw(SKCanvas canvas)
        {
            var start = ((int)(count/Slow) % 6) * geometry.Width;

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
            canvas.DrawImage(image, new SKRect(start, 0, start + geometry.Width, geometry.Height), geometry);
            count = count + 1 == Slow*6 ? 0 : count + 1;
        }

        void SurprisingDraw(SKCanvas canvas)
        {
            geometry.Offset(0, (velocity * timeDifference) + (accel * (timeDifference * timeDifference) / 2));
            velocity = velocity + (accel * timeDifference);

            geometry.Offset(-2, 0);
            if (geometry.Bottom >= runningRect.Bottom)
            {
                geometry = runningRect;
                surprisingDone.TrySetResult(true);
            }
            canvas.DrawImage(image, new SKRect(0, 0, geometry.Width, geometry.Height), geometry);
        }
    }

    public enum CatState
    {
        Walking,
        Running,
        Surprising
    }
}
