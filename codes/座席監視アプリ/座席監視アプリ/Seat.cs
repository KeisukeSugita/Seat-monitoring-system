using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class Seat
    {
        private object name;
        private SeatStatus seatStatus;

        public enum SeatStatus
        {
            Exists,
            NotExists,
            Failure
        }

        [DataMember(Name = "Name")]
        public string Name { get; private set; }
        [DataMember(Name = "Status")]
        public SeatStatus Status { get; private set; }


        public Seat(string name, SeatStatus status)
        {
            Name = name;
            Status = status;
        }

        public Seat(string name, string status)
        {
            Name = name;
            Status = FromString(status);
        }

        public SeatStatus FromString(string status)
        {
            if (status == Seat.SeatStatus.Exists.ToString())
            {
                return Seat.SeatStatus.Exists;
            }
            else if (status == Seat.SeatStatus.NotExists.ToString())
            {
                return Seat.SeatStatus.NotExists;
            }
            else if (status == Seat.SeatStatus.Failure.ToString())
            {
                return Seat.SeatStatus.Failure;
            }
            else
            {
                throw new InvalidOperationException("指定された文字列はSetStatusに存在しません。");
            }
        }
    }
}
