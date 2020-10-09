using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    [Serializable]
    public class SerializableBitmap : ISerializable
    {
        private Bitmap image;
        private readonly string expression;
        public Bitmap Image { get; private set; }

        public SerializableBitmap(Bitmap image)
        {
            this.image = image;
        }

        private SerializableBitmap(SerializationInfo info, StreamingContext context)
        {
            //byte[] buffer = (byte[])info.GetValue("Data", typeof(byte[]));
            //try
            //{
            //    this.SetNativeImage(this.InitializeFromStream(new MemoryStream(buffer)));
            //}
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

                            Image = new Bitmap(stream);

                            //using (MemoryStream mStream = new MemoryStream())
                            //{
                            //    mStream.Write(blob, 0, blob.Length);
                            //    mStream.Seek(0, SeekOrigin.Begin);

                            //    Bitmap bm = new Bitmap(mStream);
                            //    return bm;
                            //}

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
                else if (string.Equals(i.Name, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    expression = (string)i.Value;
                }
            }
        }

        public static implicit operator SerializableBitmap(Bitmap bitmapImage)
        {
            return new SerializableBitmap(bitmapImage);
        }

        public static implicit operator Bitmap(SerializableBitmap serializableBitmapImage)
        {
            return serializableBitmapImage.Image;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var source = image;

            if (source != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    source.Save(memoryStream, ImageFormat.Png);
                    info.AddValue("Image", memoryStream.ToArray(), typeof(byte[]));
                }

                info.AddValue("Name", source.ToString());
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
