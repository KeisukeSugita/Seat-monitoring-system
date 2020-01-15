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
            var camera = new Camera();
            var humanDetector = new HumanDetector();
            var seats = new List<Seat>();

            foreach (var seatDefinition in Configuration.Instance.SeatDefinitions)
            {
                Bitmap photo = null;
                SeatStatus humanExists;

                try
                {
                    photo = camera.Shoot(seatDefinition.Moniker);
                }
                catch (InvalidOperationException)
                {
                    humanExists = SeatStatus.Failure;
                    break;
                }

                if (humanDetector.Detect(photo))
                {
                    humanExists = SeatStatus.Exists;
                }
                else
                {
                    humanExists = SeatStatus.NotExists;
                }
                
                seats.Add(new Seat(seatDefinition, humanExists));
            }

            return seats;
        }
    }
}