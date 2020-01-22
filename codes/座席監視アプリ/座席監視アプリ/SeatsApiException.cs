using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class SeatsApiException : Exception
    {
        public SeatsApiException() : base() { }
        public SeatsApiException(string message) : base(message) { }
    }
}
