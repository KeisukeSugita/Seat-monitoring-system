using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// <see cref="ToastNotificationManager"/>
    /// <see cref="ToastNotifier"/>
    /// 上記クラスをラップしたクラス
    /// </summary>
    public interface IToastNotificationManagerWrapper
    {
        /// <summary>
        /// <see cref="ToastNotifier"/>のShowメソッドと
        /// <see cref="ToastNotificationManager"/>のCreateToastNotifierメソッド
        /// をラップしたメソッド
        /// </summary>
        /// <param name="applicationId">通知を発行するアプリのApplicationID</param>
        /// <param name="xmlDocument">トースト通知の内容が埋め込まれた<see cref="ToastNotification">のインスタンス</param>
        void Show(string applicationId, ToastNotification tastNotification);
    }
}
