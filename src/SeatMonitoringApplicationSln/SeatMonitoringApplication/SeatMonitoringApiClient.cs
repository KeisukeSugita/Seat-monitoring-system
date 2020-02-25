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
    /// SeatMonitoringAPIのメソッドを呼び出すクラス
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
        /// GetSeatsメソッドを呼び出し、結果をアプリが解釈しやすい形に変換するメソッド
        /// </summary>
        /// <returns>取得した座席情報のリスト</returns>
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
