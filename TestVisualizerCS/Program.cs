using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var image2small = new System.Windows.Media.Imaging.BitmapImage(); //new Uri("VisualStudio256_256.png", UriKind.Relative));
            image2small.BeginInit();
            image2small.StreamSource = new FileStream("VisualStudio256_256.png", FileMode.Open);
            image2small.EndInit();

            visualizerHost = new VisualizerDevelopmentHost(image2small, typeof(ImageVisualizer.Visualizer), typeof(ImageVisualizer.ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();

            Debugger.Break();
        }
    }
}
