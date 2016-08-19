using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airport
{
    public  class FlightInfo
    {
        public string Number{get;set;}
        public string Destination { get; set; }
        public string DeparturePlace { get; set; }
        public string AirLine { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Gate { get; set; }        
        public FlightStatus Status { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", this.Number, this.Destination, this.DeparturePlace, this.AirLine, this.Departure, this.Arrival, this.Gate, this.Status);
        }

    }
}
