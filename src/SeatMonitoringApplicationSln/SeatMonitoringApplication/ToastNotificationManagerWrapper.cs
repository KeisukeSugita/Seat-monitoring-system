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
    /// <see cref="IToastNotifier"/>
    /// </summary>
    class ToastNotificationManagerWrapper :IToastNotificationManagerWrapper
    {
        /// <summary>
        /// <see cref="IToastNotifier.Show(ToastNotification)"/>
        /// </summary>
        public void Show(string applicationId, XmlDocument xmlDocument)
        {
            ToastNotificationManager.CreateToastNotifier(applicationId).Show(new ToastNotification(xmlDocument));
        }
    }
}
