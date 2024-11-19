using System;
using System.Windows.Forms;
using Aberus.VisualStudio.Debugger.ImageVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
#if VS16 || VS17 || VS17_6
using Microsoft.VisualStudio.Utilities;
#endif

// System.Drawing.Bitmap
[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ImageVisualizer),
#if VS17_6       
    typeof(ImageVisualizerJsonObjectSource),
#elif VS16 || VS17
    typeof(ImageVisualizerBitmapObjectSource),
#else
    typeof(VisualizerObjectSource),
#endif
    Target = typeof(System.Drawing.Bitmap), 
    Description = "Image Visualizer")]

// System.Windows.Media.Imaging.BitmapImage, System.Windows.Media.Imaging.BitmapSource
[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ImageVisualizer),
#if VS17_6       
    typeof(ImageVisualizerJsonObjectSource),
#else
    typeof(ImageVisualizerObjectSource), 
#endif
    Target = typeof(System.Windows.Media.Imaging.BitmapSource), 
    Description = "Image Visualizer")]

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    /// <summary>
    /// A Visualizer for <see cref="System.Windows.Media.Imaging.BitmapSource"/> and <see cref="System.Drawing.Bitmap"/>.
    /// </summary>
    public class ImageVisualizer : DialogDebuggerVisualizer
    {

#if VS17_6
        public ImageVisualizer() : base(FormatterPolicy.Json)
        {

        }
#endif

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException(nameof(windowService), "This debugger does not support modal visualizers.");
            if (objectProvider == null)
                throw new ArgumentNullException(nameof(objectProvider));

            if (windowService is IWin32Window win32Window)
            {

#if VS17_6
                var debugObject = (objectProvider as IVisualizerObjectProvider3).GetObject<ImageVisualizerImage>();
#else              
                var debugObject = objectProvider.GetObject();
#endif


#if VS16 || VS17 || VS17_6
                using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.PerMonitorAwareV2))
#endif
                using (var imageForm = new ImageForm(debugObject))
                {
                     windowService.ShowDialog(imageForm);
                }
            }
        }

#if DEBUG
        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(ImageVisualizer), typeof(ImageVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
#endif
    }
}
