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
    /// トースト通知に必要なクラスをラップするインターフェース
    /// </summary>
    public interface IToastNotificationManagerWrapper
    {
        void Show(string applicationId, XmlDocument xmlDocument);
    }
}
