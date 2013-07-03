using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TwilioEmulator.Code.Extensions
{
    public static class Linq2XMLExtensions
    {
        public static XElement NextElement(this XElement ele)
        {
            var a = ele.NextNode;
            while (a != null && a.NodeType != System.Xml.XmlNodeType.Element)
            {
                a = a.NextNode;
            }
            return (XElement)a;
        }
    }
}
