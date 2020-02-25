using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Setup
{
    class Setup
    {
        public static void Main()
        {
            // ショートカットそのもののパス
            string shortcutPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\座席監視アプリ.lnk";
            // ショートカットのリンク先(起動するプログラムのパス)
            string targetPath = Application.StartupPath + @"\SeatMonitoringApplication.exe";

            // WshShellを作成
            WshShell wshShell = new WshShell();
            // ショートカットのパスを指定して、WshShortcutを作成
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutPath);
            // リンク先
            shortcut.TargetPath = targetPath;
            // 作業フォルダ
            shortcut.WorkingDirectory = Application.StartupPath;

            try
            {
                // ショートカットを作成
                shortcut.Save();
            }
            finally
            {
                // 参照の解放
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wshShell);
            }
        }
    }
}
