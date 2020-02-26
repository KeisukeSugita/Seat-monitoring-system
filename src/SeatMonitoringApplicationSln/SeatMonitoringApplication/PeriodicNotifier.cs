using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// <see cref="IPeriodicNotifier"/>
    /// </summary>
    public class PeriodicNotifier : IPeriodicNotifier
    {
        private ISeatMonitoringApiClient SeatMonitoringApiClient { get; set; }
        /// <summary>
        /// <see cref="IPeriodicNotifier.Destination"/>
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
        /// <see cref="IPeriodicNotifier.Start"/>
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
        /// <see cref="IPeriodicNotifier.Stop"/>
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
            task.Wait();
        }
    }
}
