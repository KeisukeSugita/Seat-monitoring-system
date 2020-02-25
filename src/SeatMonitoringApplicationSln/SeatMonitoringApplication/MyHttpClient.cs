using System;
using System.Net.Http;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// <see cref="HttpClient"/>クラスの一部を実装したクラス
    /// このクラスを介してHttpClientを利用する
    /// </summary>
    class MyHttpClient : IMyHttpClient
    {
        private HttpClient httpClient = new HttpClient();

        public MyHttpClient()
        {
            // タイムアウト時間を60秒に設定
            httpClient.Timeout = TimeSpan.FromMilliseconds(60000);
        }

        public void Dispose()
        {
            httpClient.Dispose();
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
