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
                try
                {
                    var responseMessage = httpClient.GetAsync($@"http://{IpAddress}:44383/api/seats").Result;
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        throw new InvalidOperationException("サーバへの接続に失敗しました。");
                    }

                    var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                    var seatsResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    var seatsResult = new List<Seat>();
                    foreach (var seat in seatsResponse)
                    {
                        seatsResult.Add(new Seat((string)seat.Name, (string)seat.Status));
                    }
                    return seatsResult;
                }
                catch(HttpRequestException e)
                {
                    throw new HttpRequestException("サーバへの接続に失敗しました。");
                }
                catch (WebException e)
                {
                    throw new WebException("サーバへの接続に失敗しました。");
                }

                //var seatsResult = JsonConvert.DeserializeObject<dynamic>(responseBody)
                //    .Select(seat => new Seat(seat.Name, Seat.FromString(seat.Status))
                //    .ToList();


            }

        }
    }
}
