using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    public interface IMyHttpClient
    {
        TimeSpan Timeout { get; set; }

        HttpResponseMessage GetAsync(string requestUri);
    }
}
