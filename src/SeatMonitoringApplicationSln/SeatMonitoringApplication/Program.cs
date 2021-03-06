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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch(ConfigurationErrorsException e)
            {
                // IPアドレスを読み込めなかった場合
                MessageBox.Show(e.Message, "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
