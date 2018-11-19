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

        static Resource()
        {
            var bgImage = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, bgFileName));
            Background = SKImage.FromBitmap(bgImage);

            var catrunBitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, catrunFileName));
            CatRuns = SKImage.FromBitmap(catrunBitmap);

            var catwalkBitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, catwalkFileName));
            CatWalk = SKImage.FromBitmap(catwalkBitmap);

            var startBitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, startFileName));
            Start = SKImage.FromBitmap(startBitmap);

            var start2Bitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, start2FileName));
            Start2 = SKImage.FromBitmap(start2Bitmap);

            var butterBitmap = SKBitmap.Decode(Path.Combine(Application.Current.DirectoryInfo.Resource, butterFileName));
            Butter = SKImage.FromBitmap(butterBitmap);
        }

        public static SKImage Background { get; private set; }
        public static SKImage Start { get; private set; }
        public static SKImage Start2 { get; private set; }
        public static SKImage CatRuns { get; private set; }
        public static SKImage CatWalk { get; private set; }
        public static SKImage Butter { get; private set; }
    }
}
