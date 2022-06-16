using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace Aberus.VisualStudio.Debugger.ImageVisualizer
{
    public partial class ImageForm : Form
    {
        [DllImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        public static Font UIFont
        {
            get
            {
#if VS10
                string dteProgID = "VisualStudio.DTE.10.0";
#elif VS11
                string dteProgID = "VisualStudio.DTE.11.0";
#elif VS12
                string dteProgID = "VisualStudio.DTE.12.0";
#elif VS13
                string dteProgID = "VisualStudio.DTE.13.0";
#elif VS14
                string dteProgID = "VisualStudio.DTE.14.0";
#elif VS15
                string dteProgID = "VisualStudio.DTE.15.0";
#elif VS16
                string dteProgID = "VisualStudio.DTE.16.0";
#elif VS17
                string dteProgID = "VisualStudio.DTE.17.0";
#endif
                var dte = (EnvDTE.DTE)Marshal.GetActiveObject(dteProgID);
                var fontProperty = dte.Properties["FontsAndColors", "Dialogs and Tool Windows"];
                if (fontProperty != null)
                {
                    object objValue = fontProperty.Item("FontFamily").Value;
                    string fontFamily = objValue.ToString();
                    objValue = fontProperty.Item("FontSize").Value;
                    float fontSize = Convert.ToSingle(objValue);
                    var font = new Font(fontFamily, fontSize);

                    return font;
                }

                return System.Drawing.SystemFonts.DefaultFont;
            }
        }

        public ImageForm(IVisualizerObjectProvider objectProvider)
        {
            InitializeComponent();

#if DEBUG
            this.ShowInTaskbar = true;
#endif

            this.label1.Font = UIFont;
            SetFontAndScale(this.label1, UIFont);
            this.label2.Font = UIFont;
            this.txtExpression.Font = UIFont;
            this.btnClose.Font = UIFont;

            object objectBitmap = objectProvider.GetObject();
            if (objectBitmap != null)
            {
#if DEBUG
                string expression = objectBitmap.ToString();
#endif

                var method = objectBitmap.GetType().GetMethod("ToBitmap", new Type[] { });
                if (method != null)
                {
                    objectBitmap = method.Invoke(objectBitmap, null);
                }

                BitmapSource bitmapSource = null;

                if (objectBitmap is Bitmap)
                {
                    var hObject = ((Bitmap)objectBitmap).GetHbitmap();
                    
                    try
                    {
                        bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                               hObject,
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
                    }
                    catch (Win32Exception)
                    {
                        bitmapSource = null;
                    }
                    finally
                    {
                        DeleteObject(hObject);
                    }
                }
                else if (objectBitmap is SerializableBitmapImage serializableBitmapImage)
                {
                    bitmapSource = serializableBitmapImage;
                }

                if (bitmapSource != null)
                {
                    imageControl.SetImage(bitmapSource);
                }
            }
     
            txtExpression.Text = objectProvider.GetObject().ToString();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SetFontAndScale(Control ctlControl, Font objFont)
        {
            float sngRatio = objFont.Size / ctlControl.Font.Size;
            if (ctlControl is Form form)
            {
                form.AutoScaleMode = AutoScaleMode.None;
            }
            ctlControl.Font = objFont;
            ctlControl.Scale(new SizeF(sngRatio, sngRatio));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
