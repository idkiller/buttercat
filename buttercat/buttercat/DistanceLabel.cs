using SkiaSharp;
using System;

namespace buttercat
{
    public class DistanceLabel : Drawable
    {
        const int FontWidth = 46;
        const int FontHeight = 59;
        const int FontKerning = -16;
        static SKRect[] FontMap = 
        {
            new SKRect(0            , 0, FontWidth    , FontHeight),
            new SKRect(FontWidth    , 0, FontWidth * 2, FontHeight),
            new SKRect(FontWidth * 2, 0, FontWidth * 3, FontHeight),
            new SKRect(FontWidth * 3, 0, FontWidth * 4, FontHeight),
            new SKRect(FontWidth * 4, 0, FontWidth * 5, FontHeight),
            new SKRect(FontWidth * 5, 0, FontWidth * 6, FontHeight),
            new SKRect(FontWidth * 6, 0, FontWidth * 7, FontHeight),
            new SKRect(FontWidth * 7, 0, FontWidth * 8, FontHeight),
            new SKRect(FontWidth * 8, 0, FontWidth * 9, FontHeight),
            new SKRect(FontWidth * 9, 0, FontWidth * 10, FontHeight),
            new SKRect(FontWidth * 10, 0, FontWidth * 11, FontHeight),
        };
        SKRect geometry;

        uint distance;

        string digits;

        public override SKRect Geometry => geometry;

        public DistanceLabel(Drawable parent) : base(parent)
        {
            geometry = new SKRect(127, 10, 233, 69);
            distance = 0;
            digits = distance.ToString();
        }

        public uint Distance
        {
            get => distance;
            set
            {
                if (value != distance)
                {
                    distance = value;
                    digits = distance.ToString();
                    if (digits.Length > 5)
                    {
                        digits = "99999";
                    }
                }
            }
        }

        protected override void OnDraw(object sender, DrawEventArgs e)
        {
            var canvas = e.Canvas;

            var txtWidth = (digits.Length+1) * FontWidth + FontKerning * digits.Length;

            geometry = new SKRect(Parent.Geometry.Width/2 - txtWidth/2, 10, Parent.Geometry.Width/2 + txtWidth/2, 10 + FontHeight);

            int count = 0;
            float left;
            foreach (var c in digits)
            {
                var index = c - '0';
                left = geometry.Left + count * FontWidth + FontKerning;
                canvas.DrawImage(Resource.Numbers, FontMap[index], new SKRect(left, geometry.Top, left + FontWidth, geometry.Bottom));
                count++;
            }
            left = geometry.Left + count * FontWidth + FontKerning;
            canvas.DrawImage(Resource.Numbers, FontMap[FontMap.Length-1], new SKRect(left, geometry.Top, left + FontWidth, geometry.Bottom));
        }
    }
}
