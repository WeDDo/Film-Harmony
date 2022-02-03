using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Halls
{
    [XmlRoot(ElementName = "Hall")]
    public class Hall
    {
        [XmlElement("HallID")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("TicketLimit")]
        public int TicketLimit { get; set; }

        [XmlElement(ElementName = "HallGroup")]
        public List<HallGroup> hallGroups { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Id, Name, TicketLimit);
        }
    }
}