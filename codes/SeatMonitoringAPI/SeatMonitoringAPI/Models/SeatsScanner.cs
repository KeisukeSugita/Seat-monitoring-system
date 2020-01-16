using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeatMonitoringAPI.Models
{
    /*
     * 初期化されたConfigurationのSeatDefinitionsのすべての座席に対して座席の状態を取得し、
     * Seat型のリストを作成するクラス
     */
    public class SeatsScanner : ISeatsScanner
    {
        private List<ICamera> Cameras { get; set; } // カメラから画像を取得するためのICameraのList
        private IHumanDetector HumanDetector { get; set; }  // 画像に人が写っているかを判定するためのIHumanDetector

        public SeatsScanner(List<ICamera> cameras, IHumanDetector humanDetector)
        {
            Cameras = cameras;
            HumanDetector = humanDetector;
        }

        /*
         * Cameras内のすべてのCameraインスタンスから画像を取得してそれぞれの画像を判定し
         * ConfigurationのSeatDefinitionsと対応する判定結果をList<Seat>にして返すメソッド
         */
        public List<Seat> ScanAll()
        {
            var seats = new List<Seat>();
            int seatNum = 0;

            foreach (var camera in Cameras)
            {
                Bitmap photo;
                SeatStatus status;

                try
                {
                    // 画像を取得
                    photo = camera.Shoot();

                    // 画像に人が写っているか判定
                    if (HumanDetector.Detect(photo))
                    {
                        status = SeatStatus.Exists;
                    }
                    else
                    {
                        status = SeatStatus.NotExists;
                    }
                }
                catch (InvalidOperationException)
                {
                    status = SeatStatus.Failure;
                }

                // SeatクラスのインスタンスをListに追加
                seats.Add(new Seat(Configuration.Instance.SeatDefinitions[seatNum], status));

                seatNum++;
            }

            return seats;
        }
    }
}