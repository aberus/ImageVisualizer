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

            if (target != null)
            {
                if (target is BitmapImage)
                {
                    base.GetData(new SerializableBitmapImage((BitmapImage)target), outgoingData);

                    //var target1 = GenericCopier.DeepCopy((BitmapSource)target);
                    //var selector = new SurrogateSelector();
                    //var imageSurrogate = new BitmapImageCloneSurrogate();
                    //imageSurrogate.Register(selector);
                    //GenericCopier.Formater().

                    //BinaryFormatter binaryFormatter = new BinaryFormatter(selector, new StreamingContext(StreamingContextStates.Clone));
                    //binaryFormatter.Serialize(outgoingData, target);
                    //GenericCopier.Formater().Serialize(outgoingData, target);

                }
                                   
                //var encoder = new PngBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create((BitmapSource)target));
                //encoder.Save(outgoingData);
            }   
            else
                base.GetData(target, outgoingData);
        }
    }
}
