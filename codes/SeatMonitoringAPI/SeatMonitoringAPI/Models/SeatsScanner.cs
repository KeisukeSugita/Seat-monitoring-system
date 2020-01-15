using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public interface ISeatsScanner
    {
        List<Seat> ScanAll();
    }


    public class SeatsScanner : ISeatsScanner
    {
        private List<ICamera> Cameras { get; set; }
        private IHumanDetector HumanDetector { get; set; }

        public SeatsScanner(List<ICamera> cameras, IHumanDetector humanDetector)
        {
            Cameras = cameras;
            HumanDetector = humanDetector;
        }

        public List<Seat> ScanAll()
        {
            var seats = new List<Seat>();
            int seatNum = 0;

            foreach (var camera in Cameras)
            {
                Bitmap photo = null;
                SeatStatus status;

                try
                {
                    photo = camera.Shoot();

                    if (HumanDetector.Detect(photo))
                    {
                        status = SeatStatus.Exists;
                    }
                    else
                    {
                        status = SeatStatus.NotExists;
                    }
                }
                catch (InvalidOperationException)
                {
                    status = SeatStatus.Failure;
                }

                seats.Add(new Seat(Configuration.Instance.SeatDefinitions[seatNum], status));

                seatNum++;
            }

            return seats;
        }
    }
}