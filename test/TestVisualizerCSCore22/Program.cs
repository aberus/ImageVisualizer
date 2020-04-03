using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TestVisualizer
{
    class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
        static void Main(string[] args)
        {
            var image1 = System.Drawing.Image.FromFile("tumblr_mwiiixNSpW1qbkusho1_1280_1.png");


            var aa = image1.GetType().GetCustomAttributes(typeof(ISerializable), false);
            var bb = image1.GetType().GetCustomAttributes(typeof(ISerializable), true);

            if (typeof(ISerializable).IsAssignableFrom(image1.GetType()))
            { 
            }

            bool i = image1 is ISerializable;


            Debugger.Break();
        }
    }
}
