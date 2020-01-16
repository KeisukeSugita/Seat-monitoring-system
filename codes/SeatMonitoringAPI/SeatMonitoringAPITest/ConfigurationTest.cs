﻿using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class ConfigurationTest
    {
        // Configurationが初期化されていない状態でInstanseプロパティをGetしたとき、例外が発生する
        [TestMethod]
        public void GetInstance_YetCallInitialize_ExceptionThrow()
        {
            try
            {
                var configuration = Configuration.Instance.SeatDefinitions;
            }
            catch(InvalidOperationException e)
            {
                Assert.AreEqual(e.Message, "Configurationが初期化されていません。");
                return;
            }

            Assert.Fail();
        }

        // Configurationが初期化されていない状態でInitializeメソッドを実行したとき、Instanceプロパティが初期化される
        [TestMethod]
        public void Initiarlize_YetCallInitialize_Initializing()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""Name"":""スギタ ケイスケ""}]")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                Configuration.Initialize(streamReader);
                var configuration = Configuration.Instance.SeatDefinitions;
                Assert.AreEqual(configuration[0].Moniker, "usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002");
                Assert.AreEqual(configuration[0].Name, "杉田 圭輔");
                Assert.AreEqual(configuration[1].Moniker, "usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003");
                Assert.AreEqual(configuration[1].Name, "Keisuke Sugita");
                Assert.AreEqual(configuration[2].Moniker, "usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004");
                Assert.AreEqual(configuration[2].Name, "スギタ ケイスケ");
            }
        }

        // Configurationが初期化されている状態でInitializeメソッドを実行したとき、例外が発生する
        [TestMethod]
        public void Initialize_AlreadyCallInitialize_ExceptionThrow()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""Name"":""スギタ ケイスケ""}]")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Configuration.Initialize(streamReader);
                    Configuration.Initialize(streamReader);
                }
                catch(InvalidOperationException e)
                {
                    Assert.AreEqual(e.Message, "Configurationは既に初期化されています。");
                    return;
                }

                Assert.Fail();
            }
        }

        // ConfigurationのInitializeメソッドにJson形式ではないStreamを渡して実行したとき、例外が発生する
        [TestMethod]
        public void Initialize_InvalidFileType_ExceptionThrow()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""杉田 圭輔""
""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Keisuke Sugita""
""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""スギタ ケイスケ""")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Configuration.Initialize(streamReader);
                }
                catch(InvalidOperationException e)
                {
                    Assert.AreEqual(e.Message, "Json形式ではないファイルが読み込まれました。");
                    return;
                }

                Assert.Fail();
            }
        }

        // ConfigurationのInitializeメソッドにキーが"Moniker"と"Name"ではないJson形式のStreamを渡して実行したとき、例外が発生する
        [TestMethod]
        public void Initialize_InvalidJsonKeys_ExceptionThrow()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""モニカ"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""名前"":""杉田 圭輔""},
{""モニカ"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""名前"":""Keisuke Sugita""},
{""モニカ"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""名前"":""スギタ ケイスケ""}]")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Configuration.Initialize(streamReader);
                }
                catch (InvalidOperationException e)
                {
                    Assert.AreEqual(e.Message, "Jsonファイルのキーが不正です。");
                    return;
                }

                Assert.Fail();
            }
        }
    }
}
