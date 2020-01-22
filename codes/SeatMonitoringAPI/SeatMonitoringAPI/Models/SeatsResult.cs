using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// SeatsAPIの結果に必要なデータを保持するクラス
    /// </summary>
    public class SeatsResult
    {
        public string Name { get; private set; }    // 監視座席名
        public string Status { get; private set; }  // 座席状態

        public SeatsResult(string name, string status)
        {
            Name = name;
            Status = status;
        }
    }
}