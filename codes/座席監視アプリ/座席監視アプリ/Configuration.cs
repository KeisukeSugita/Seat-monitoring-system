using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class Configuration
    {
        public string IpAddress { get; private set; }

        public Configuration()
        {
            IpAddress = ConfigurationManager.AppSettings["IpAddress"];
        }
    }
}
