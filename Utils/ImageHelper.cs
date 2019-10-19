using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace M.Core.Utils
{
    public class ImageHelper
    {
        /// <summary>
        /// EncodeBase64 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string EncodeBase64(string source, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            byte[] bytes = encode.GetBytes(source);
            string encodedStr = "";

            try
            {
                encodedStr = Convert.ToBase64String(bytes);
            }
            catch
            {
                encodedStr = source;
            }
            return encodedStr;
        }
        /// <summary>
        /// 将图片数据转换为Base64字符串
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private string ImageToBase64(Image img)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            binFormatter.Serialize(memStream, img);
            byte[] bytes = memStream.GetBuffer();
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        private Image Base64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream memStream = new MemoryStream(bytes);
            BinaryFormatter binFormatter = new BinaryFormatter();
            return (Image)binFormatter.Deserialize(memStream);
        }


        /// <summary>
        /// 将二进制转换为图片
        /// </summary>
        /// <param name="oData">二进制</param>
        /// <returns></returns>
        public static Image ByteToImage(object oData)
        {
            try
            {
                MemoryStream ms = new MemoryStream((byte[])oData);
                if (ms.Length > 0)
                {
                    return Image.FromStream(ms, true);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 将
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Byte[] BitmapToByte(Bitmap bitmap)
        {
            MemoryStream ms = null;
            try
            {
                using (ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
                    ms.Close();
                    return bytes;
                }
            }
            catch
            {
                ms.Close();
                return null;
            }
            finally
            {
                ms.Close();
            }
        }
        /// <summary>
        /// 将二进制转换为图片
        /// </summary>
        /// <param name="oData">二进制</param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image BytesToImage(object oData, Image img = null)
        {
            try
            {
                if (Convert.IsDBNull(oData) == true)
                {
                    return img;
                }
                else
                {
                    MemoryStream ms = new MemoryStream((byte[])oData);
                    if (ms.Length > 0)
                    {
                        return Image.FromStream(ms, true);
                    }
                    else
                    {
                        return img;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// byte[] 转换 Bitmap  
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap(new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Bitmap转byte[]  
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                bitmap.Save(ms, bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }



        /// <summary>
        /// 返回图片
        /// </summary>
        /// <param name="imagePath">完整路径</param>
        /// <returns></returns>
        public static Image FileToImage(string imagePath)
        {
            try
            {
                FileStream files = new FileStream(imagePath, FileMode.OpenOrCreate, FileAccess.Read);
                byte[] imgByte = new byte[files.Length];
                files.Read(imgByte, 0, imgByte.Length);
                files.Close();
                if (Convert.IsDBNull(imgByte) == true)
                {
                    return null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(imgByte);
                    if (ms.Length > 0)
                    {
                        return Image.FromStream(ms, true);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }


        /// <summary>
        /// 改变图片透明度
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Image ChangeAlpha(Image image)
        {
            Bitmap img = new Bitmap(image);
            using (Bitmap bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(img, 0, 0);
                    for (int h = 0; h <= img.Height - 1; h++)
                    {
                        for (int w = 0; w <= img.Width - 1; w++)
                        {
                            Color c = img.GetPixel(w, h);
                            bmp.SetPixel(w, h, Color.FromArgb(200, c.R, c.G, c.B));
                        }
                    }
                    return (Image)bmp.Clone();
                }
            }
        }


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgSource"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Bitmap ScalePicture(Image imgSource, float scale)
        {
            if (imgSource == null)
            {
                return null;
            }

            int newWidth = Convert.ToInt32(imgSource.Width * scale);
            int newHeight = Convert.ToInt32(imgSource.Height * scale);
            return ScalePicture(imgSource, newWidth, newHeight);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static byte[] ScalePicture(byte[] bytes, float scale)
        {
            if (bytes == null)
            {
                return null;
            }

            Image imgSource = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Write(bytes, 0, bytes.Length);
                imgSource = Image.FromStream(ms, true);

            }
            if (imgSource == null)
            {
                return null;
            }

            int newWidth = Convert.ToInt32(imgSource.Width * scale);
            int newHeight = Convert.ToInt32(imgSource.Height * scale);
            return ScalePicture(bytes, newWidth, newHeight);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="img"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Bitmap ScalePicture(Image img, int newWidth, int newHeight)
        {
            if (img == null)
            {
                return null;
            }

            System.Drawing.Image imgSource = img;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            Bitmap outBmp = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量 
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((newWidth - sW) / 2, (newHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量???? 
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static byte[] ScalePicture(byte[] bytes, int newWidth, int newHeight)
        {
            Image img = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Write(bytes, 0, bytes.Length);
                img = Image.FromStream(ms, true);

            }
            if (img == null)
            {
                return null;
            }

            System.Drawing.Image imgSource = img;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            Bitmap outBmp = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量 
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((newWidth - sW) / 2, (newHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量???? 
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();

            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                outBmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
            }
            return data;
        }
    }
}
