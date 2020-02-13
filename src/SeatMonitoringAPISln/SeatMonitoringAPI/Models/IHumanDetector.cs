using System.Drawing;

namespace SeatMonitoringAPI.Models
{
    public interface IHumanDetector
    {
        bool Detect(Bitmap photo);
    }
}
