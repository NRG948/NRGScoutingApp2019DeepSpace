using System;
using Data = System.Collections.Generic.KeyValuePair<string, string>;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace NRGScoutingApp
{
    public class MatchFormat
    {
        public class EntryParams
        {
            public String team { get; set; }
            public int matchNum { get; set; }
            public int side { get; set; }

            public bool crossedB { get; set; }
            public bool allyItem1 { get; set; }
            public bool allyItem2 { get; set; }
            public bool oppItem1 { get; set; }
            public bool oppItem2 { get; set; }

            public bool death { get; set; }
            public bool noClimb { get; set; }
            public bool soloClimb { get; set; }
            public bool giveAssistClimb { get; set; }
            public bool needAssistClimb { get; set; }
            public bool onClimbArea { get; set; }

            public int fouls { get; set; }
            public bool yellowCard { get; set; }
            public bool redCard { get; set; }
            public String comments { get; set; }

        }
        public enum DROP_TYPE{
           dropNone, 
           drop1, //Ally Scale
           drop2, //Ally Switch
           drop3,  // Opp. Switch
           dropItemCollector //Vault
        }
        public class EntryEvents {

        }
    }
}
