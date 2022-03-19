using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    [Serializable]
    public class SerializableBitmap : ISerializable
    {
        private readonly Bitmap bitmap;
        private readonly string expression;

        public SerializableBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        protected SerializableBitmap(SerializationInfo info, StreamingContext context)
        {
            foreach (var i in info)
            {
                if (string.Equals(i.Name, "Bitmap", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        if (i.Value is byte[] array)
                        {
                            var stream = new MemoryStream(array);
                            stream.Seek(0, SeekOrigin.Begin);

                            bitmap = new Bitmap(stream);

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

        public static implicit operator SerializableBitmap(Bitmap bitmap)
        {
            return new SerializableBitmap(bitmap);
        }

        public static implicit operator Bitmap(SerializableBitmap serializableBitmap)
        {
            return serializableBitmap.bitmap;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var source = bitmap;

            if (source != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                    //TODO try/catch
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    info.AddValue("Bitmap", memoryStream.ToArray(), typeof(byte[]));
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