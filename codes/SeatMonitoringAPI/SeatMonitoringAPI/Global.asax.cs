using AForge.Video.DirectShow;
using SeatMonitoringAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SeatMonitoringAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        protected void Application_Start()
        {
            using (var streamReader = new StreamReader(@"SeatMonitoringAPI\LinkCameraAndName.json", Encoding.UTF8))
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
