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
        private SeatMonitoringApiClient SeatMonitoringApiClient { get; set; }
        private Destination destination;
        private bool isStopRequested = false;

        public PeriodicNotifier(Destination destination, SeatMonitoringApiClient seatMonitoringApiClient)
        {
            this.destination = destination;
            SeatMonitoringApiClient = seatMonitoringApiClient;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                while (!isStopRequested)
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
            isStopRequested = true;
        }
    }
}
