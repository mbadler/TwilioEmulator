using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Phones
{
    public interface IPhoneManager
    {
        PhoneStatus AttemptDial(string PhoneNumber);
        void CallHungUp(string PhoneNumber, string Reason);
        void SayReceived(string PhoneNumber, string Text);
    }
}
