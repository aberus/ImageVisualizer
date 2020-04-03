using System;
using System.Diagnostics;
using System.IO;

namespace TestVisualizer
{
    class Program
    {
        [STAThread]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
        static void Main()
        {
            var image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280.png");
            //Debugger.Break();

            var image2 = new System.Windows.Media.Imaging.BitmapImage();

            image2.BeginInit();
            image2.UriSource = new Uri("tumblr_mwiiixNSpW1qbkusho1_1280.png", UriKind.Relative);
            image2.EndInit();
            //Debugger.Break();

            var image3 = new System.Windows.Media.Imaging.BitmapImage();

            image3.BeginInit();
            image3.StreamSource = new FileStream("VisualStudio256_256.png", FileMode.Open);
            image3.EndInit();
            //Debugger.Break();

            var pixelFormat = System.Windows.Media.PixelFormats.Bgr32;
            int width = 1280;
            int height = 720;
            int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];
            var value = new Random();
            value.NextBytes(rawImage);

            var image4 = System.Windows.Media.Imaging.BitmapSource.Create(width, height, 96, 96, pixelFormat, null, rawImage, rawStride);
            Debugger.Break();
        }
    }
}
