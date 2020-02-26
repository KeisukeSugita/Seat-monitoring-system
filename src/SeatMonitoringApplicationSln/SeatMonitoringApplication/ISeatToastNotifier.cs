using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// SeatMonitoringApplication用のトースト通知を行うインターフェース
    /// </summary>
    public interface ISeatToastNotifier
    {
        /// <summary>
        /// ・状態が変化している座席についてトースト通知を送信するメソッド
        /// ・1回目の呼び出し時にはトースト通知は行われず、2回も以降の呼び出し時に前回の呼び出し時の引数との差をトースト通知する
        /// ・サーバ接続エラーが発生した場合、サーバ接続エラーのトースト通知を送信する
        /// ・サーバ接続エラーから復帰した場合、復帰したことを伝えるトースト通知を送信し、
        /// 　以降の呼び出し時に前回の呼び出し時の引数との差をトースト通知する
        /// </summary>
        /// <param name="currentSeats">監視座席名とその状態のリスト</param>
        /// <param name="isSucceeded">監視座席の状態取得の可否</param>
        void Notify(List<Seat> currentSeats, bool isSucceeded);
    }
}
