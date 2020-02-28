using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Threading;
using System.Linq;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 指定したMonikerに対応するカメラのクラス
    /// コンストラクタでMonikerを指定する
    /// </summary>
    public class TestCamera : ICamera
    {
        private Bitmap photo;  // 取得した画像
        public readonly string moniker;
        private static int latestNumber = 1;
        public TestCamera(string deviceInstansePath)
        {
            moniker = $@"@device:pnp:\\?\{deviceInstansePath}#{{65e8773d-8f56-11d0-a3b9-00a0c9223196}}\global";
        }


        /// <summary>
        /// monikerに対応するカメラから画像を1枚取得し、Bitmap型で返すメソッド
        /// string moniker:カメラのデバイスインスタンスパス
        /// </summary>
        /// <returns>取得した画像データ</returns>
        public Bitmap Shoot()
        {
            photo = null;
            switch (latestNumber % 4)
            {
                case 1:
                    latestNumber++;
                    photo = new Bitmap(@"C:\OJT\座席監視システム\src\SeatMonitoringAPISln\SeatMonitoringAPI\bin\SeatMonitoringAPITest用画像\Exists画像.jpg");
                    break;

                case 2:
                    latestNumber++;
                    photo = new Bitmap(@"C:\OJT\座席監視システム\src\SeatMonitoringAPISln\SeatMonitoringAPI\bin\SeatMonitoringAPITest用画像\NotExists画像.jpg");
                    break;

                case 3:
                    latestNumber++;
                    throw new InvalidOperationException("該当するカメラが存在しません。");

                case 0:
                    latestNumber++;
                    throw new InvalidOperationException("画像が取得できませんでした。該当するカメラが存在しないか、接続が切断された可能性があります。");
            }

            return photo;
        }
    }
}