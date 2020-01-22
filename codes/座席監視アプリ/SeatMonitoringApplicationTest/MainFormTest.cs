using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringApplication;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class MainFormTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var periodicNotifierMock = new Mock<IPeriodicNotifier>();

            var mainForm = new MainForm(periodicNotifierMock.Object);
            Assert.AreEqual("在席", mainForm.GetText(MainForm.ToolTipTexts.Exist));
            Assert.AreEqual("離席", mainForm.GetText(MainForm.ToolTipTexts.NotExist));
            Assert.AreEqual("不明", mainForm.GetText(MainForm.ToolTipTexts.Unknown));
            Assert.AreEqual("サーバ接続エラー", mainForm.GetText(MainForm.ToolTipTexts.ConectingError));
        }
    }
}
