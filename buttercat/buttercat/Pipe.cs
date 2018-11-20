using SkiaSharp;

namespace buttercat
{
    public class Pipe : Drawable
    {
        bool reverse;

        SKRect geometry;
        public override SKRect Geometry => geometry;

        static SKRect reverseStartGeometry = new SKRect(-25, 360 - 25, 0, 360);
        static SKRect startGeometry = new SKRect(360, 360 - 25, 360 + 25, 360);

        public Pipe(int height, bool reverse = false)
        {
            geometry = reverse ? reverseStartGeometry : startGeometry;
            geometry.Top -= height;
            this.reverse = reverse;
        }

        public void Move(int x)
        {
            geometry.Offset(reverse ? -x : x, 0);
        }

        protected override void OnDraw(object sender, DrawEventArgs args)
        {
            var canvas = args.Canvas;
            var lattice = new SKLattice
            {
                XDivs = new int[] { 4, 17},
                YDivs = new int[] { 17 },
                Flags = new SKLatticeFlags[6]
            };
            if (reverse)
            {
                canvas.Save();
                canvas.RotateDegrees(180, 180, 180);
                canvas.DrawImageLattice(Resource.Pipe, lattice, Geometry);
                canvas.Restore();
            }
            else
            {
                canvas.DrawImageLattice(Resource.Pipe, lattice, Geometry);
            }
        }
    }
}
