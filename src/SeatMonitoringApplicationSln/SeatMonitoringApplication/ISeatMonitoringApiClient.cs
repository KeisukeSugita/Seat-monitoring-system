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
        /// SeatMonitoringAPIのGETを呼び出し、結果を<see cref="Seat"/>のリストに変換して返すメソッド
        /// </summary>
        /// <returns>取得した座席情報のリスト</returns>
        List<Seat> GetSeats();
    }
}
