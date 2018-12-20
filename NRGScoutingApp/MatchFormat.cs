using System;
using Data = System.Collections.Generic.KeyValuePair<string, string>;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace NRGScoutingApp
{
    public class MatchFormat
    {
        public MatchFormat()
        {
        }
        public static String[,] mainStringToSplit(String matchData){
            if (!matchData.Contains("|") || !matchData.Contains("||"))
            {
                return null;
            }
            else{
                String[] matches = matchData.Split('&');
                String[,] splitData = new String[matches.Length, 2];
                for (int i = 0; i < matches.Length; i++){
                    if(String.IsNullOrWhiteSpace(matches[i])){}
                    else{
                        String[] temp = matches[i].Split('*');
                        splitData[i, 0] = temp[0]; // Match Parameters
                        splitData[i, 1] = temp[1]; // Match Events
                        Console.WriteLine(splitData[i, 0]);
                        Console.WriteLine(splitData[i, 1]);
                    }
                }
                return splitData;
            }
        }

        public static List<Data> matchesToSimpleData(String[,] inputString){
            List <Data>  data = new List<Data>();
            if(inputString == null){
                return null;
            }
            for (int i = 0; i < inputString.GetLength(0); i++)
            {
                if (String.IsNullOrWhiteSpace(inputString[i, 0]) || String.IsNullOrWhiteSpace(inputString[i, 1])) { }

                else
                {

               }
            }
                      
            return data;
        }
    }
}
