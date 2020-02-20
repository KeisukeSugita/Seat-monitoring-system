﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// ToastNotificationManagerクラス、ToastNotifierクラス、ToastNotificationクラスをラップしたクラス
    /// </summary>
    class ToastNotificationManagerWrapper :IToastNotificationManagerWrapper
    {
        /// <summary>
        /// ToastNotifierクラスのShowメソッドと
        /// ToastNotificationManagerクラスのCreateToastNotifierメソッド
        /// をラップしたメソッド
        /// </summary>
        /// <param name="applicationId">通知を発行するアプリのApplicationID</param>
        /// <param name="xmlDocument">トースト通知の内容が記述されたXml</param>
        public void Show(string applicationId, XmlDocument xmlDocument)
        {
            ToastNotificationManager.CreateToastNotifier(applicationId).Show(new ToastNotification(xmlDocument));
        }
    }
}
