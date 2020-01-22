using System;
using System.Net.Http;

namespace SeatMonitoringApplication
{
    class MyHttpClient : IMyHttpClient
    {
        private HttpClient httpClient = new HttpClient();
        private TimeSpan timeout;
        public TimeSpan Timeout
        {
            get => timeout;
            set => timeout = httpClient.Timeout = value;
        }

        public HttpResponseMessage GetAsync(string requestUri)
        {
            return httpClient.GetAsync(requestUri).Result;
        }
    }
}
