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
        // not used because switch case only accept constant variables
        private readonly int CLIMB_LVL_1_INDEX = 0;
        private readonly int CLIMB_LVL_2_INDEX = 1;
        private readonly int CLIMB_LVL_3_INDEX = 2;
        private readonly int GIVE_CLIMB_LVL_2_INDEX = 0;
        private readonly int GIVE_CLIMB_LVL_3_INDEX = 1;

        



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

        /*
         * Switchboard operator for getting match Ranks
         * PRE: Rank type is provided
         * POST: Dictionary<String,double> type
         * Used for populating the listView in Rankings Page
         */
        public Dictionary<String,double> getRank(MatchFormat.CHOOSE_RANK_TYPE x)
        {
            Console.WriteLine(x);
            switch (x)
            {
                case MatchFormat.CHOOSE_RANK_TYPE.pick1:
                    return getPickAvgData((int)MatchFormat.ACTION.pick1);
                case MatchFormat.CHOOSE_RANK_TYPE.pick2:
                    return getPickAvgData((int)MatchFormat.ACTION.pick2);
                case MatchFormat.CHOOSE_RANK_TYPE.drop1:
                    return getDropData((int)MatchFormat.ACTION.drop1);
                case MatchFormat.CHOOSE_RANK_TYPE.drop2:
                    return getDropData((int)MatchFormat.ACTION.drop2);
                case MatchFormat.CHOOSE_RANK_TYPE.drop3:
                    return getDropData((int)MatchFormat.ACTION.drop3);
                case MatchFormat.CHOOSE_RANK_TYPE.climb:
                    return getClimbData();
                case MatchFormat.CHOOSE_RANK_TYPE.overallRank:
                    return getOverallData();
                default:
                    Console.WriteLine("ERROR: WRONG RANK TYPE");
                    return new Dictionary<string, double>();
            }
            //return new Dictionary<string, double>();
            //Enum.GetNames(typeof(MatchFormat.ACTION)).Length;
        }

        // Pre: climbData returns a dictionary that consist of every team appeared
        public Dictionary<string, double> getOverallData()
        {
            Dictionary<string, double> data = new Dictionary<string, double>();
            Dictionary<string, double> dropData1 = getDropData((int)MatchFormat.CHOOSE_RANK_TYPE.drop1);
            Dictionary<string, double> dropData2 = getDropData((int)MatchFormat.CHOOSE_RANK_TYPE.drop2);
            Dictionary<string, double> dropData3 = getDropData((int)MatchFormat.CHOOSE_RANK_TYPE.drop3);
            Dictionary<string, double> cargoData = getPickAvgData((int)MatchFormat.CHOOSE_RANK_TYPE.pick1);
            Dictionary<string, double> hatcherData = getPickAvgData((int)MatchFormat.CHOOSE_RANK_TYPE.pick2);
            Dictionary<string, double> climbData = getClimbData();
            foreach (KeyValuePair<string, double> entry in climbData)
            {
                double point = 0;
                if (dropData1.ContainsKey(entry.Key))
                {
                    point += dropData1[entry.Key] * ConstantVars.DROP_1_MULTIPLIER;
                }
                if (dropData2.ContainsKey(entry.Key))
                {
                    point += dropData2[entry.Key] * ConstantVars.DROP_2_MULTIPLIER;
                }
                if (dropData3.ContainsKey(entry.Key))
                {
                    point += dropData3[entry.Key] * ConstantVars.DROP_3_MULTIPLIER;
                }
                if (cargoData.ContainsKey(entry.Key))
                {
                    point += cargoData[entry.Key] * ConstantVars.CARGO_MULTIPLIER;
                }
                if (hatcherData.ContainsKey(entry.Key))
                {
                    point += hatcherData[entry.Key] * ConstantVars.HATCHER_MULTIPLIER;
                }
                point += climbData[entry.Key] * ConstantVars.CLIMB_MULTIPLIER;
                data.Add(entry.Key, point);
            }

            return data;
        }

        //Returns average data for drop level passed through (enum int is passed through sortType)
        public Dictionary<string,double> getDropData(int levelEnum)
        {
            Dictionary<string, double> totalData = new Dictionary<string, double>();
            Dictionary<string, int> numsData = new Dictionary<string, int>();
            try
            {
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
                    for (int i = 1; i < reps; i++)
                    {
                        if ((int)match["TE" + i + "_1"] == levelEnum)
                        {
                            if (((int)match["TE" + (i - 1) + "_1"] != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + (i - 1) + "_1"] != (int)MatchFormat.ACTION.dropNone))
                            {
                                int doTime = (int)match["TE" + (i) + "_0"] - (int)match["TE" + (i - 1) + "_0"];
                                if ((int)match["TE" + (i) + "_0"] <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
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
            }
            catch (System.NullReferenceException) {

            }
            Dictionary<string, double> pushData = new Dictionary<string, double>();
            foreach (var data in totalData)
            {
                pushData.Add(data.Key, data.Value / numsData[data.Key]);
            }
            return pushData;
        }

        //Returns average data for the climb parameters
        public Dictionary<String, double> getClimbData()
        {
            Dictionary<string, double> totalPoint = new Dictionary<string, double>();
            Dictionary<string, double> amountOfMatch = new Dictionary<string, double>();
            try
            {
                foreach (var match in fullData)
                {
                    int point = 0;
                    if ((bool)match["climb"])
                    {
                        if ((bool)match["needAstClimb"])
                        {
                            switch ((int)match["climbLvl"])
                            {
                                case 1:
                                    point += (int)ConstantVars.PTS_NEED_HELP_LVL_2;
                                    break;
                                case 2:
                                    point += (int)ConstantVars.PTS_NEED_HELP_LVL_3;
                                    System.Diagnostics.Debug.WriteLine("dam 1");
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
                                case 0:
                                    point += (int)ConstantVars.PTS_SELF_LVL_1;
                                    break;
                                case 1:
                                    point += (int)ConstantVars.PTS_SELF_LVL_2;
                                    break;
                                case 2:
                                    point += (int)ConstantVars.PTS_SELF_LVL_3;
                                    System.Diagnostics.Debug.WriteLine("dam 2");
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
                            case 0:
                                point += (int)ConstantVars.PTS_HELPED_LVL_2;
                                break;
                            case 1:
                                point += (int)ConstantVars.PTS_HELPED_LVL_3;
                                System.Diagnostics.Debug.WriteLine("dam 3");
                                break;
                            default:
                                point += 0;
                                break;
                        }
                    }

                    if (totalPoint.ContainsKey(match["team"].ToString()))
                    {
                        totalPoint[match["team"].ToString()] += point;
                        amountOfMatch[match["team"].ToString()] += 1;
                    }
                    else
                    {
                        totalPoint.Add(match["team"].ToString(), point);
                        amountOfMatch.Add(match["team"].ToString(), 1);
                    }
                }
            }
            catch (System.NullReferenceException)
            {

            }
            Dictionary<string, double> data = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> entry in totalPoint)
            {
                data.Add(entry.Key, entry.Value / amountOfMatch[entry.Key]);
            }
            return data;
        }

        //Returns average data for pick type passed through (enum int is passed through sortType)
        public Dictionary<string, double> getPickAvgData(int sortType)
        {
            Dictionary<string, double> totalData = new Dictionary<string, double>();
            Dictionary<string, int> numsData = new Dictionary<string, int>();
            Dictionary<string, double> pushData = new Dictionary<string, double>();
            try
            {
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
            }
            catch (System.NullReferenceException)
            {

            }
            foreach (var data in totalData)
            {
                pushData.Add(data.Key, data.Value / numsData[data.Key]);
            }
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
                    System.Diagnostics.Debug.WriteLine("Caught NullRepEx for ranker JObject");
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
