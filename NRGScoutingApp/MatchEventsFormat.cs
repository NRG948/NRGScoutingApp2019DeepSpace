using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Data = System.Collections.Generic.KeyValuePair<string, string>;

namespace NRGScoutingApp
{

    /* FORMAT OF EVENTS: (cubePicked:1000|cubeDroppedLocation0:5000||cubePicked:5500|cubeDropped:back||climbStart:139000||)
     * MUST RETURN Dictionary<string,int> or Dictionary<string,string>  
     *      PLEASE KEEP THIS IN MIND WHILE PASSING VALUES THROUGH THIS
    */
    public class MatchEventsFormat
    {

        public MatchEventsFormat() { }
    }
}
