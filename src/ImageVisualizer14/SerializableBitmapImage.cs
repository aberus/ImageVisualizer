using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    [Serializable]
    public class SerializableBitmapImage : ISerializable
    {
        public BitmapSource bitmapSource;
        private readonly string expression;

        public SerializableBitmapImage(BitmapImage image)
        {
            bitmapSource = image;
        }

        public SerializableBitmapImage(BitmapSource source)
        {
            bitmapSource = source;
        }

        protected SerializableBitmapImage(SerializationInfo info, StreamingContext context)
        {
            foreach (var i in info)
            {
                if (string.Equals(i.Name, "Image", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        if (i.Value is byte[] array)
                        {
                            var stream = new MemoryStream(array);
                            stream.Seek(0, SeekOrigin.Begin);

                            var bitmapImage = new BitmapImage();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();

                            bitmapSource = bitmapImage;
                        }
                    }
                    catch (ExternalException)
                    {
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (OutOfMemoryException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (NotImplementedException)
                    {
                    }
                    catch (FileNotFoundException)
                    {
                    }
                }
                else if(string.Equals(i.Name, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    expression = (string)i.Value;
                }
            }
        }

        public static implicit operator SerializableBitmapImage(BitmapImage bitmapImage)
        {
            return new SerializableBitmapImage(bitmapImage);
        }

        public static implicit operator BitmapImage(SerializableBitmapImage serializableBitmapImage)
        {
            return (BitmapImage)serializableBitmapImage.bitmapSource;
        }

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (bitmapSource != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    //TODO try/catch
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource)); 
                    encoder.Save(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    info.AddValue("Image", memoryStream.ToArray(), typeof(byte[]));
                }

                info.AddValue("Name", bitmapSource.ToString());
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(expression))
                return expression;

            return base.ToString();
        }
    }
}
