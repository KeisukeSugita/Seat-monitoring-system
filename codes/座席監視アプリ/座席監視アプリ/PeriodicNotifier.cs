using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class PeriodicNotifier : IPeriodicNotifier
    {
        public delegate void Destination(List<Seat> seats, bool isSucceeded);
        private ISeatMonitoringApiClient SeatMonitoringApiClient { get; set; }
        private Destination destination;
        public bool IsStopRequested { get; private set; }

        public PeriodicNotifier(Destination destination, ISeatMonitoringApiClient seatMonitoringApiClient)
        {
            this.destination = destination;
            SeatMonitoringApiClient = seatMonitoringApiClient;
            IsStopRequested = false;
        }

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
                        seats = SeatMonitoringApiClient.GetSeats();
                        destination(seats, true);
                    }
                    catch(SeatsApiException)
                    {
                        destination(seats, false);
                    }

                    stopwatch.Stop();
                    if (60000 > stopwatch.ElapsedMilliseconds)
                    {
                        Thread.Sleep((int)(60000 - stopwatch.ElapsedMilliseconds));
                    }
                }
            });
        }

        public void Stop()
        {
            IsStopRequested = true;
        }
    }
}
