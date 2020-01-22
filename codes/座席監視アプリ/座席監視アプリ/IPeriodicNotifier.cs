using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public interface IPeriodicNotifier
    {
        void Start();
        void Stop();
    }
}
