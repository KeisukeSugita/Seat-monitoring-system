using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class Seat
    {
        public SeatDefinition SeatDefinition { get; private set; }  // 監視座席名、モニカ
        public bool Succeeded { get; private set; } // 状態の取得に成功したか
        public bool HumanExists { get; private set; }   // 座席状態

        public Seat(SeatDefinition seatDefinition, bool succeeded, bool humanExists)
        {
            SeatDefinition = seatDefinition;
            Succeeded = succeeded;
            HumanExists = humanExists;
        }
    }
}