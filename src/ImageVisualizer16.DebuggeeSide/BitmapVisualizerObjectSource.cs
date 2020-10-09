using System.Drawing;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace ImageVisualizer
{
    public class BitmapVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, System.IO.Stream outgoingData)
        {
            if (target is Bitmap image && image.GetType().GetCustomAttributes(typeof(ISerializable), false).Length == 0)
                base.GetData(new SerializableBitmap(image), outgoingData);
            else
                base.GetData(target, outgoingData);
        }
    }
}
