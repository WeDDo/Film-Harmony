using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Halls
{
    [XmlRoot(ElementName ="HallGroup")]
    public class HallGroup
    {
        public HallGroup()
        {
        }

        public HallGroup(int id, int hallId, string name, int az)
        {
            Id = id;
            HallID = hallId;
            Name = name;
            AZ = az;
        }

        [XmlElement(ElementName = "HallGroupID")]
        public int Id { get; set; }

        [XmlElement(ElementName = "HallID")]
        public int HallID { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "AZ")]
        public int AZ { get; set; }

        [XmlElement(ElementName = "HallSeat")]
        public List<HallSeat> HallSeats { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Id, HallID, Name, AZ);
        }
    }
}