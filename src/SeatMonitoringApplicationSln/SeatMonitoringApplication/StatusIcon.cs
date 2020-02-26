using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 座席の状態を表すアイコンへのパスを取得するクラス
    /// </summary>
    public class StatusIcon
    {
        /// <summary>
        /// 在席アイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <returns>在席アイコンへの絶対パス</returns>
        static public string GetExistIcon()
        {
            return $@"{Application.StartupPath}\Icons\在席アイコン.png";
        }

        /// <summary>
        /// 離席アイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <returns>離席アイコンへの絶対パス</returns>
        static public string GetNotExistIcon()
        {
            return $@"{Application.StartupPath}\Icons\離席アイコン.png";
        }

        /// <summary>
        /// 状態取得失敗アイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <returns>状態取得失敗アイコンへの絶対パス</returns>
        static public string GetFailureIcon()
        {
            return $@"{Application.StartupPath}\Icons\状態取得失敗アイコン.png";
        }

        /// <summary>
        /// サーバ接続エラーアイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <returns>サーバ接続エラーアイコンへの絶対パス</returns>
        static public string GetErrorIcon()
        {
            return $@"{Application.StartupPath}\Icons\サーバ接続エラーアイコン.png";
        }

        /// <summary>
        /// サーバ接続エラー復帰アイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <returns>サーバ接続エラー復帰アイコンへの絶対パス</returns>
        static public string GetReturnFromErrorIcon()
        {
            return $@"{Application.StartupPath}\Icons\サーバ接続エラー復帰アイコン.png";
        }
    }
}
