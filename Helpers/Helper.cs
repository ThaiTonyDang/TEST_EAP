using System;
using System.Globalization;

namespace Helpers
{
    public static class Helper
    {
        public static string GetPriceFormat(this decimal price)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
            return string.Format(cultureInfo, "{0:C0}", price);
        }

    }
}
