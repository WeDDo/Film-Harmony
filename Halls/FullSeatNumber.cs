using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Halls
{
    public class FullSeatNumber
    {
        public int Number { get; set; }
        public string Letter { get; set; }
        public string EntireNumber { get; set; }

        public FullSeatNumber()
        {

        }

        public FullSeatNumber(int number, string letter)
        {
            Number = number;
            Letter = letter;
            EntireNumber = string.Format("{0}{1}", number, Letter);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Number, Letter);
        }
    }
}