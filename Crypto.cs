using System;
using System.Security.Cryptography;
using System.Text;

namespace Clear
{
    public interface ICrypto
    {
        string CreateSalt(byte size = 128);
        string DecodeSHA1(string Data);
        string EncodeSHA1(string Data);
        string EncodeSHA256(string d2e, string salt = "");
        string EncodeSHA512(string d2e, string salt = "");
    }
    public class Crypto : ICrypto
    {
        public string CreateSalt(byte size = 128)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[] { size };
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string EncodeSHA512(string d2e, string salt = "")
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytD2e = uEncode.GetBytes(d2e + salt);
            SHA512 sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(bytD2e);
            return Convert.ToBase64String(hash);
        }

        public string EncodeSHA256(string d2e, string salt = "")
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytD2e = uEncode.GetBytes(d2e + salt);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(bytD2e);
            return Convert.ToBase64String(hash);
        }

        public string EncodeSHA1(string Data)
        {
            SHA1Managed shaM = new SHA1Managed();
            Convert.ToBase64String(shaM.ComputeHash(Encoding.ASCII.GetBytes(Data)));
            byte[] eNC_data = Encoding.ASCII.GetBytes(Data);
            string eNC_str = Convert.ToBase64String(eNC_data);
            return eNC_str;
        }

        public string DecodeSHA1(string Data)
        {
            byte[] dEC_data = Convert.FromBase64String(Data);
            string dEC_Str = Encoding.ASCII.GetString(dEC_data);
            return dEC_Str;
        }
    }
}