
using ImageVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: System.Diagnostics.DebuggerVisualizer(typeof(Visualizer), typeof(ImageVisualizerObjectSource), Target = typeof(System.Drawing.Bitmap), Description = "Image Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(Visualizer), typeof(ImageVisualizerObjectSource), Target = typeof(System.Windows.Media.Imaging.BitmapImage), Description = "Image Visualizer")]

namespace ImageVisualizer
{
    public class Visualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService), "This debugger does not support modal visualizers.");
            }

            using (var imageForm = new ImageForm(objectProvider))
            {
                windowService.ShowDialog(imageForm);
            }
        }

#if DEBUG
        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(Visualizer), typeof(ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
#endif
    }
}
