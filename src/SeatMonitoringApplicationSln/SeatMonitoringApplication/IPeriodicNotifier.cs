using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public interface IPeriodicNotifier
    {
        Action<List<Seat>, bool> Destination { get; set; }

        void Start();
        void Stop();
    }
}
