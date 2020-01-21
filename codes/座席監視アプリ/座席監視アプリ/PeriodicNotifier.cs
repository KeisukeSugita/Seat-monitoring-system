using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class PeriodicNotifier
    {
        public delegate void Destination(List<Seat> seats);
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
            while(!isStopRequested)
            {
                var stopwatch = new Stopwatch();
                Task task = new Task(() =>
                {
                    stopwatch.Start();
                    destination(SeatMonitoringApiClient.GetSeats());
                    stopwatch.Stop();
                    if (60.0 > stopwatch.ElapsedMilliseconds)
                    {
                        Thread.Sleep((int)(60 - stopwatch.ElapsedMilliseconds) * 100);
                    }
                });
            }
        }

        public void Stop()
        {
            isStopRequested = true;
        }
    }
}
