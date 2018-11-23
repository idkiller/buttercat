using SkiaSharp;

namespace buttercat
{
    public class Pipe : Drawable
    {
        bool reverse;

        SKRect geometry;
        public override SKRect Geometry => geometry;

        static SKRect reverseStartGeometry = new SKRect(360, 0, 360 + 25, 25);
        static SKRect startGeometry = new SKRect(360, 360 - 25, 360 + 25, 360);

        public Pipe(Drawable parent, int height, bool reverse = false): base(parent)
        {
            geometry = reverse ? reverseStartGeometry : startGeometry;
            this.reverse = reverse;
            if (reverse)
            {
                geometry.Bottom += height;
            }
            else
            {
                geometry.Top -= height;
            }
        }

        public void Move(int x)
        {
            geometry.Offset(x, 0);
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;
            if (reverse)
            {
                var lattice = new SKLattice
                {
                    XDivs = new int[] { 4, 17},
                    YDivs = new int[] { 0, 7 },
                    Flags = new SKLatticeFlags[12]
                };
                canvas.DrawImageLattice(Resource.PipeReverse, lattice, Geometry);
            }
            else
            {
                var lattice = new SKLattice
                {
                    XDivs = new int[] { 4, 17},
                    YDivs = new int[] { 17 },
                    Flags = new SKLatticeFlags[6]
                };
                canvas.DrawImageLattice(Resource.Pipe, lattice, Geometry);
            }
        }
    }
}
