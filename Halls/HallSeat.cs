using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Halls
{
    [XmlRoot(ElementName = "HallSeat")]
    public class HallSeat
    {
        [XmlElement(ElementName = "ShowSeatID")]
        public int Id { get; set; }

        [XmlElement(ElementName = "HallGroupID")]
        public int HallGroupId { get; set; }

        [XmlElement(ElementName = "Color")]
        public string Color { get; set; }

        [XmlElement(ElementName = "Price")]
        public double Price { get; set; }

        [XmlElement(ElementName = "SeatRow")]
        public int Row { get; set; }

        [XmlElement(ElementName = "SeatRowLetter")]
        public string RowLetter { get; set; }

        [XmlElement(ElementName = "SeatNumber")]
        public int Number { get; set; }

        [XmlElement(ElementName = "SeatNumberLetter")]
        public string NumberLetter { get; set; }

        [XmlElement(ElementName = "IsReserved")]
        public bool IsReserved { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", Id, HallGroupId, Color, Price, Row, RowLetter, Number, NumberLetter, IsReserved);
        }
    }
}