using System;
using System.Net.Http;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// HttpClientクラスの一部を実装したクラス
    /// このクラスを介してHttpClientを利用する
    /// </summary>
    class MyHttpClient : IMyHttpClient
    {
        private HttpClient httpClient = new HttpClient();
        private TimeSpan timeout;
        public TimeSpan Timeout
        {
            get => timeout;
            set => timeout = httpClient.Timeout = value;
        }

        /// <summary>
        /// HttpClientクラスのGetAsyncメソッドの結果を返すメソッド
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns>レスポンスメッセージ</returns>
        public HttpResponseMessage GetAsync(string requestUri)
        {
            return httpClient.GetAsync(requestUri).Result;
        }
    }
}
