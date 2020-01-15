using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class Configuration
    {
        private static Configuration instance = null;
        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("Configurationが初期化されていません。");
                }
                return instance;
            }
            private set
            {
            }
        }
        public ReadOnlyCollection<SeatDefinition> SeatDefinitions { get; private set; }

        private Configuration(StreamReader streamReader)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(streamReader.ReadToEnd());
            var seatDefinitions = new List<SeatDefinition>();

            int objNum = 0;
            foreach(var jsonObjElement in jsonObj)
            {
                string moniker = jsonObjElement.Moniker;
                string name = jsonObjElement.Name;
                seatDefinitions.Add(new SeatDefinition(moniker, name));
                objNum++;
                if (objNum == 10)
                {
                    break;
                }
            }
            SeatDefinitions = new ReadOnlyCollection<SeatDefinition>(seatDefinitions);
        }

        public static void Initialize(StreamReader streamReader)
        {
            instance = instance == null ? new Configuration(streamReader) : throw new InvalidOperationException("Configurationは既に初期化されています。");
        }
    }
}