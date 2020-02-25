using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// <see cref="HttpClient"/>クラスの一部を実装するインターフェース
    /// このクラスを介してHttpClientを利用する
    /// </summary>
    public interface IMyHttpClient : IDisposable
    {
        /// <summary>
        /// HttpClientクラスのGetAsyncメソッドの結果を返すメソッド
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns>レスポンスメッセージ</returns>
        HttpResponseMessage GetAsync(string requestUri);
    }
}
