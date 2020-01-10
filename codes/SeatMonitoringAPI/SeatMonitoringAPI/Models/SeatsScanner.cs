using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class SeatsScanner
    {
        public List<Seat> ScanAll()
        {
            Camera camera = new Camera();
            HumanDetector humanDetector = new HumanDetector();
            var seats = new List<Seat>();

            foreach (var seatDefinition in Configuration.Instance.SeatDefinitions)
            {
                Bitmap photo = null;
                bool succeeded = true;
                bool humanExists = false;

                try
                {
                    photo = camera.Shoot(seatDefinition.Moniker);
                }
                catch (InvalidOperationException)
                {
                    succeeded = false;
                }
                if (photo != null)
                {
                    humanExists = humanDetector.Detect(photo);
                }
                
                seats.Add(new Seat(seatDefinition, succeeded, humanExists));
            }

            return seats;
        }
    }
}