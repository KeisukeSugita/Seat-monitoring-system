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
        public readonly string name;    // 監視座席名
        public readonly string status;  // 座席状態

        public SeatsResult(string name, string status)
        {
            this.name = name;
            this.status = status;
        }
    }
}