using System.Collections.Generic;

namespace SeatMonitoringAPI.Models
{
    public interface ISeatsScanner
    {
        List<Seat> ScanAll();
    }
}
