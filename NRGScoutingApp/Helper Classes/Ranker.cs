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
        public void getJSON() {
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                try
                {
                    x = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Caught NullRepEx for populateMatchesList");
                    x = new JObject();
                }
            }
            else
            {
                x = new JObject();
            }
        }
    }
}
