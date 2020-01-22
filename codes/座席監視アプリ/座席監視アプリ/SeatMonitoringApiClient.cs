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
    public class SeatMonitoringApiClient : ISeatMonitoringApiClient
    {
        private string IpAddress { get; set; }
        private IMyHttpClient HttpClient { get; set; }

        public SeatMonitoringApiClient(string ipAddress, IMyHttpClient httpClient)
        {
            IpAddress = ipAddress;
            HttpClient = httpClient;
        }

        public List<Seat> GetSeats()
        {
            HttpResponseMessage responseMessage;
            HttpClient.Timeout = TimeSpan.FromMilliseconds(60000);
            responseMessage = HttpClient.GetAsync($@"http://{IpAddress}:44383/api/seats");
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new AggregateException("サーバへの接続に失敗しました。");
            }

            var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
            var seatsResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);

            return ((IEnumerable<dynamic>)seatsResponse)
                .Select(seat => new Seat((string)seat.Name, (string)seat.Status))
                .ToList();
        }
    }
}
