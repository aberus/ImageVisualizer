using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageVisualizer
{
    /// <summary>
    /// Interaction logic for ImageControl.xaml
    /// </summary>
    public partial class ImageControl : UserControl
    {
        public ImageControl()
        {
            InitializeComponent();
        }

        //public ImageControl(System.Drawing.Bitmap sourceBitmap) : this()
        //{
        //    image.Source =
        //}

        //public void SetImage(ImageSource imageSource)
        //{
        //    image.Source = imageSource;
        //}

        public void SetImage(object sourceBitmap)//(System.IO.Stream imageStream)
        {

            if (sourceBitmap != null)
            {
                

                if (sourceBitmap is System.Drawing.Bitmap)
                {
                    IntPtr hObject = ((System.Drawing.Bitmap)sourceBitmap).GetHbitmap();

                    var bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                           hObject,
                           IntPtr.Zero,
                           System.Windows.Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

                    DeleteObject(hObject);

                    image.Source = bitmap;
                }
                else if (sourceBitmap is SerializableBitmapImage)
                {
                    image.Source = (SerializableBitmapImage)sourceBitmap;
                }


                //var selector = new SurrogateSelector();
                //var imageSurrogate = new BitmapImageCloneSurrogate();
                //imageSurrogate.Register(selector);

                //var bitmap = (BitmapSource)GenericCopier.Formater().Deserialize(imageStream);


                // BinaryFormatter binaryFormatter = new BinaryFormatter(selector, new StreamingContext(StreamingContextStates.Clone));
                //var bitmap = (BitmapSource)binaryFormatter.Deserialize(imageStream);
                //var bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.StreamSource = imageStream;
                //bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //bitmap.EndInit();
                //bitmap.Freeze();
            }

        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr hObject);
    }
}
