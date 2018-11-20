using Tizen.Applications;
using SkiaSharp;
using System.IO;
using System;

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

        static Resource()
        {
            Background = Load(bgFileName);
            CatRuns = Load(catrunFileName);
            CatWalk = Load(catwalkFileName);
            Start = Load(startFileName);
            Start2 = Load(start2FileName);
            Butter = Load(butterFileName);
            Pipe = Load(pipeFileName);
        }

        static SKImage Load(string fileName, bool flip = false)
        {
            var bitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, fileName));
            if (flip)
            {
                
            }
            return SKImage.FromBitmap(bitmap);
        }

        public static SKImage Background { get; private set; }
        public static SKImage Start { get; private set; }
        public static SKImage Start2 { get; private set; }
        public static SKImage CatRuns { get; private set; }
        public static SKImage CatWalk { get; private set; }
        public static SKImage Butter { get; private set; }
        public static SKImage Pipe { get; private set; }
    }
}
