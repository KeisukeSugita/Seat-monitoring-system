using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly int interval;
        private Task task;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public PeriodicNotifier(Destination destination, ISeatMonitoringApiClient seatMonitoringApiClient, int interval = 60 * 1000)
        {
            this.destination = destination;
            SeatMonitoringApiClient = seatMonitoringApiClient;
            this.interval = interval;
        }

        /// <summary>
        /// 座席状態の取得と結果の通知を1分毎に非同期に行うメソッド
        /// </summary>
        public void Start()
        {            
            task = Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    stopwatch.Restart();
                    List<Seat> seats = null;
                    try
                    {
                        // SeatMonitoringAPIの結果を取得
                        seats = SeatMonitoringApiClient.GetSeats();
                        // 結果を通知
                        destination(seats, true);
                    }
                    catch(SeatsApiException)
                    {
                        // 結果を通知
                        destination(seats, false);
                    }

                    stopwatch.Stop();

                    // (60－処理時間)秒、処理を止める
                    if (interval > stopwatch.ElapsedMilliseconds)
                    {
                        cancellationTokenSource.Token.WaitHandle.WaitOne((int)(interval - stopwatch.ElapsedMilliseconds));
                    }
                }
            });
        }

        /// <summary>
        /// 非同期処理を終了させるメソッド
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
            task.Wait();
        }
    }
}
