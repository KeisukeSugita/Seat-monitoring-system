using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 座席の状態を表すためのEnum
    /// </summary>
    public enum SeatStatus
    {
        Exists, // 在席
        NotExists,  // 離席
        Failure // 不明
    }

    /// <summary>
    /// 座席の定義に加え、座席状態も保持するクラス
    /// </summary>
    public class Seat
    {
        public readonly SeatDefinition seatDefinition;  // 監視座席名、モニカ
        public readonly SeatStatus status;   // 座席状態

        public Seat(SeatDefinition seatDefinition, SeatStatus status)
        {
            this.seatDefinition = seatDefinition;
            this.status = status;
        }
    }
}