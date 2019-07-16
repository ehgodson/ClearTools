using System;
using System.Linq;

namespace Clear
{
    public interface IBaseConverter
    {
        string ConvertFromDecimal(long value, int targetBase);
        string ConvertToAlpa(long IDouble);
        long ConvertToDecimal(string value, int originalBase);
    }
    public class BaseConverter : IBaseConverter
    {
        private char[] Base36 = ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToCharArray();

        public long ConvertToDecimal(string value, int originalBase)
        {
            value = value.ToUpper();
            long IDecimal = 0;

            for (int i = value.Length - 1; i >= 0; --i)
            {
                long bc = (long)Math.Pow(originalBase, (value.Length - 1 - i));
                if (Base36.Contains(value[i]))
                    IDecimal += Array.LastIndexOf(Base36, value[i].ToString()) * bc;
                else throw new InvalidCastException();
            }

            return IDecimal;
        }

        public string ConvertFromDecimal(long value, int targetBase)
        {
            if ((targetBase < 2) || (targetBase > 36))
                throw new Exception("Base is out of range please enter a value between 2 and 36.");

            var base36 = Base36.Take(targetBase).ToArray();

            string BaseX = string.Empty;
            decimal i;
            do
            {
                i = (decimal)value % targetBase;
                BaseX = base36[(int)i] + BaseX;
                value = Math.DivRem(value, targetBase, out long xx);
            } while (value >= 1);
            return BaseX;
        }

        public string ConvertToAlpa(long IDouble)
        {
            int IBase = 26;
            char[] Base26 = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToCharArray();
            string BaseX = string.Empty;

            decimal i;
            do
            {
                i = (decimal)IDouble % IBase;
                BaseX = Base26[(int)i] + BaseX;
                IDouble = Math.DivRem(IDouble, IBase, out long xx);
            } while (IDouble >= 1);

            return BaseX;
        }
    }
}