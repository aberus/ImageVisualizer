using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    /// <summary>
    /// Interaction logic for ImageControl.xaml
    /// </summary>
    public partial class ImageControl : UserControl
    {
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        double _zoomValue = 1.0;

        public ImageControl()
        {
            InitializeComponent();
        }

        public void SetImage(ImageSource imageSource)
        {
            if (imageSource != null)
            {
                //DisplayImage.Width = bitmap.Width;
                //DisplayImage.Height = bitmap.Height;
                //DisplayImage.Source = bitmap;

                DisplayImage.Source = imageSource;
            }
        }

        private double ZoomToFit()
        {
            if (DisplayImage.Source != null)
            {
                double width = DisplayScroll.ActualWidth;
                double height = DisplayScroll.ActualHeight;
                double imageWidth = DisplayImage.Source.Width;
                double imageHeight = DisplayImage.Source.Height;
                double zoom;
                double aspectRatio;

                if (imageWidth <= width || imageHeight <= height)
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

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
#if DEBUG
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
#endif

            DisplayScroll.UpdateLayout();

            if (e.ViewportHeightChange != 0 || e.ViewportWidthChange != 0)
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

                    double multiplicatorX = e.ExtentWidth / DisplayImage.ActualWidth;
                    double multiplicatorY = e.ExtentHeight / DisplayImage.ActualHeight;

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

        private void DisplayScroll_Loaded(object sender, RoutedEventArgs e)
        {
            double zoom = ZoomToFit();
            if (zoom != -1)
            {
                var scale = new ScaleTransform(zoom, zoom);
                DisplayImage.LayoutTransform = scale;
                _zoomValue = zoom;
            }
        }

        private void DisplayImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePosition = Mouse.GetPosition(DisplayImage);
            lastMousePositionOnTarget = mousePosition;

            if (e.Delta > 0)
            {
                _zoomValue += 0.1;
            }
            else if (e.Delta < 0)
            {
                _zoomValue -= 0.1;
            }

            if (_zoomValue <= 0)
                _zoomValue = double.Epsilon;

            double zoom = ZoomToFit();
            if (_zoomValue < zoom)
                _zoomValue = zoom;

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            DisplayImage.LayoutTransform = scale;
        }

        private void DisplayScroll_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint is Point lastPosition)
            {
                var mousePosition = e.GetPosition(DisplayScroll);
                double dX = mousePosition.X - lastPosition.X;
                double dY = mousePosition.Y - lastPosition.Y;
                lastDragPoint = mousePosition;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _zoomValue += 0.1;

            if (_zoomValue <= 0)
                _zoomValue = double.Epsilon;

            double zoom = ZoomToFit();
            if (_zoomValue < zoom)
                _zoomValue = zoom;

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            DisplayImage.LayoutTransform = scale;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double zoom = ZoomToFit();
            if (zoom != -1)
            {
                var scale = new ScaleTransform(zoom, zoom);
                DisplayImage.LayoutTransform = scale;
                _zoomValue = zoom;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _zoomValue -= 0.1;

            if (_zoomValue <= 0)
                _zoomValue = double.Epsilon;

            double zoom = ZoomToFit();
            if (_zoomValue < zoom)
                _zoomValue = zoom;

            var scale = new ScaleTransform(_zoomValue, _zoomValue);
            DisplayImage.LayoutTransform = scale;
        }
    }
}
