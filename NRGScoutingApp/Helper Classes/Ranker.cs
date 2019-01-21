using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Data = System.Collections.Generic.KeyValuePair<string, double>;

using System.Collections;
using System.Runtime.InteropServices;

namespace NRGScoutingApp
{
    public class Ranker
    {
        JObject fullData;
        //PRE: data is in JSON Format
        public Ranker(String data) {
            fullData = getJSON(data);
        }

        //Checks if the JObject contains any values
        public bool isRankNeeded() {
            return fullData.Count > 0;
        }

        public JObject getJSON(String input) {
            JObject tempJSON;
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                try
                {
                    tempJSON = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Caught NullRepEx for ranker JObject");
                    tempJSON = new JObject();
                }
            }
            else
            {
                tempJSON = new JObject();
            }
            return tempJSON;
        }
    }
}
