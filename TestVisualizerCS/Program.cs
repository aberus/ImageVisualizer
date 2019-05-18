using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace TestVisualizer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280.png");

            var visualizerHost = new VisualizerDevelopmentHost(image1, typeof(ImageVisualizer.Visualizer));
            visualizerHost.ShowVisualizer();
            var debuggeeObject = visualizerHost.DebuggeeObject;

            Debugger.Break();

            var image2 = new System.Windows.Media.Imaging.BitmapImage();
            image2.BeginInit();
            image2.UriSource = new Uri("tumblr_mwiiixNSpW1qbkusho1_1280.png", UriKind.Relative);
            image2.EndInit();

            visualizerHost = new VisualizerDevelopmentHost(image2, typeof(ImageVisualizer.Visualizer), typeof(ImageVisualizer.ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();

            Debugger.Break();

            var image3 = new System.Windows.Media.Imaging.BitmapImage(); //new Uri("VisualStudio256_256.png", UriKind.Relative));
            image3.BeginInit();
            image3.StreamSource = new FileStream("VisualStudio256_256.png", FileMode.Open);
            image3.EndInit();

            visualizerHost = new VisualizerDevelopmentHost(image3, typeof(ImageVisualizer.Visualizer), typeof(ImageVisualizer.ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();

            Debugger.Break();

            var pixelFormat = PixelFormats.Bgr32;
            int width = 1280;
            int height = 720;
            int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];

            Random value = new Random();
            value.NextBytes(rawImage);

            var image4 = System.Windows.Media.Imaging.BitmapSource.Create(width, height, 96, 96, pixelFormat, null, rawImage, rawStride);
            visualizerHost = new VisualizerDevelopmentHost(image4, typeof(ImageVisualizer.Visualizer), typeof(ImageVisualizer.ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();

            Debugger.Break();
        }
    }
}
