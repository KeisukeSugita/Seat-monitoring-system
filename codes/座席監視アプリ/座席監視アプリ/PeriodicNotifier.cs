using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// コンストラクタで指定されたメソッドに結果を通知するクラス
    /// </summary>
    public class PeriodicNotifier : IPeriodicNotifier
    {
        public delegate void Destination(List<Seat> seats, bool isSucceeded);
        private ISeatMonitoringApiClient SeatMonitoringApiClient { get; set; }
        private Destination destination;
        public bool IsStopRequested { get; set; }

        public PeriodicNotifier(Destination destination, ISeatMonitoringApiClient seatMonitoringApiClient)
        {
            this.destination = destination;
            SeatMonitoringApiClient = seatMonitoringApiClient;
            IsStopRequested = false;
        }

        /// <summary>
        /// 座席状態の取得と結果の通知を1分毎に非同期に行うメソッド
        /// </summary>
        public void Start()
        {
            Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                while (!IsStopRequested)
                {
                    stopwatch.Start();
                    List<Seat> seats = null;
                    try
                    {
                        // SeatMonitoringAPIの結果を取得
                        seats = SeatMonitoringApiClient.GetSeats();
                    }
                    catch(SeatsApiException)
                    {
                        // 結果を通知
                        destination(seats, false);
                    }
                    // 結果を通知
                    destination(seats, true);

                    stopwatch.Stop();

                    // (60－処理時間)秒、処理を止める
                    if (60000 > stopwatch.ElapsedMilliseconds)
                    {
                        Thread.Sleep((int)(60000 - stopwatch.ElapsedMilliseconds));
                    }
                }
            });
        }

        /// <summary>
        /// 非同期処理を終了させるメソッド
        /// </summary>
        public void Stop()
        {
            IsStopRequested = true;
        }
    }
}
