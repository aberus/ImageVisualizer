# Debugger Image Visualizer for Visual Studio

Debugger Image Visualizer is a debug visualizer for Visual Studio. It allows you to visually view the graphic content of WPF's images [BitmapImage](https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.bitmapimage.aspx), [BitmapSource](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.bitmapsource), [ImageSource](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imagesource), and WinForms' images [Bitmap](https://msdn.microsoft.com/en-us/library/system.drawing.bitmap.aspx) during debugging. Supports zooming and paning while previewing images.

[Download this extension](https://marketplace.visualstudio.com/items?itemName=AleksanderBerus.DebuggerImageVisualizerPreview) on Visual Studio Marketplace.

This extension works with:
* Visual Studio 2010
* Visual Studio 2012
* Visual Studio 2013
* Visual Studio 2015
* Visual Studio 2017
* Visual Studio 2019

![](https://aleksanderberus.gallerycdn.vsassets.io/extensions/aleksanderberus/debuggerimagevisualizerpreview/0.6.0/1556274284741/219157/1/Preview.gif)

## Changelog

These are the changes to each version that has been released.

### 0.7

* Support for .NET Core 3.x

### 0.6

* Support for VS 2019

### 0.5

* Returned VS2010 support
* Added support of WPF ImageSource / BitmapSource
* Added support for custom ToBitmap() method

### 0.4

* VS Package (VS2010 support removed due to limitation on VSIX format)
* UI fixes
* Fixes for BitmapImage serialization

### 0.3

* Support for VS2017
* Bug fixed (Visualizer only worked in VS2015)

### 0.2

* Support for VS 2013 and 15

### 0.1

* First preview release 

## Contribute

If you have any troubles and/or suggestions please log an issue on the GitHub issue tracker.

## License

The source code is available under [The MIT License (MIT)](LICENSE)
