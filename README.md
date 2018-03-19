# Debugger Image Visualizer for Visual Studio

Debugger Image Visualizer is a debug visualizer for Visual Studio. It allows you to visually view the graphic content of WPF's images [System.Windows.Media.Imaging.BitmapImage](https://msdn.microsoft.com/en-us/library/system.windows.media.imaging.bitmapimage.aspx) and WinForms' images [System.Drawing.Bitmap](https://msdn.microsoft.com/en-us/library/system.drawing.bitmap.aspx) during debugging. Supports zooming and paning while previewing images.

[Download this extension](https://marketplace.visualstudio.com/items?itemName=AleksanderBerus.DebuggerImageVisualizerPreview) on Visual Studio Marketplace.

This extension works with:
* <s>Visual Studio 2010</s>
* Visual Studio 2012
* Visual Studio 2013
* Visual Studio 2015
* Visual Studio 2017

![](https://i1.visualstudiogallery.msdn.s-msft.com/1a6045f1-1bb9-4f45-adde-b004cc657a9c/image/file/217228/1/preview.gif)

## Changelog

These are the changes to each version that has been released.

### 0.4

* VSIX Installer (VS2010 support removed due to limitation on VSIX format)
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
