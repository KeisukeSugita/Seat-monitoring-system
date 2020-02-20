using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringApplication;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class SeatToastNotifierTest
    {
        /// <summary>
        /// 座席の状態取得に成功していた状態で、状態取得に成功した通知を受け取った場合のテスト
        /// </summary>
        [TestMethod]
        public void Notify_IsSucceededTrueToTrue()
        {
            var expectedSeatToasts = new List<(string, string)>();
            var resultSeatToasts = new List<(string, string)>();

            var callTimes = 0;

            var expectedXmlDocument1 = new XmlDocument();
            expectedXmlDocument1.LoadXml(new ToastContent()
            {
                Header = new ToastHeader("0001", "座席監視アプリ", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = $@"{Application.StartupPath}\Icons\離席アイコン.png",
                            HintCrop = ToastGenericAppLogoCrop.Default
                        },

                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = @"""佐藤　太郎""さんが離席しました。",
                            }
                        }
                    }
                }
            }.GetContent());

            var expectedXmlDocument2 = new XmlDocument();
            expectedXmlDocument2.LoadXml(new ToastContent()
            {
                Header = new ToastHeader("0001", "座席監視アプリ", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = $@"{Application.StartupPath}\Icons\在席アイコン.png",
                            HintCrop = ToastGenericAppLogoCrop.Default
                        },

                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = @"""さとう　たろう""さんは在席しています。",
                            }
                        }
                    }
                }
            }.GetContent());
            expectedSeatToasts.Add((Application.ExecutablePath, expectedXmlDocument1.GetXml()));
            expectedSeatToasts.Add((Application.ExecutablePath, expectedXmlDocument2.GetXml()));

            var toastNotificationManagerMock = new Mock<IToastNotificationManagerWrapper>();
            toastNotificationManagerMock
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<XmlDocument>()))
                .Callback<string, XmlDocument>((x, y) =>
                {
                    resultSeatToasts.Add((x, y.GetXml()));
                    callTimes++;
                });

            // 1回目に通知されるリスト
            var firstTimeSeats = new List<Seat>();
            firstTimeSeats.Add(new Seat("佐藤　太郎", Seat.SeatStatus.Exists));
            firstTimeSeats.Add(new Seat("さとう　たろう", Seat.SeatStatus.NotExists));
            firstTimeSeats.Add(new Seat("サトウ　タロウ", Seat.SeatStatus.Exists));

            // 2回目に通知されるリスト
            var secondTimeSeats = new List<Seat>();
            secondTimeSeats.Add(new Seat("佐藤　太郎", Seat.SeatStatus.NotExists));
            secondTimeSeats.Add(new Seat("さとう　たろう", Seat.SeatStatus.Exists));
            secondTimeSeats.Add(new Seat("サトウ　タロウ", Seat.SeatStatus.Exists));

            var seatToastNotifier = new SeatToastNotifier(toastNotificationManagerMock.Object, Application.ExecutablePath);
            var privateObject = new PrivateObject(seatToastNotifier);

            seatToastNotifier.Notify(firstTimeSeats, true);
            Assert.AreEqual(firstTimeSeats, privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 0);
            Assert.AreEqual(callTimes, 0);

            seatToastNotifier.Notify(secondTimeSeats, true);
            Assert.AreEqual(secondTimeSeats, privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 2);
            Assert.AreEqual(callTimes, 2);

            CollectionAssert.AreEqual(expectedSeatToasts, resultSeatToasts);
        }

        /// <summary>
        /// 座席の状態取得に成功していた状態で、状態取得に失敗した通知を受け取った場合のテスト
        /// </summary>
        [TestMethod]
        public void Notify_IsSucceededTrueToFalse()
        {
            var expectedSeatToasts = new List<(string, string)>();
            var resultSeatToasts = new List<(string, string)>();

            var callTimes = 0;

            var expectedXmlDocument1 = new XmlDocument();
            expectedXmlDocument1.LoadXml(new ToastContent()
            {
                Header = new ToastHeader("0001", "座席監視アプリ", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = $@"{Application.StartupPath}\Icons\サーバ接続エラーアイコン.png",
                            HintCrop = ToastGenericAppLogoCrop.Default
                        },

                        Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = @"サーバへの接続に失敗しました。",
                                }
                            }
                    }
                }
            }.GetContent());
            expectedSeatToasts.Add((Application.ExecutablePath, expectedXmlDocument1.GetXml()));

            var toastNotificationManagerMock = new Mock<IToastNotificationManagerWrapper>();
            toastNotificationManagerMock
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<XmlDocument>()))
                .Callback<string, XmlDocument>((x, y) =>
                {
                    resultSeatToasts.Add((x, y.GetXml()));
                    callTimes++;
                });

            // 1回目に通知されるリスト
            var firstTimeSeats = new List<Seat>();
            firstTimeSeats.Add(new Seat("佐藤　太郎", Seat.SeatStatus.Exists));
            firstTimeSeats.Add(new Seat("さとう　たろう", Seat.SeatStatus.NotExists));
            firstTimeSeats.Add(new Seat("サトウ　タロウ", Seat.SeatStatus.Exists));

            // 2回目に通知されるリスト
            var secondTimeSeats = new List<Seat>();
            secondTimeSeats = null;

            var seatToastNotifier = new SeatToastNotifier(toastNotificationManagerMock.Object, Application.ExecutablePath);
            var privateObject = new PrivateObject(seatToastNotifier);

            seatToastNotifier.Notify(firstTimeSeats, true);
            Assert.AreEqual(firstTimeSeats, privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 0);
            Assert.AreEqual(callTimes, 0);

            // サーバ接続エラーのAssert
            seatToastNotifier.Notify(secondTimeSeats, false);
            Assert.IsNull(privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 1);
            Assert.AreEqual(callTimes, 1);

            CollectionAssert.AreEqual(expectedSeatToasts, resultSeatToasts);
        }

        /// <summary>
        /// 座席の状態取得に失敗していた状態で、状態取得に成功した通知を受け取った場合のテスト
        /// </summary>
        [TestMethod]
        public void Notify_IsSucceededFalseToTrue()
        {
            var resultSeatToasts = new List<(string, string)>();

            var callTimes = 0;

            var toastNotificationManagerMock = new Mock<IToastNotificationManagerWrapper>();
            toastNotificationManagerMock
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<XmlDocument>()))
                .Callback<string, XmlDocument>((x, y) =>
                {
                    resultSeatToasts.Add((x, y.GetXml()));
                    callTimes++;
                });

            // 1回目に通知されるリスト
            var firstTimeSeats = new List<Seat>();
            firstTimeSeats = null;

            // 2回目に通知されるリスト
            var secondTimeSeats = new List<Seat>();
            secondTimeSeats.Add(new Seat("佐藤　太郎", Seat.SeatStatus.Exists));
            secondTimeSeats.Add(new Seat("さとう　たろう", Seat.SeatStatus.NotExists));
            secondTimeSeats.Add(new Seat("サトウ　タロウ", Seat.SeatStatus.Exists));

            var seatToastNotifier = new SeatToastNotifier(toastNotificationManagerMock.Object, Application.ExecutablePath);
            var privateObject = new PrivateObject(seatToastNotifier);

            seatToastNotifier.Notify(firstTimeSeats, false);
            Assert.IsNull(privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 0);
            Assert.AreEqual(callTimes, 0);

            // サーバ接続エラーから復帰した場合のAssert
            seatToastNotifier.Notify(secondTimeSeats, true);
            Assert.AreEqual(secondTimeSeats, privateObject.GetField("pastSeats"));
            Assert.AreEqual(resultSeatToasts.Count, 0);
            Assert.AreEqual(callTimes, 0);
        }
    }
}
