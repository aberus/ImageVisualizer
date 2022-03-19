#if NETCOREAPP3_0_OR_GREATER
using System.Drawing;
#endif
using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    public class ImageVisualizerBitmapObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
#if NETCOREAPP3_0_OR_GREATER
            if (target is Bitmap bitmap && !bitmap.GetType().IsSerializable)
                base.GetData(new SerializableBitmap(bitmap), outgoingData);

            else
#endif
                base.GetData(target, outgoingData);
        }
    }
}
