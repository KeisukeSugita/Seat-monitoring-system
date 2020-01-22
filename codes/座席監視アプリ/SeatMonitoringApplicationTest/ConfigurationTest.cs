using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringApplication;
using Configuration = SeatMonitoringApplication.Configuration;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class ConfigurationTest
    {
        /// <summary>
        /// 初期化していない状態でIpAddressをGetしたとき、正常に取得できる
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("testAddress", Configuration.Instance.IpAddress);
        }

        /// <summary>
        /// 初期化した状態でIpAddressをGetしたとき、正常に取得できる
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            var ipAddress = Configuration.Instance.IpAddress;
            Assert.AreEqual("testAddress", Configuration.Instance.IpAddress);
        }

        /// <summary>
        /// App.configファイルにIpAddressが設定されていない時、例外が発生する
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            try
            {
                var ipAddress = Configuration.Instance.IpAddress;
            }
            catch(InvalidOperationException e)
            {
                Assert.AreEqual("\"IpAddress\"が見つかりません。", e.Message);
                return;
            }

            Assert.Fail();
        }
    }
}
