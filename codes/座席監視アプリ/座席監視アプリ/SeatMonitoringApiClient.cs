using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public class SeatMonitoringApiClient
    {
        private string IpAddress { get; set; }

        public SeatMonitoringApiClient(string ipAddress)
        {
            IpAddress = ipAddress;
        }

        public List<Seat> GetSeats()
        {
            using (var httpClient = new HttpClient())
            {
                var responseMessage = httpClient.GetAsync($@"https://{IpAddress}:44383/api/seats").Result;
                if(responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                    var seatsResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    var seatsResult = new List<Seat>();
                    foreach (var seat in seatsResponse)
                    {
                        string name = seat.Name;
                        Seat.SeatStatus status;
                        if (seat.Status == "Exists")
                        {
                            status = Seat.SeatStatus.Exists;
                        }
                        else if (seat.Status == "NotExists")
                        {
                            status = Seat.SeatStatus.NotExists;
                        }
                        else
                        {
                            status = Seat.SeatStatus.Failure;
                        }

                        seatsResult.Add(new Seat(name, status));
                    }

                    return seatsResult;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

        }
    }
}
