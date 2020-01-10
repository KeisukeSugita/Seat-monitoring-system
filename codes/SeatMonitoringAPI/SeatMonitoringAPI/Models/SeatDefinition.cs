using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class SeatDefinition
    {
        public string Moniker { get; private set; } // モニカ
        public string Name { get; private set; }    // 監視座席名

        public SeatDefinition(string moniker, string name)
        {
            Moniker = moniker;
            Name = name;
        }
    }
}