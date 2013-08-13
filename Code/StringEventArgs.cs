using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Code
{
    public class StringEventArgs:EventArgs
    {
        public string value { get; set; }
        public StringEventArgs(string StringValue)
        {
            this.value = StringValue;
        }
    }
}
