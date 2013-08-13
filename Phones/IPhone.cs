using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Phones
{
    public interface IPhone
    {
        PhoneStatus PhoneStatus { get; set; }
        string PhoneNumber { get; set; }
    }

    public enum PhoneStatus
    {
        ReadyHuman,
        ReadyMachine,
        Talking,
        Busy,
        Ringing,
        NotInService,
        NoAnswer
    }
}
