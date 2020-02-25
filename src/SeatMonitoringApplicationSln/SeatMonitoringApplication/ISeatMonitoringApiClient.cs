using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// SeatMonitoringAPIのメソッドを呼び出すインターフェース
    /// </summary>
    public interface ISeatMonitoringApiClient
    {
        /// <summary>
        /// GetSeatsメソッドを呼び出し、結果をアプリが解釈しやすい形に変換するメソッド
        /// </summary>
        /// <returns>取得した座席情報のリスト</returns>
        List<Seat> GetSeats();
    }
}
