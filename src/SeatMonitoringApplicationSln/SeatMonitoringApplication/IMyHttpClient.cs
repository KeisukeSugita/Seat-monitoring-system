using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// テスト用に<see cref="HttpClient"/>をラップするインターフェース
    /// </summary>
    public interface IMyHttpClient : IDisposable
    {
        /// <summary>
        /// HttpClientクラスのGetAsyncメソッドの結果を返すメソッド
        /// </summary>
        /// <param name="requestUri">HTTPリクエストを送信するURI</param>
        /// <returns>レスポンスメッセージ</returns>
        HttpResponseMessage GetAsync(string requestUri);
    }
}
