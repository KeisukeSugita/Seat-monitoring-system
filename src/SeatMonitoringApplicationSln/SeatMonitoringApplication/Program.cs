﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeatMonitoringApplication
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Hostの読み込み
            string host = ConfigurationManager.AppSettings["Host"];
            if (host == null)
            {
                MessageBox.Show(@"""Host""が読み込めませんでした", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var httpClient = new MyHttpClient())
            {
                var toastNotificationManager = new ToastNotificationManagerWrapper();
                var seatToastNotifier = new SeatToastNotifier(toastNotificationManager, Application.ExecutablePath);
                var periodicNotifier = new PeriodicNotifier(new SeatMonitoringApiClient(host, httpClient));

                // 通知設定の読み込み
                if (bool.TryParse(ConfigurationManager.AppSettings["IsNotify"], out bool isNotify))
                {
                    // 通知設定がtrueならトースト通知を通知先として追加する
                    if (isNotify)
                    {
                        periodicNotifier.Destination += seatToastNotifier.Notify;
                    }
                }
                else
                {
                    MessageBox.Show(@"""isNotify""が読み込めませんでした", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(periodicNotifier));
            }
        }
    }
}
