﻿using System;
using Aberus.VisualStudio.Debugger.ImageVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
#if VS16 || VS17
using Microsoft.VisualStudio.Utilities;
#endif

// System.Drawing.Bitmap
[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ImageVisualizer),
#if VS16 || VS17
    typeof(ImageVisualizerBitmapObjectSource),
#else
    typeof(VisualizerObjectSource),
#endif
    Target = typeof(System.Drawing.Bitmap), 
    Description = "Image Visualizer")]

// System.Windows.Media.Imaging.BitmapImage, System.Windows.Media.Imaging.BitmapSource
[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ImageVisualizer), 
    typeof(ImageVisualizerObjectSource), 
    Target = typeof(System.Windows.Media.Imaging.BitmapSource), 
    Description = "Image Visualizer")]

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    /// <summary>
    /// A Visualizer for <see cref="System.Windows.Media.Imaging.BitmapSource"/> and <see cref="System.Drawing.Bitmap"/>.
    /// </summary>
    public class ImageVisualizer : DialogDebuggerVisualizer
    {

#if VS17
        public ImageVisualizer() : base(FormatterPolicy.Legacy)
        {

        }
#endif

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException(nameof(windowService), "This debugger does not support modal visualizers.");
            if (objectProvider == null)
                throw new ArgumentNullException(nameof(objectProvider));

#if VS16 || VS17
            using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.SystemAware))
#endif
            using (var imageForm = new ImageForm(objectProvider))
            {
                 windowService.ShowDialog(imageForm);
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
