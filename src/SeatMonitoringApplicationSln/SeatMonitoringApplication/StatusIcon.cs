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
    public class StatusIcon : IStatusIcon
    {
        /// <summary>
        /// 名前に対応するアイコンへの絶対パスを返すメソッド
        /// </summary>
        /// <param name="iconName">アイコンの名前</param>
        /// <returns>対応するアイコンへの絶対パス</returns>
        public string GetIcon(string iconName)
        {
            return $@"{Application.StartupPath}\{iconName}アイコン.png";
        }
    }
}
