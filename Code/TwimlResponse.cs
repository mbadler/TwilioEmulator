using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TwilioEmulator.Code
{
    public class TwimlResponse
    {
        public string  GatherCallBaackUrl { get; set; }
        public string FallBackUrl { get; set; }
        public XDocument Twiml { get; set; }
        public XNode CurrentElem { get; set; }

        public TwimlResponse(string Twiml)
        {
            this.Twiml = XDocument.Parse(Twiml);
            if (this.Twiml.Nodes().Count() == 0)
            {
                throw new Exception("There are no nodes in this twiml response");
            }

            CurrentElem = this.Twiml.Nodes().First();

        }
    }
}
