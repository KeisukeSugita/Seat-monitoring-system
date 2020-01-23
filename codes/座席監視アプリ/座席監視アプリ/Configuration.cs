using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// アプリの設定を保持するシングルトンクラス
    /// 初めてgetが行われたときに値が初期化される
    /// </summary>
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
        }
        public string IpAddress { get; private set; }

        public Configuration()
        {
            IpAddress = ConfigurationManager.AppSettings["IpAddress"];
            if (IpAddress == null)
            {
                throw new InvalidOperationException("\"IpAddress\"が見つかりません。");
            }
        }
    }
}
