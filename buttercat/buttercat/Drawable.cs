using ElmSharp;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using System;
using System.Collections.Generic;

namespace buttercat
{
    public abstract class Drawable
    {
        bool isTouchStarted;

        public string Name { get; protected set; }

        public abstract SKRect Geometry { get; }

        public List<Drawable> Children { get; private set; } = new List<Drawable>();

        public bool IsClickable { get; set; }

        public bool IsVisible { get; set; } = true;
        public event EventHandler<TouchEventArgs> Touched;

        public event EventHandler Clicked;

        public bool DispatchEvent(TouchEventArgs args)
        {
            bool isCanceled = args.IsCanceled;
            foreach (var child in Children)
            {
                child.DispatchEvent(args);
                isCanceled = isCanceled || args.IsCanceled;
            }
            if (IsClickable) return false;
            if (isCanceled) return true;
            if (Geometry.Left < args.X && Geometry.Right > args.X && Geometry.Top < args.Y && Geometry.Bottom > args.Y)
            {
                OnTouch(this, args);
                return args.IsCanceled;
            }
            return false;
        }
        public void Draw(DrawEventArgs e, SKRect dirtyArea)
        {
            if (IsVisible && Geometry.Right <= dirtyArea.Right && Geometry.Right >= dirtyArea.Left && 
                Geometry.Top <= dirtyArea.Bottom && Geometry.Bottom >= dirtyArea.Top)
            {
                OnDraw(this, e);
            }
            foreach (var drawable in Children)
            {
                drawable.Draw(e, dirtyArea);
            }
        }
        protected abstract void OnDraw(object sender, DrawEventArgs e);

        protected virtual void OnTouch(object sender, TouchEventArgs args)
        {
            Touched?.Invoke(sender, args);
            if (args.State == TouchState.Start)
            {
                isTouchStarted = true;
            }
            else
            {
                if (args.State == TouchState.End && isTouchStarted)
                {
                    Clicked?.Invoke(sender, EventArgs.Empty);
                }
                isTouchStarted = false;
            }
        }
    }

    public class DrawEventArgs : EventArgs
    {
        public SKCanvas Canvas { get; set; }
    }

    public class TouchEventArgs : EventArgs
    {
        public bool IsCanceled { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TouchState State { get; set; }
    }

    public enum TouchState
    {
        Start,
        Move,
        End,
        Abort
    }
}
