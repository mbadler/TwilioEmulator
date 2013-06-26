using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Code
{
    public enum LogSymbol
    {
        WebToTwilio,
        TwilioToWeb,
        TwilioToPhone,
        PhoneToTwilio
    }

    public static class LogSymbolExtensions
    {
        public static string ToDisplayString(this LogSymbol symbol)
        {
            switch (symbol)
            {
                case LogSymbol.WebToTwilio:
                    return "() --> []";
                case LogSymbol.TwilioToWeb:
                    return "() <-- []";
                case LogSymbol.TwilioToPhone:
                    return "[] -->  }";
                case LogSymbol.PhoneToTwilio:
                    return "[] <--  }";
                default:
                    return "";
            }
        }
    }
}
