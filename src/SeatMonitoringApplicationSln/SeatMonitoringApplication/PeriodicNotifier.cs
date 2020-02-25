using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 指定されたメソッド(Destination)に結果を通知するクラス
    /// </summary>
    public class PeriodicNotifier : IPeriodicNotifier
    {
        private ISeatMonitoringApiClient SeatMonitoringApiClient { get; set; }
        /// <summary>
        ///  Startメソッドの中で定期的に実行されるメソッド
        ///  座席の状態を通知させたいメソッドを登録できる
        /// </summary>
        public Action<List<Seat>, bool> Destination { get; set; }
        private readonly int interval;
        private Task task;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public PeriodicNotifier(ISeatMonitoringApiClient seatMonitoringApiClient, int interval = 60 * 1000)
        {
            SeatMonitoringApiClient = seatMonitoringApiClient;
            this.interval = interval;
        }

        /// <summary>
        /// 座席状態の取得と結果の通知を定期的に非同期に行うメソッド
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
                        Destination?.Invoke(seats, true);
                    }
                    catch(SeatsApiException)
                    {
                        // 結果を通知
                        Destination?.Invoke(seats, false);
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
