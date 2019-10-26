using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace M.Core.Utils
{
    /// <summary>
    /// 说明的PrintHelper。
    /// </summary>
    public class ImagePrintHelper
    {
        private readonly Image image;
        private readonly PrintDocument printDocument = new PrintDocument();
        private readonly PrintDialog printDialog = new PrintDialog();
        //private WK_Framework.Forms.CoolPrintPreviewDialog previewDialog = new WK_Framework.Forms.CoolPrintPreviewDialog();

        #region 配置参数
        /// <summary>
        /// Align printouts centered on the page.
        /// </summary>
        public bool AllowPrintCenter = true;

        /// <summary>
        /// rotate the image if it fits the page better
        /// </summary>
        public bool AllowPrintRotate = true;
        /// <summary>
        /// scale the image to fit the page better
        /// </summary>
        public bool AllowPrintEnlarge = true;
        /// <summary>
        /// scale the image to fit the page better
        /// </summary>
        public bool AllowPrintShrink = true;
        #endregion

        public ImagePrintHelper(Image image)
            : this(image, "test.png")
        {
        }

        public ImagePrintHelper(Image image, string documentname)
        {
            this.image = (Image)image.Clone();
            printDialog.UseEXDialog = true;
            printDocument.DocumentName = documentname;
            printDocument.PrintPage += GetImageForPrint;
            printDialog.Document = printDocument;
        }

        public PrinterSettings PrintWithDialog()
        {
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
                return printDialog.PrinterSettings;
            }
            else
            {
                return null;
            }
        }

        private void GetImageForPrint(object sender, PrintPageEventArgs e)
        {
            //PrintOptionsDialog pod = new PrintOptionsDialog();
            //pod.ShowDialog();

            ContentAlignment alignment = AllowPrintCenter ? ContentAlignment.MiddleCenter : ContentAlignment.TopLeft;

            RectangleF pageRect = e.PageSettings.PrintableArea;
            GraphicsUnit gu = GraphicsUnit.Pixel;
            RectangleF imageRect = image.GetBounds(ref gu);
            // rotate the image if it fits the page better
            if (AllowPrintRotate)
            {
                if ((pageRect.Width > pageRect.Height && imageRect.Width < imageRect.Height) ||
                   (pageRect.Width < pageRect.Height && imageRect.Width > imageRect.Height))
                {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    imageRect = image.GetBounds(ref gu);
                    if (alignment.Equals(ContentAlignment.TopLeft))
                    {
                        alignment = ContentAlignment.TopRight;
                    }
                }
            }
            RectangleF printRect = new RectangleF(0, 0, imageRect.Width, imageRect.Height); ;
            // scale the image to fit the page better
            if (AllowPrintEnlarge || AllowPrintShrink)
            {
                SizeF resizedRect = ScaleHelper.GetScaledSize(imageRect.Size, pageRect.Size, false);
                if ((AllowPrintShrink && resizedRect.Width < printRect.Width) ||
                   AllowPrintEnlarge && resizedRect.Width > printRect.Width)
                {
                    printRect.Size = resizedRect;
                }
            }

            // align the image
            printRect = ScaleHelper.GetAlignedRectangle(printRect, new RectangleF(0, 0, pageRect.Width, pageRect.Height), alignment);
            e.Graphics.DrawImage(image, printRect, imageRect, GraphicsUnit.Pixel);
        }
    }
}
