using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Clear
{
    public interface IImageUtility
    {
        Image ConvertBase64ToImage(string base64);
        byte[] ConvertBitmapToBytes(Bitmap bitmap);
        string ConvertImageToBase64(Image image, ImageFormat format);
        Image CropImage(Bitmap source, int maxWidth, int maxHeight);
        Bitmap ResizeImage(Image image, int width, int height);
        void SaveJpegToFile(string path, Image image, int quality);
        void SaveJpegToStream(MemoryStream stream, Image image, int quality);
        Image ScaleImage(Image image, int maxWidth, int maxHeight, ImageSizePref pref = ImageSizePref.None);
    }
    public class ImageUtility : IImageUtility
    {
        public byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public Image ScaleImage(Image image, int maxWidth, int maxHeight, ImageSizePref pref = ImageSizePref.None)
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

        public Image CropImage(Bitmap source, int maxWidth, int maxHeight)
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

        public Bitmap ResizeImage(Image image, int width, int height)
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

        private (ImageCodecInfo, EncoderParameters) GetJpegCodecEncoder(int quality)
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
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().First(x => x.MimeType.ToLower() == "image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;

            //save the image using the codec and the parameters
            return (jpegCodec, encoderParams);
        }

        public void SaveJpegToFile(string path, Image image, int quality)
        {
            var jpegCodecEncoder = GetJpegCodecEncoder(quality);
            image.Save(path, jpegCodecEncoder.Item1, jpegCodecEncoder.Item2);
        }

        public void SaveJpegToStream(MemoryStream stream, Image image, int quality)
        {
            var jpegCodecEncoder = GetJpegCodecEncoder(quality);
            image.Save(stream, jpegCodecEncoder.Item1, jpegCodecEncoder.Item2);
        }

        public string ConvertImageToBase64(Image image, ImageFormat format)
        {
            using MemoryStream ms = new MemoryStream();
            // convert image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // convert by to base64 string
            string base64 = Convert.ToBase64String(imageBytes);

            return base64;
        }

        public Image ConvertBase64ToImage(string base64)
        {
            // convert string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // convert byte[] to image
            ms.Write(imageBytes, 0, imageBytes.Length);

            return Image.FromStream(ms, true);
        }
    }
}