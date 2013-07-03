using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TwilioEmulator.Code
{
    public interface IPhoneInteractionLogger
    {
        void LogInteraction(InteractionWho fromwho, InteractionWhat what,Color BackgroundColor, string interaction);
    }

    public enum InteractionWho
    {
        Phone, server
    }

    public enum InteractionWhat
    {
        Say, Play, SMS, Dial, Pickup, Hangup
    }
}
