using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace ImageVisualizer
{
    public class ImageVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target is BitmapImage)
                base.GetData(new SerializableBitmapImage((BitmapImage)target), outgoingData);
            else
                base.GetData(target, outgoingData);
        }
    }
}
