using System;
using System.Net.Http;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// <see cref="IMyHttpClient"/>
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
        /// <see cref="IMyHttpClient.GetAsync(string)"/>
        /// </summary>
        public HttpResponseMessage GetAsync(string requestUri)
        {
            return httpClient.GetAsync(requestUri).Result;
        }

    }
}
