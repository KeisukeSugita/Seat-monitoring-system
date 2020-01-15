using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public enum SeatStatus
    {
        Exists,
        NotExists,
        Failure
    }

    public class Seat
    {
        public SeatDefinition SeatDefinition { get; private set; }  // 監視座席名、モニカ
        public SeatStatus Status { get; private set; }   // 座席状態

        public Seat(SeatDefinition seatDefinition, SeatStatus status)
        {
            SeatDefinition = seatDefinition;
            Status = status;
        }
    }
}