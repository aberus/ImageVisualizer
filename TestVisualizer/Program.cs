using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TestVisualizer
{

    class Program
    {
        [STAThread]
        static void Main()
        {
            //var image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280.png");

            //var visualizerHost = new VisualizerDevelopmentHost(image1, typeof(ImageVisualizer.Visualizer));
            //visualizerHost.ShowVisualizer();

            //Debugger.Break();

            var image2 = new BitmapImage();
            image2.BeginInit();
            image2.UriSource = new Uri("tumblr_mwiiixNSpW1qbkusho1_1280.png", UriKind.Relative);
            image2.EndInit();

            var visualizerHost = new VisualizerDevelopmentHost(image2, typeof(ImageVisualizer.Visualizer), typeof(ImageVisualizer.ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
