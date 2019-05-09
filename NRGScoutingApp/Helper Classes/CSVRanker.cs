using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NRGScoutingApp
{
    public class CSVRanker
    {
        //ORDERL Team,Match Num,Side,Avg. Hatch,Num Hatch,Avg. Cargo,Num Cargo,Climb,Lvl1,Lvl2,Lvl3,Cargoship
        private JObject match;

        public string matchCalc(JObject match)
        {
            this.match = match;
            String total;
            try
            {
                total = this.match["team"] + "," +
                    this.match["matchNum"] + "," +
                    MatchFormat.matchSideFromEnum((int)this.match["side"]) + ","; //Side
            }
            catch
            {
                total = ",,,";
            }
            total += pickCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick1) + "," + //Hatch
                numCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick1) + "," +
                pickCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick2) + "," + //Cargo
                numCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick2) + ",";

            total += climbCalc() + ",";

            total += dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop1) + "," +
                dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop2) + "," +
                dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop3) + "," +
                dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop4);
            return total;
        }

        private String climbCalc()
        {
            int point = 0;
            String total = "";
            Boolean needHelp = (bool)match["needAstClimb"];
            Boolean gaveHelp = (bool)match["giveAstClimb"];
            try
            {
                if ((bool)match["climb"])
                {
                    switch ((int)match["climbLvl"])
                    {
                        case 0:
                            point = (int)ConstantVars.PTS_LVL_1_CLIMB;
                            break;
                        case 1:
                            point = (int)ConstantVars.PTS_LVL_2_CLIMB;
                            break;
                        case 2:
                            point = (int)ConstantVars.PTS_LVL_3_CLIMB;
                            break;
                        default:
                            point += 0;
                            break;
                    }
                }
            }
            catch (System.NullReferenceException)
            {

            }
            if (needHelp)
            {
                total += "NeedA";
            }
            else if (gaveHelp)
            {
                total += "A";
            }
            total += point;
            return total;
        }

        private String dropCalc(int levelEnum)
        {
            double totalData = 0;
            int reps = 0;
            int eventReps = 0;
            try
            {
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
                    if (Convert.ToInt32(match["TE" + i + "_1"]) == levelEnum)
                    {
                        if ((Convert.ToInt32(match["TE" + (i - 1) + "_1"]) != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + (i - 1) + "_1"] != (int)MatchFormat.ACTION.dropNone))
                        {
                            int doTime = Convert.ToInt32(match["TE" + i + "_0"]) - Convert.ToInt32(match["TE" + (i - 1) + "_0"]);
                            if (Convert.ToInt32(match["TE" + i + "_0"]) <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
                            {
                                doTime /= 2;
                            }
                            totalData += doTime;
                            eventReps++;
                        }
                    }
                }

            }
            catch (System.NullReferenceException)
            {

            }

            if (eventReps > 0)
            {
                return (totalData / eventReps).ToString();
            }
            else
            {
                return "";
            }

        }

        private double numCalc(int sortType)
        {
            int total = 0;
            int reps = 0;
            try
            {
                try
                {
                    reps = (int)match["numEvents"];
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e.StackTrace);
                    reps = 0;
                }
                for (int i = 0; i < reps; i++)
                {
                    if (Convert.ToInt32(match["TE" + i + "_1"]) == sortType && i != reps - 1)
                    {
                        if ((Convert.ToInt32(match["TE" + (i + 1) + "_1"]) != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + (i + 1) + "_1"] != (int)MatchFormat.ACTION.dropNone))
                        {
                            total++;
                        }
                    }
                }
            }
            catch (NullReferenceException) { }
            return total;
        }

        private String pickCalc(int sortType)
        {
            double total = 0;
            int reps = 0;
            int eventReps = 0;
            try
            {
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
                    if (Convert.ToInt32(match["TE" + i + "_1"]) == sortType && i != reps - 1)
                    {
                        if ((Convert.ToInt32(match["TE" + (i + 1) + "_1"]) != (int)MatchFormat.ACTION.startClimb) && ((int)match["TE" + (i + 1) + "_1"] != (int)MatchFormat.ACTION.dropNone))
                        {
                            int doTime = Convert.ToInt32(match["TE" + (i + 1) + "_0"]) - Convert.ToInt32(match["TE" + i + "_0"]);
                            if (Convert.ToInt32(match["TE" + (i + 1) + "_0"]) <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
                            {
                                doTime /= 2;
                            }
                            total += doTime;
                            eventReps++;
                        }
                    }
                }
            }
            catch (System.NullReferenceException)
            {

            }
            if (eventReps > 0)
            {
                return (total / eventReps).ToString();
            }
            else
            {
                return "";
            }
        }
    }
}