using Tizen.Applications;
using SkiaSharp;
using System.IO;

namespace buttercat
{
    public static class Resource
    {
        const string bgFileName = "bg.png"; // 360 x 360
        const string catrunFileName = "catrun.png";  // 80 x 68 in 480 x 68

        const string catwalkFileName = "catwalk.png";  // 72 x 60 in 432 x 60

        const string startFileName = "start.png"; // 215 x 100

        const string start2FileName = "start2.png"; // 215 x 100

        const string butterFileName = "butter.png"; // 25 x 12

        const string pipeFileName = "pipe.png"; // 25 x 25

        const string numbersFileName = "numbers.png"; // 46x59 * 11 in 506x59 kerning 16

        static Resource()
        {
            Background = Load(bgFileName);
            CatRuns = Load(catrunFileName);
            CatWalk = Load(catwalkFileName);
            Start = Load(startFileName);
            Start2 = Load(start2FileName);
            Butter = Load(butterFileName);

            var pipeBitmap = LoadBitmap(pipeFileName);
            Pipe = SKImage.FromBitmap(pipeBitmap);
            var pipeReverse = FlipBitmap(pipeBitmap);
            PipeReverse = SKImage.FromBitmap(pipeReverse);
            Numbers = Load(numbersFileName);
        }

        static SKBitmap LoadBitmap(string fileName)
        {
            return SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, fileName));
        }

        static SKBitmap FlipBitmap(SKBitmap bitmap)
        {
            var flipped = new SKBitmap(bitmap.Width, bitmap.Height);
            using (SKCanvas canvas = new SKCanvas(flipped))
            {
                canvas.Clear();
                canvas.Scale(1, -1, 0, bitmap.Width / 2);
                canvas.DrawBitmap(bitmap, new SKPoint());
            }
            return flipped;
        }

        static SKImage Load(string fileName)
        {
            var bitmap = LoadBitmap(fileName);
            return SKImage.FromBitmap(bitmap);
        }

        public static SKImage Background { get; private set; }
        public static SKImage Start { get; private set; }
        public static SKImage Start2 { get; private set; }
        public static SKImage CatRuns { get; private set; }
        public static SKImage CatWalk { get; private set; }
        public static SKImage Butter { get; private set; }
        public static SKImage Pipe { get; private set; }
        public static SKImage PipeReverse { get; private set; }
        public static SKImage Numbers { get; private set; }
    }
}
