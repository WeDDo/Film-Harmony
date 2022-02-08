using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Halls
{
    public class FullSeatRow
    {
        public int Row { get; set; }
        public string Letter { get; set; }
        public string EntireRow { get; set; }

        public FullSeatRow()
        {

        }

        public FullSeatRow(int row, string letter)
        {
            Row = row;
            Letter = letter;
            EntireRow = string.Format("{0}{1}", Row, Letter);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Row, Letter);
        }
    }
}