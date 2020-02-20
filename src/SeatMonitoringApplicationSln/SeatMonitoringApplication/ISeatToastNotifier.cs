using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// SeatMonitoringApplication用のトースト通知を行うインターフェース
    /// </summary>
    public interface ISeatToastNotifier
    {
        void Notify(List<Seat> currentSeats, bool isSucceeded);
    }
}
