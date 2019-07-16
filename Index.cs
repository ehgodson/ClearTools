using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Threading.Tasks;

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

    public static class Tools
    {
        public static IApiClient ApiClient => new ApiClient();
        public static IBaseConverter BaseConverter => new BaseConverter();
        public static ICrypto Crypto => new Crypto();
        public static IFileManager FileManager => new FileManager();
        public static IImageUtility ImageUtility => new ImageUtility();
        public static IStringUtility StringUtility => new StringUtility();

        public static string GetAllExceptionMessage(Exception ex)
        {
            string msg = ex.Message;
            if (ex.InnerException != null)
                msg += string.Format(" \n[INNER: {0}]", GetAllExceptionMessage(ex.InnerException));
            return msg;
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
    }
}