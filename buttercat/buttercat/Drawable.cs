using ElmSharp;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using System;
using System.Collections.Generic;

namespace buttercat
{
    public abstract class Drawable
    {
        List<Drawable> children;

        public Drawable()
        {
            children = new List<Drawable>();
        }

        public string Name { get; protected set; }

        public abstract SKRect Geometry { get; }

        public List<Drawable> Children => children;

        public bool IsClickable { get; set; }

        public event EventHandler Clicked;

        public bool DispatchClicked(int x, int y)
        {
            bool isCanceled = false;
            foreach (var child in children)
            {
                isCanceled = child.DispatchClicked(x, y);
            }

            if (IsClickable) return false;

            if (isCanceled) return true;

            if (Geometry.Left < x && Geometry.Right > x && Geometry.Top < y && Geometry.Bottom > y)
            {
                var args = new ClickEventArgs();
                Clicked?.Invoke(this, args);
                return args.IsCanceled;
            }
            return false;
        }

        public void Draw(SKPaintSurfaceEventArgs e, SKRect dirtyArea)
        {
            if (Geometry.Right <= dirtyArea.Right && Geometry.Right >= dirtyArea.Left && 
            Geometry.Top <= dirtyArea.Bottom && Geometry.Bottom >= dirtyArea.Top)
            {
                OnDraw(this, e);

                foreach (var drawable in Children)
                {
                    drawable.Draw(e, dirtyArea);
                }
            }
        }

        protected abstract void OnDraw(object sender, SKPaintSurfaceEventArgs e);
    }

    public class ClickEventArgs : EventArgs
    {
        public bool IsCanceled { get; set; }
    }
}
