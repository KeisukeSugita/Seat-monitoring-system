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
    /// <summary>
    /// <see cref="ISeatMonitoringApiClient"/>
    /// </summary>
    public class SeatMonitoringApiClient : ISeatMonitoringApiClient
    {
        private string Host { get; set; }
        private IMyHttpClient HttpClient { get; set; }

        public SeatMonitoringApiClient(string host, IMyHttpClient httpClient)
        {
            Host = host;
            HttpClient = httpClient;
        }

        /// <summary>
        /// <see cref="ISeatMonitoringApiClient.GetSeats"/>
        /// </summary>
        public List<Seat> GetSeats()
        {
            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = HttpClient.GetAsync($@"http://{Host}/api/seats");
            }
            catch (AggregateException e)
            {
                throw new SeatsApiException("サーバへの接続に失敗しました。", e);
            }
            
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                // 期待しないステータスコードを受け取った場合
                throw new SeatsApiException($@"ステータスコード""{(int)responseMessage.StatusCode}""で失敗しました。");
            }
            
            var responseBody = responseMessage.Content.ReadAsStringAsync().Result;

            // JSON形式の文字列をdynamic型にデシリアライズ
            var seatsResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
            // dynammic型からList<Seat>に変換
            return ((IEnumerable<dynamic>)seatsResponse)
                .Select(seat => new Seat((string)seat.name, (string)seat.status))
                .ToList();
        }
    }
}
