using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    public class ImageVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target is ImageSource image)
                base.GetData(new SerializableBitmapImage(image), outgoingData);
            else
                base.GetData(target, outgoingData);
        }
    }
}
