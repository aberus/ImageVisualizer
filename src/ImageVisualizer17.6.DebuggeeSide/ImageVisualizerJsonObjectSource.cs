using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    public class ImageVisualizerJsonObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target is Bitmap bitmap)
            {
                var result = ConvertBitmap(bitmap);
                SerializeAsJson(outgoingData, result);
            }
            else if (target is BitmapSource bitmapSource)
            {
                var result = ConvertBitmapSource(bitmapSource);
                SerializeAsJson(outgoingData, result);
            }
        }

        private object ConvertBitmap(Bitmap bitmap)
        {
            var imageVisualizerImage = new ImageVisualizerImage
            {
                Name = bitmap.ToString()
            };

            if (bitmap != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                    imageVisualizerImage.Image = memoryStream.ToArray();
                }
            }
            return imageVisualizerImage;
        }

        private ImageVisualizerImage ConvertBitmapSource(BitmapSource bitmapSource)
        {

            var imageVisualizerImage = new ImageVisualizerImage
            {
                Name = bitmapSource.ToString()
            };

            if (bitmapSource != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    //TODO try/catch
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    imageVisualizerImage.Image =memoryStream.ToArray();
                }
            }
            return imageVisualizerImage;
        }
    }
}