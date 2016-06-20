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
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        public ImageControl()
        {
            InitializeComponent();
            
            //ZoomToFit();
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
                var expression = sourceBitmap.ToString();

                if (sourceBitmap is System.Drawing.Bitmap)
                {
                    BitmapSource bitmap = null;
                    //bitmap.HorizontalResolution, bitmap.VerticalResolution, bitmap.Size, bitmap.PixelFormat, bitmap.RawFormat;


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
            }
        }

        [DllImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        double _zoomValue = 1.0;

        private void DisplayImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var p = Mouse.GetPosition(DisplayImage);
            lastMousePositionOnTarget = p;

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
            //var matrix = DisplayImage.LayoutTransform.Value;
            
            if (e.Delta > 0)
            {
                _zoomValue += 0.1;
                //matrix.ScaleAt(1.1, 1.1, p.X, p.Y);
            }
            else if(e.Delta < 0)
            {
                _zoomValue -= 0.1;
                //matrix.ScaleAt(1.0 / 1.1, 1.0 / 1.1, p.X, p.Y);
            }

            //TODO
            if (_zoomValue <= 0)
                _zoomValue = double.Epsilon;

            var zoom = ZoomToFit();
            if (_zoomValue < zoom)
               _zoomValue = zoom;

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            DisplayImage.LayoutTransform = scale;
           // DisplayImage.LayoutTransform = new MatrixTransform(matrix);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (DisplayScroll.CanContentScroll)
            {
                Console.WriteLine("ScrollChangedEvent just Occurred");
                Console.WriteLine("ExtentHeight is now " + e.ExtentHeight);
                Console.WriteLine("ExtentWidth is now " + e.ExtentWidth);
                Console.WriteLine("ExtentHeightChange was " + e.ExtentHeightChange);
                Console.WriteLine("ExtentWidthChange was " + e.ExtentWidthChange);
                Console.WriteLine("HorizontalOffset is now " + e.HorizontalOffset);
                Console.WriteLine("VerticalOffset is now " + e.VerticalOffset);
                Console.WriteLine("HorizontalChange was " + e.HorizontalChange);
                Console.WriteLine("VerticalChange was " + e.VerticalChange);
                Console.WriteLine("ViewportHeight is now " + e.ViewportHeight);
                Console.WriteLine("ViewportWidth is now " + e.ViewportWidth);
                Console.WriteLine("ViewportHeightChange was " + e.ViewportHeightChange);
                Console.WriteLine("ViewportWidthChange was " + e.ViewportWidthChange);
                Console.WriteLine("ActualHeight is now " + DisplayScroll.ActualHeight);
                Console.WriteLine("ActualWidth is now " + DisplayScroll.ActualWidth);
                Console.WriteLine("--------------------------------------------------------------");
                //ActualHeight = ViewportHeight + HorizontalScrollbarHeight
            }

            DisplayScroll.UpdateLayout();

            if(e.ViewportHeightChange != 0 || e.ViewportWidthChange != 0)
            {
                _zoomValue = ZoomToFit();
                var scale = new ScaleTransform(_zoomValue, _zoomValue);
                DisplayImage.LayoutTransform = scale;

                DisplayScroll.UpdateLayout();
            }

            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (lastMousePositionOnTarget.HasValue)
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(DisplayImage);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX =  e.ExtentWidth / DisplayImage.ActualWidth;
                    double multiplicatorY =  e.ExtentHeight / DisplayImage.ActualHeight;

                    double newOffsetX = DisplayScroll.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = DisplayScroll.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    DisplayScroll.ScrollToHorizontalOffset(newOffsetX);
                    DisplayScroll.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }


        public double ZoomToFit()
        {
            if (DisplayImage.Source != null)
            {
                double width = DisplayScroll.ActualWidth;
                double height = DisplayScroll.ActualHeight;
                double imageWidth = DisplayImage.Source.Width;
                double imageHeight = DisplayImage.Source.Height;
                double zoom;
                double aspectRatio;

                if(imageWidth <= width || imageHeight <= height)
                    return 1;
                
                if (imageWidth > imageHeight)
                {
                    aspectRatio = width / imageWidth;
                    zoom = aspectRatio;

                   if (height < imageHeight * zoom)
                   {
                       aspectRatio = height / imageHeight;
                       zoom = aspectRatio;
                   }
                }
                else
                {
                    aspectRatio = height / imageHeight;
                    zoom = aspectRatio;

                    if (width < imageWidth * zoom)
                    {
                        aspectRatio = width / imageWidth;
                        zoom = aspectRatio;
                    }
                }

                return zoom;               
            }

            return -1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var zoom = ZoomToFit();
            //if (zoom != -1)
            //{
            //    ScaleTransform scale = new ScaleTransform(zoom, zoom);
            //    DisplayImage.LayoutTransform = scale;
            //    this.Zoom = (int)Math.Round(Math.Floor(zoom));
            //}
        }

        private void DisplayScroll_Loaded(object sender, RoutedEventArgs e)
        {
            var zoom = ZoomToFit();
            if (zoom != -1)
            {
                var scale = new ScaleTransform(zoom, zoom);
                DisplayImage.LayoutTransform = scale;
                _zoomValue = zoom; //(int)Math.Round(Math.Floor(zoom));
            }
        }

        private void DisplayScroll_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(DisplayScroll);
                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;
                lastDragPoint = posNow;
                DisplayScroll.ScrollToHorizontalOffset(DisplayScroll.HorizontalOffset - dX);
                DisplayScroll.ScrollToVerticalOffset(DisplayScroll.VerticalOffset - dY);
            }
        }

        private void DisplayScroll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(DisplayScroll);
            if (mousePosition.X <= DisplayScroll.ViewportWidth && mousePosition.Y < DisplayScroll.ViewportHeight)
            {
                DisplayScroll.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePosition;
                Mouse.Capture(DisplayScroll);
            }
        }

        private void DisplayScroll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayScroll.Cursor = Cursors.Arrow;
            DisplayScroll.ReleaseMouseCapture();
            lastDragPoint = null;
        }
    }
}
