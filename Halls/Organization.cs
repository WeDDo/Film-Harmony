using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Halls
{
    [XmlRoot(ElementName = "Filharmonija")]
    public class Organization
    {
        [XmlElement(ElementName = "Hall")]
        public List<Hall> Hall { get; set; }
    }
}