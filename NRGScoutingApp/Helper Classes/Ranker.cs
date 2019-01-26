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

        public void setData(String data)
        {
            fullData = getJSON(data);
        }

        public Dictionary<string,double> getLevelData(int levelEnum)
        {

        }

        public Dictionary<String, double> getClimbData()
        {
            Dictionary<string, double> totalPoint = new Dictionary<string, double>();
            Dictionary<string, int> amountOfMatch = new Dictionary<string, int>();
            foreach (var match in fullData)
            {
                int point = 0;
                if ((bool)match["climb"])
                {
                    if ((bool)match["needAstClimb"])
                    {
                        switch ((int)match["climbLvl"])
                        {
                            case 2:
                                point += (int)ConstantVars.PTS_NEED_HELP_LVL_2;
                                break;
                            case 3:
                                point += (int)ConstantVars.PTS_NEED_HELP_LVL_2;
                                break;
                            default:
                                point += 0;
                                break;
                        }
                    }
                    else
                    {
                        switch ((int)match["climbLvl"])
                        {
                            case 1:
                                point += (int)ConstantVars.PTS_SELF_LVL_1;
                                break;
                            case 2:
                                point += (int)ConstantVars.PTS_SELF_LVL_2;
                                break;
                            case 3:
                                point += (int)ConstantVars.PTS_SELF_LVL_3;
                                break;
                            default:
                                point += 0;
                                break;
                        }
                    }
                }
                if ((bool)match["giveAstClimb"])
                {
                    switch ((int)match["giveAstClimbLvl"])
                    {
                        case 2:
                            point += (int)ConstantVars.PTS_HELPED_LVL_2;
                            break;
                        case 3:
                            point += (int)ConstantVars.PTS_HELPED_LVL_3;
                            break;
                        default:
                            point += 0;
                            break;
                    }
                }

                if (totalPoint.ContainsKey(match["team"].ToString()))
                {
                    totalPoint["team"] += point;
                    amountOfMatch["team"] += 1;
                }
                else
                {
                    totalPoint.Add(match["team"].ToString(), point);
                    amountOfMatch["team"] = 1;
                }
            }
            Dictionary<string, double> data = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> entry in totalPoint)
            {
                data[entry.Key] = totalPoint[entry.Key] / amountOfMatch[entry.Key];
            }
            return data;
        }


        //Returns average data for type passed through (enum int is passed through sortType)
        public Dictionary<string, double> getPickAvgData(int sortType)
        {
            Dictionary<string, double> totalData = new Dictionary<string, double>();
            Dictionary<string, int> numsData = new Dictionary<string, int>();
            Dictionary<string, double> pushData = new Dictionary<string, double>();
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
                        if (((int)match["TE" + (i + 1) + "_1"] != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + (i + 1) + "_1"] != (int)MatchFormat.ACTION.dropNone))
                        {
                            int doTime = (int)match["TE" + (i + 1) + "_0"] - (int)match["TE" + i + "_0"];
                            if ((int)match["TE" + (i + 1) + "_0"] <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
                            {
                                doTime /= 2;
                            }
                            if (totalData.ContainsKey(match["team"].ToString()))
                            {
                                totalData[match["team"].ToString()] += doTime;
                                numsData[match["team"].ToString()]++;
                            }
                            else
                            {
                                totalData.Add(match["team"].ToString(), doTime);
                                numsData.Add(match["team"].ToString(), 1);
                            }
                        }
                    }
                }
            }

            foreach (var data in totalData)
            {
                pushData.Add(data.Key, data.Value / numsData[data.Key]);
            }
            Console.WriteLine(pushData);
            return pushData;
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
                tempJSON = new JObject(new JProperty("Matches"));
            }
            return (JArray)tempJSON["Matches"];
        }
    }
}
