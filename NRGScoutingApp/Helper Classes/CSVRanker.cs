using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NRGScoutingApp
{
    public class CSVRanker
    {
        //ORDERL Team,Match Num,Side,Avg. Hatch,Avg. Cargo,Climb,Lvl1,Lvl2,Lvl3,Cargoship
        private JObject match;
        public string matchCalc(JObject match)
        {
            this.match = match;
            String total = this.match["team"] + "," +
            this.match["matchNum"] + "," +
            MatchFormat.matchSideFromEnum((int)this.match["side"]) + ",";
            total += pickCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick1) + "," +
                pickCalc((int)MatchFormat.CHOOSE_RANK_TYPE.pick2) + ",";

            total += climbCalc() + ",";

            total += dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop1) + "," +
            dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop2) + "," +
            dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop3) + "," +
            dropCalc((int)MatchFormat.CHOOSE_RANK_TYPE.drop4);
            return total;
        }

        private double climbCalc()
        {
            int point = 0;
            try
            {
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
            }
            catch (System.NullReferenceException)
            {

            }
            return point;
        }

        private double dropCalc(int levelEnum)
        {
            double totalData = 0;
            int reps = 0;
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
                            int doTime = Convert.ToInt32(match["TE" + (i) + "_0"]) - Convert.ToInt32(match["TE" + (i - 1) + "_0"]);
                            if (Convert.ToInt32(match["TE" + (i) + "_0"]) <= ConstantVars.AUTO_LENGTH && !(bool)match["autoOTele"])
                            {
                                doTime /= 2;
                            }
                            totalData += doTime;
                            reps++;
                        }
                    }
                }

            }
            catch (System.NullReferenceException)
            {

            }
            if (reps != 0)
            {
                return totalData / reps;
            }
            else
            {
                return 0;
            }



        }

        private double pickCalc(int sortType)
        {
            double total = 0;
            int reps = 0;
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
                Console.WriteLine(reps);
                for (int i = 0; i < reps; i++)
                {
                    Console.WriteLine(match["TE" + i + "_1"]);
                    Convert.ToInt32(match["TE" + i + "_1"]);
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
                            reps++;
                        }
                    }
                }
            }
            catch (System.NullReferenceException)
            {

            }
            if (reps != 0)
            {
                return total / reps;
            }
            else
            {
                return 0;
            }
        }
    }
}