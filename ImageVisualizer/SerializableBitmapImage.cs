using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageVisualizer
{
    [Serializable]
    public class SerializableBitmapImage : ISerializable
    {
        private BitmapImage image;

        public BitmapImage Image { get { return image; } private set { image = value; } }

        internal SerializableBitmapImage(BitmapImage image)
        {
            this.image = image;
        }

        protected SerializableBitmapImage(SerializationInfo info, StreamingContext context)
        {
            foreach (var i in info)
            {
                if (string.Equals(i.Name, "Image", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var array = (byte[])i.Value;
                        if (array != null)
                        {
                            using (MemoryStream stream = new MemoryStream(array))
                            {
                                stream.Position = 0;
                                image = new BitmapImage();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.BeginInit();
                                image.StreamSource = new MemoryStream(array);
                                image.EndInit();
                                image.Freeze();
                            }
                        }
                    }
                    catch
                    {
                    }
                    //catch (ExternalException)
                    //{
                    //}
                    //catch (ArgumentException)
                    //{
                    //}
                    //catch (OutOfMemoryException)
                    //{
                    //}
                    //catch (InvalidOperationException)
                    //{
                    //}
                    //catch (NotImplementedException)
                    //{
                    //}
                    //catch (FileNotFoundException)
                    //{
                    //}
                }
            }
        }

        public static implicit operator SerializableBitmapImage(BitmapImage bitmapImage)
        {
            return new SerializableBitmapImage(bitmapImage);
        }

        public static implicit operator BitmapImage(SerializableBitmapImage serializableBitmapImage)
        {
            return serializableBitmapImage.Image;
        }

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memoryStream);

                info.AddValue("Image", memoryStream.ToArray(), typeof(byte[]));
            }
        }
    }
}
