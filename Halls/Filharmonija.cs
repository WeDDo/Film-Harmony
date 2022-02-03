using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Halls
{
    [XmlRoot(ElementName = "Filharmonija")]
    public class Filharmonija
    {
        [XmlElement(ElementName = "Hall")]
        public Hall Hall { get; set; }
    }
}