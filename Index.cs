using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Clear
{

    #region enumie
    public enum ImageSizePref
    {
        None,
        Width,
        Height
    }
    public enum Sharers
    {
        Facebook,
        Twitter,
        Pinterest,
        Google
    }
    #endregion

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".

    public static class Tools
    {
        /// <summary>
        /// Write any given text to a text file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        private static void WriteToFile(string filename, string text)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter(filename);

            //Write a line of text
            sw.WriteLine(text);

            //Close the file
            sw.Close();
        }

        /// <summary>
        /// Read the content of any given file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string ReadFile(string filename)
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(filename);

            //Read the first line of text
            string line = sr.ReadLine();

            ////Continue to read until you reach end of file
            //while (line != null)
            //{
            //    //Read the next line
            //    line = sr.ReadLine();
            //}

            //close the file
            sr.Close();

            return line;
        }

        public static long FromBaseX(string IBaseX, int IBase)
        {
            IBaseX = IBaseX.ToUpper();
            //string[] Base36 = {
            //    "0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H",
            //    "I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            //};
            char[] Base36 = ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToCharArray();
            long IDecimal = 0;
            for (int i = IBaseX.Length - 1; i >= 0; --i)
            {
                long bc = (long)Math.Pow(IBase, (IBaseX.Length - 1 - i));
                if (Base36.Contains(IBaseX[i]))
                    IDecimal += Array.LastIndexOf(Base36, IBaseX[i].ToString()) * bc;
                else throw new InvalidCastException();
            }
            return IDecimal;
        }

        public static string ToBaseX(long IDouble, int IBase)
        {
            if ((IBase < 2) || (IBase > 36))
                throw new Exception("Base is out of range please enter a value between 2 and 36.");

            //string[] Base36 = {
            //    "0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H",
            //    "I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            //};

            char[] Base36 = ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToCharArray();
            Base36 = Base36.Take(IBase).ToArray();

            string BaseX = string.Empty;
            decimal i;
            do
            {
                i = (decimal)IDouble % IBase;
                BaseX = Base36[(int)i] + BaseX;
                long xx;
                IDouble = Math.DivRem(IDouble, IBase, out xx);
            } while (IDouble >= 1);
            return BaseX;
        }

        public static string ToAlpha(long IDouble)
        {
            int IBase = 26;
            string[] Base36 = {
                "A","B","C","D","E","F","G","H","I","J","K","L","M",
                "N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };
            Base36 = Base36.Take(IBase).ToArray();
            string BaseX = string.Empty;
            decimal i;
            do
            {
                i = (decimal)IDouble % IBase;
                BaseX = Base36[(int)i] + BaseX;
                long xx;
                IDouble = Math.DivRem(IDouble, IBase, out xx);
            } while (IDouble >= 1);
            return BaseX;
        }

        public static string AddUpDate()
        {
            var now = DateTime.Now;
            return (
                now.Year + now.Month + now.Day + now.Hour +
                now.Minute + now.Second + now.Millisecond).ToString();
        }

        public static string AddUpDate(int no)
        {
            var now = DateTime.Now;
            return (
                now.Year + now.Month + now.Day + now.Hour +
                now.Minute + now.Second + now.Millisecond + no).ToString();
        }

        public static string GetDateCode() => Truncate(DateTime.Now.ToFileTime().ToString());

        public static string CreateSalt(byte size = 128)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[] { size };
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string EncodeSHA512(string d2e, string salt = "")
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytD2e = uEncode.GetBytes(d2e + salt);
            SHA512 sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(bytD2e);
            return Convert.ToBase64String(hash);
        }

        public static string EncodeSHA256(string d2e, string salt = "")
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytD2e = uEncode.GetBytes(d2e + salt);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(bytD2e);
            return Convert.ToBase64String(hash);
        }

        public static string EncodeSHA1(string Data)
        {
            SHA1Managed shaM = new SHA1Managed();
            Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)));
            byte[] eNC_data = Encoding.ASCII.GetBytes(Data);
            string eNC_str = Convert.ToBase64String(eNC_data);
            return eNC_str;
        }

        public static string DecodeSHA1(string Data)
        {
            byte[] dEC_data = Convert.FromBase64String(Data);
            string dEC_Str = Encoding.ASCII.GetString(dEC_data);
            return dEC_Str;
        }

        public static string GetAllExceptionMessage(Exception ex)
        {
            string msg = ex.Message;
            if (ex.InnerException != null)
                msg += string.Format(" \n[INNER: {0}]", GetAllExceptionMessage(ex.InnerException));
            return msg;
        }

        public static string GenerateUrlKey(string txt) =>
            StripHTML(StripSymbols(txt)).Replace(" ", "-").Replace("--", "-").ToLower();

        public static string GenerateTags(params string[] keys) => string.Join(",", keys);

        public static string StripHTML(string htmlstring) =>
            Regex.Replace(htmlstring, "<[^>]*>", string.Empty).Replace("&nbsp;", string.Empty).Trim();

        public static string StripSymbols(string xstring) => 
            new Regex("[;\\\\\\\\/:*?\"<>|&']")
                .Replace(xstring, string.Empty)
                .Replace("+", string.Empty)
                .Replace(".", string.Empty)
                .Replace("`", string.Empty)
                .Replace("'", string.Empty)
                .Replace(",", string.Empty)
                .Replace("/", string.Empty)
                .Replace("'", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("#", string.Empty)
                .Replace("*", string.Empty);

        public static string GetSubstring(string text, int startIndex) => text.Substring(startIndex);
        public static string GetSubstring(string text, int startIndex, int count) => text.Substring(startIndex, count);

        public static string GenerateFileName(string title, string fileExtension) =>
            GenerateFileName(title, fileExtension, string.Empty);
        public static string GenerateFileName(string title, string fileExtension, string siteName)
        {
            var ts = StripSymbols(StripHTML(title)).Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Take(5).ToList();
            ts.Add(Guid.NewGuid().ToString().Substring(0, 5));
            return $"{siteName}-{string.Join("-", ts)}.{fileExtension}".Trim('-').Trim();
        }

        public static string Truncate(string id)
        {
            int b = id.Length / 2;
            return (Convert.ToInt64(id.Substring(0, b)) + Convert.ToInt64(id.Substring(b + 1))).ToString();
        }

        public static string GetShareLink(string url, Sharers sharer, string description, string imageUrl)
        {
            switch (sharer)
            {
                case Sharers.Facebook:
                    return string.Format("https://www.facebook.com/sharer/sharer.php?u={0}", url);
                case Sharers.Twitter:
                    return string.Format("https://twitter.com/home?status=Check%20out%20this%20article:%20{1}%20-%20{0}", url, description);
                case Sharers.Pinterest:
                    return string.Format("https://pinterest.com/pin/create/button/?url={0}&amp;media={1}&amp;description={2}", url, imageUrl, description);
                case Sharers.Google:
                    return string.Format("https://plus.google.com/share?url={0}", url);
                default:
                    return string.Empty;
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, ImageSizePref pref = ImageSizePref.None)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;

            double ratio = 1;

            switch (pref)
            {
                case ImageSizePref.Height:
                    ratio = ratioY;
                    break;
                case ImageSizePref.Width:
                    ratio = ratioX;
                    break;
                default:
                    ratio = Math.Max(ratioX, ratioY);
                    break;
            }

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static Image CropImage(Bitmap source, int maxWidth, int maxHeight)
        {
            // An empty bitmap which will hold the cropped image  
            Bitmap bmp = new Bitmap(maxWidth, maxHeight);
            Graphics g = Graphics.FromImage(bmp);

            // reduce image quality
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50);

            // Draw the given area (section) of the source image  
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, new Rectangle(0, 0, maxWidth, maxHeight), GraphicsUnit.Pixel);
            return bmp;
        }

        //public static Bitmap GenerateQRCode(string text) => GenerateQRCode(text, Color.Black, Color.White);

        //public static Bitmap GenerateQRCode(string text, Color DarkColor, Color LightColor)
        //{
        //    throw new NotImplementedException();

        //    //var jjdjd = new Gma.QrCodeNet.Encoding.MatrixSize();

        //    //Gma.QrCodeNet.Encoding.QrEncoder Encoder = new Gma.QrCodeNet.Encoding.QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.H);
        //    //Gma.QrCodeNet.Encoding.QrCode Code = Encoder.Encode(text);
        //    //Bitmap TempBMP = new Bitmap(Code.Matrix.Width, Code.Matrix.Height);
        //    //for (int X = 0; X <= Code.Matrix.Width - 1; X++)
        //    //{
        //    //    for (int Y = 0; Y <= Code.Matrix.Height - 1; Y++)
        //    //    {
        //    //        if (Code.Matrix.InternalArray[X, Y])
        //    //            TempBMP.SetPixel(X, Y, DarkColor);
        //    //        else
        //    //            TempBMP.SetPixel(X, Y, LightColor);
        //    //    }
        //    //}
        //    //TempBMP.SetResolution(6000, 6000000);
        //    //return TempBMP;
        //}

        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            return destImage;
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().First(x => x.MimeType.ToLower() == "image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // convert image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // convert by to base64 string
                string base64 = Convert.ToBase64String(imageBytes);

                return base64;
            }
        }

        public static Image Base64ToImage(string base64)
        {
            // convert string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // convert byte[] to image
            ms.Write(imageBytes, 0, imageBytes.Length);

            return Image.FromStream(ms, true);
        }

        public static string CreateParagraphsFromReturns(string text)
        {
            XElement xe = new XElement("div");

            IEnumerable<string> blocks = text.Split('\n');

            foreach (string block in blocks)
            {
                if (!string.IsNullOrEmpty(block))
                    xe.Add(new XElement("p", block));
            }

            return xe.ToString();
        }

        public static string CreateReturnsFromParagraphs(string text)
        {
            text = text.Replace("<div>", string.Empty)
                .Replace("</div>", string.Empty)
                .Replace("<p>", string.Empty)
                .Replace("</p>", string.Empty);

            IEnumerable<string> blocks = text.Split('\n');

            text = string.Empty;

            foreach (string block in blocks)
            {
                if (!string.IsNullOrEmpty(block.Trim()))
                    text += (text == string.Empty ? string.Empty : "\n") + block.Trim();
            }

            return StripHTML(text);
        }
    }

}