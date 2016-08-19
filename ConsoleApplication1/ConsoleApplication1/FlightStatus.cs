using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public enum FlightStatus 
    {
        checkIn, 
        gateClosed, 
        arrived, 
        departedAt, 
        unknown, 
        canceled, 
        expectedAt, 
        delayed, 
        inFlight
    }
}
