using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Data = System.Collections.Generic.KeyValuePair<string, int>;
using System.Collections;
using System.Runtime.InteropServices;

namespace NRGScoutingApp
{
    public class Ranker
    {
        private JArray fullData;
        //PRE: data is in JSON Format
        public Ranker(String data)
        {
            fullData = getJSON(data);
        }

        //Returns average data for type passed through (enum int is passed through sortType)
        public Dictionary<string, double> getPickAvgData(int sortType)
        {
            Dictionary<string, double> avgData = new Dictionary<string, double>();
            foreach (var match in fullData)
            {
                int reps;
                try
                {
                    reps = (int)match["numEvents"];
                }
                catch (JsonException)
                {
                    reps = 0;
                }
                for (int i = 0; i < reps; i++)
                {
                    if ((int)match["TE" + i + "_1"] == sortType && i != reps - 1)
                    {
                        if (((int)match["TE" + i + "_1"] != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + i + "_1"] != (int)MatchFormat.ACTION.dropNone))
                        {
                            int doTime = (int)match["TE" + (i + 1) + "_0"] - (int)match["TE" + i + "_0"];
                            if ((int)match["TE" + (i + 1) + "_0"] <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
                            {
                                doTime /= 2;
                            }
                            if (avgData.ContainsKey(match["team"].ToString()))
                            {
                                avgData[match["team"].ToString()] = (avgData[match["team"].ToString()] + doTime) / 2;
                            }
                            else
                            {
                                avgData.Add(match["team"].ToString(), doTime);
                            }
                        }
                    }
                }
            }
            Console.WriteLine(avgData);
            return avgData;
        }

        //Checks if the JObject contains any values
        public bool isRankNeeded()
        {
            return fullData.Count > 0;
        }

        //Returns JObject for matches input string
        private JArray getJSON(String input)
        {
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
            return (JArray)tempJSON["Matches"];
        }
    }
}
