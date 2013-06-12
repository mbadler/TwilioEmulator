using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TwilioEmulator.Code;

namespace TwilioEmulator.Offices
{
    public abstract class OfficeBase:object
    {
        public void Startup()
        {
            Task.Factory.StartNew(() =>
                {
                    Process();
                    Thread.Sleep(SystemController.WaitInterval);
                },TaskCreationOptions.LongRunning);
        }

        public abstract void Process();
       
    }
}
