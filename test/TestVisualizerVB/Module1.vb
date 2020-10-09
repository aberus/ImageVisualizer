Module Module1

    Sub Main()

        Dim image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280.png")
        'Debugger.Break()

        Dim image2 = New System.Windows.Media.Imaging.BitmapImage()
        image2.BeginInit()
        image2.UriSource = New Uri("tumblr_mwiiixNSpW1qbkusho1_1280.png", UriKind.Relative)
        image2.EndInit()
        'Debugger.Break()

        Dim image3 = New System.Windows.Media.Imaging.BitmapImage() 'new Uri("VisualStudio256_256.png", UriKind.Relative));
        image3.BeginInit()
        image3.StreamSource = New System.IO.FileStream("VisualStudio256_256.png", System.IO.FileMode.Open)
        image3.EndInit()
        'Debugger.Break()

        Dim pixelFormat = System.Windows.Media.PixelFormats.Bgr32
        Dim width As Integer = 1280
        Dim height As Integer = 720
        Dim rawStride As Integer = (width * pixelFormat.BitsPerPixel + 7) / 8
        Dim rawImage As Byte() = New Byte(rawStride * height - 1) {}
        Dim value As Random = New Random()
        value.NextBytes(rawImage)
        Dim image4 = System.Windows.Media.Imaging.BitmapSource.Create(width, height, 96, 96, pixelFormat, Nothing, rawImage, rawStride)
        Debugger.Break()

    End Sub

End Module
