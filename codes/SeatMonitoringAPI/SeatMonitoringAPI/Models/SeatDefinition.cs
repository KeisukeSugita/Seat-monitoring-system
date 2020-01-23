using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 座席を定義するデータを保持するクラス
    /// </summary>
    public class SeatDefinition
    {
        public readonly string moniker; // モニカ
        public readonly string name;    // 監視座席名

        public SeatDefinition(string moniker, string name)
        {
            this.moniker = moniker;
            this.name = name;
        }
    }
}