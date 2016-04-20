using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    BitmapSource bitmap = null;

                    IntPtr hObject = ((System.Drawing.Bitmap)sourceBitmap).GetHbitmap();

                    try
                    {
                        bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                               hObject,
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
                    }
                    catch(Win32Exception)
                    {
                        bitmap = null;
                    }
                    finally
                    {
                        DeleteObject(hObject);
                    }

                    DisplayImage.Width = bitmap.Width;
                    DisplayImage.Height = bitmap.Height;

                    DisplayImage.Source = bitmap;




                }
                else if (sourceBitmap is SerializableBitmapImage)
                {
                    DisplayImage.Source = (SerializableBitmapImage)sourceBitmap;
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
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        double _zoomValue = 1.0;

        private void DisplayImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            //{
            //    if (e.Delta > 0)
            //    {
            //        _zoomValue += 0.1;
            //    }
            //    else
            //    {
            //        _zoomValue -= 0.1;
            //    }

            //    ScaleTransform scale = new ScaleTransform(_zoomValue, _zoomValue);
            //    DisplayImage.LayoutTransform = scale;

            //}

            //e.Handled = true;

            //if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            //{
                var matrix = DisplayImage.LayoutTransform.Value;

                if (e.Delta > 0)
                {
                    matrix.ScaleAt(1.5, 1.5, e.GetPosition(this).X, e.GetPosition(this).Y);
                }
                else
                {
                    matrix.ScaleAt(1.0 / 1.5, 1.0 / 1.5, e.GetPosition(this).X, e.GetPosition(this).Y);
                }

                DisplayImage.LayoutTransform = new MatrixTransform(matrix); 
            //}
        }

    }
}
