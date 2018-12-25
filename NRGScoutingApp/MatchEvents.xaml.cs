using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Linq;
using Data = System.Collections.Generic.KeyValuePair<string, string>;
using Contacts;

namespace NRGScoutingApp
{
    public partial class MatchEvents : ContentPage
    {
        //StringFormat paramFormat = new StringFormat();
        public MatchEvents()
        {
            BindingContext = this;
            InitializeComponent();
        }

        public class EventListFormat{ //Constructor for Events View that Users see
            public String image { get; set; }
            public String eventName { get; set; }
            public String timeOccur { get; set; }
        }

        protected override void OnAppearing()
        {
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["tempMatchEvents"].ToString()))
            {
                listView.ItemsSource = sortedEventViewList((List<MatchFormat.Data>)App.Current.Properties["tempMatchEvents"]);
            }
        }

        public static List<EventListFormat> sortedEventViewList(List<MatchFormat.Data> matchData) {
            List<EventListFormat> listData = new List<EventListFormat>();
            for (int i = 0; i < matchData.Count; i++)
            {
                listData.Add(new EventListFormat
                {
                    image = returnEventImage(matchData[i].type),
                    eventName = returnEventText(matchData[i].type),
                    timeOccur = NewMatchStart.timeToString(matchData[i].time)
                });
            }
            return listData;
        }

        public static String returnEventText(int eventType) { 
            if (eventType < 0) {
                switch (eventType)
                {
                    case (int)MatchFormat.ACTION_TYPE.pickItem:
                        return ConstantVars.PICK_ITEM_TEXT;
                    case (int)MatchFormat.ACTION_TYPE.startClimb:
                        return ConstantVars.START_CLIMB_TEXT;
                }
            }
            else
            {
                switch (eventType)
                {
                    case (int)MatchFormat.DROP_TYPE.drop1:
                        return ConstantVars.DROP_1_TEXT;
                    case (int)MatchFormat.DROP_TYPE.drop2:
                        return ConstantVars.DROP_2_TEXT;
                    case (int)MatchFormat.DROP_TYPE.drop3:
                        return ConstantVars.DROP_3_TEXT;
                    case (int)MatchFormat.DROP_TYPE.dropItemCollector:
                        return ConstantVars.DROP_COLLECTOR_TEXT;
                    case (int)MatchFormat.DROP_TYPE.dropNone:
                        return ConstantVars.DROP_NONE_TEXT;
                }
            }
            return ConstantVars.DROP_ITEM_TEXT;
        }

        public static String returnEventImage(int eventType) {
            if(eventType < 0) {
                switch (eventType) {
                    case (int)MatchFormat.ACTION_TYPE.pickItem:
                        return ConstantVars.PICK_ITEM_IMAGE;
                    case (int)MatchFormat.ACTION_TYPE.startClimb:
                        return ConstantVars.START_CLIMB_IMAGE;
                }
            }
            else {
                switch (eventType) {
                    case (int)MatchFormat.DROP_TYPE.drop1:
                        return ConstantVars.DROP_1_IMAGE;
                    case (int)MatchFormat.DROP_TYPE.drop2:
                        return ConstantVars.DROP_2_IMAGE;
                    case (int)MatchFormat.DROP_TYPE.drop3:
                        return ConstantVars.DROP_3_IMAGE;
                    case (int)MatchFormat.DROP_TYPE.dropItemCollector:
                        return ConstantVars.DROP_COLLECTOR_IMAGE;
                    case (int)MatchFormat.DROP_TYPE.dropNone:
                        return ConstantVars.DROP_NONE_IMAGE;
                }
            }
            return ConstantVars.DROP_ITEM_IMAGE;
        }

        void Handle_Clicked(object sender, System.EventArgs e, List<Data> data)
        {

        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            listView.ItemsSource = sortedEventViewList((List<MatchFormat.Data>)App.Current.Properties["tempMatchEvents"]);
        }
    }
}
