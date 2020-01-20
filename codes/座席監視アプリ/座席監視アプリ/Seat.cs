using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class Seat
    {
        public enum SeatStatus
        {
            Exists,
            NotExists,
            Failure
        }

        public string Name { get; private set; }
        public SeatStatus Status { get; private set; }

        public Seat(string name, SeatStatus status)
        {
            Name = name;
            Status = status;
        }
    }
}
