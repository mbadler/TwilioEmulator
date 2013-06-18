using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Phones
{
    public interface IPhoneManager
    {
        PhoneStatus AttemptDial(string PhoneNumber);  
    }
}
