using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 初期化されたConfigurationのSeatDefinitionsのすべての座席に対して座席の状態を取得し、
    /// Seat型のリストを作成するクラス
    /// </summary>
    public class SeatsScanner : ISeatsScanner
    {
        private readonly List<ICamera> cameras; // カメラから画像を取得するためのICameraのList
        private readonly IHumanDetector humanDetector;  // 画像に人が写っているかを判定するためのIHumanDetector

        public SeatsScanner(List<ICamera> cameras, IHumanDetector humanDetector)
        {
            this.cameras = cameras;
            this.humanDetector = humanDetector;
        }

        /// <summary>
        /// Cameras内のすべてのCameraインスタンスから画像を取得してそれぞれの画像を判定し
        /// ConfigurationのSeatDefinitionsと対応する判定結果をList<Seat>にして返すメソッド
        /// </summary>
        /// <returns>監視座席名と判定結果を対応させたリスト</returns>
        public List<Seat> ScanAll()
        {
            var seats = new List<Seat>();
            int seatNum = 0;

            foreach (var camera in cameras)
            {
                Bitmap photo;
                SeatStatus status;

                try
                {
                    // 画像を取得
                    photo = camera.Shoot();

                    // 画像に人が写っているか判定
                    status = humanDetector.Detect(photo) ? SeatStatus.Exists : SeatStatus.NotExists;
                }
                catch (InvalidOperationException)
                {
                    status = SeatStatus.Failure;
                }

                // SeatクラスのインスタンスをListに追加
                seats.Add(new Seat(Configuration.Instance.seatDefinitions[seatNum], status));

                seatNum++;
            }

            return seats;
        }
    }
}