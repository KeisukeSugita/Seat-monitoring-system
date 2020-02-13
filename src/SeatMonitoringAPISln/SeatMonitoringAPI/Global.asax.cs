using AForge.Video.DirectShow;
using SeatMonitoringAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Configuration = SeatMonitoringAPI.Models.Configuration;

namespace SeatMonitoringAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        protected void Application_Start()
        {
            string settingFilePath = ConfigurationManager.AppSettings["SettingFilePath"];
            if (settingFilePath == null)
            {
                throw new ConfigurationErrorsException(@"ÉLÅ[""SettingFilePath""Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB");
            }
            using (var streamReader = new StreamReader(settingFilePath, Encoding.UTF8))
            {
                Configuration.Initialize(streamReader);
            }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
