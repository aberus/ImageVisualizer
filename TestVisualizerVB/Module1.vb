Imports System
Imports System.Diagnostics
Imports System.IO
Imports Microsoft.VisualStudio.DebuggerVisualizers

Module Module1

    Sub Main()

        Dim image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280.png")

        Dim visualizerHost = New VisualizerDevelopmentHost(image1, GetType(ImageVisualizer.Visualizer))
        visualizerHost.ShowVisualizer()
        Dim debuggeeObject = visualizerHost.DebuggeeObject

        Debugger.Break()

        Dim image2 = New System.Windows.Media.Imaging.BitmapImage()
        image2.BeginInit()
        image2.UriSource = New Uri("tumblr_mwiiixNSpW1qbkusho1_1280.png", UriKind.Relative)
        image2.EndInit()

        visualizerHost = New VisualizerDevelopmentHost(image2, GetType(ImageVisualizer.Visualizer), GetType(ImageVisualizer.ImageVisualizerObjectSource))
        visualizerHost.ShowVisualizer()

        Debugger.Break()

        Dim image2small = New System.Windows.Media.Imaging.BitmapImage() 'new Uri("VisualStudio256_256.png", UriKind.Relative));
        image2small.BeginInit()
        image2small.StreamSource = New FileStream("VisualStudio256_256.png", FileMode.Open)
        image2small.EndInit()

        visualizerHost = New VisualizerDevelopmentHost(image2small, GetType(ImageVisualizer.Visualizer), GetType(ImageVisualizer.ImageVisualizerObjectSource))
        visualizerHost.ShowVisualizer()

        Debugger.Break()

    End Sub

End Module
