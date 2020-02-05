using System.Drawing;

namespace SeatMonitoringAPI.Models
{
    public interface ICamera
    {
        Bitmap Shoot();
    }
}
