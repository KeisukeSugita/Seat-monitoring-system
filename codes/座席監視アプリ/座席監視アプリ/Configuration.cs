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
        private static Configuration instance = null;
        public static Configuration Instance
        {
            get
            {
                if (instance == null)   // 初期化されていない場合
                {
                    instance = new Configuration();
                }
                return instance;
            }
            private set
            {
            }
        }
        public string IpAddress { get; private set; }

        public Configuration()
        {
            IpAddress = ConfigurationManager.AppSettings["IpAddress"];
        }
    }
}
