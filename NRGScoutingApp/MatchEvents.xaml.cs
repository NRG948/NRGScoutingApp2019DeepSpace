using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Linq;
using Data = System.Collections.Generic.KeyValuePair<string, string>;

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

        public static readonly String pickItemImage = "ic_picked_cube.png";
        public static readonly String dropItemImage = "ic_drop_cube_yellow.png";
        public static readonly String drop1Image = "ic_scale.png";
        public static readonly String drop2Image = "ic_switch.png";
        public static readonly String drop3Image = "ic_scale.png";
        public static readonly String dropItemCollectorImage = "ic_exchange.png";
        public static readonly String dropNoneImage = "ic_cancel.png";
        public static readonly String startClimbImage = "ic_climb_yellow.png";

        public static readonly String pickItemText = "Picked Cube";
        public static readonly String dropItemText = "Dropped Cube";
        public static readonly String drop1Text = "Dropped Scale";
        public static readonly String drop2Text = "Dropped Ally Switch";
        public static readonly String drop3Text = "Dropped Opp. Switch";
        public static readonly String dropItemCollectorText = "Dropped Exchange";
        public static readonly String dropNoneText = "Dropped None";
        public static readonly String startClimbText = "Start Climb";

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
                        return pickItemText;
                    case (int)MatchFormat.ACTION_TYPE.startClimb:
                        return startClimbText;
                }
            }
            else
            {
                switch (eventType)
                {
                    case (int)MatchFormat.DROP_TYPE.drop1:
                        return drop1Text;
                    case (int)MatchFormat.DROP_TYPE.drop2:
                        return drop2Text;
                    case (int)MatchFormat.DROP_TYPE.drop3:
                        return drop3Text;
                    case (int)MatchFormat.DROP_TYPE.dropItemCollector:
                        return dropItemCollectorText;
                    case (int)MatchFormat.DROP_TYPE.dropNone:
                        return dropNoneText;
                }
            }
            return dropItemText;
        }

        public static String returnEventImage(int eventType) {
            if(eventType < 0) {
                switch (eventType) {
                    case (int)MatchFormat.ACTION_TYPE.pickItem:
                        return pickItemImage;
                    case (int)MatchFormat.ACTION_TYPE.startClimb:
                        return startClimbImage;
                }
            }
            else {
                switch (eventType) {
                    case (int)MatchFormat.DROP_TYPE.drop1:
                        return drop1Image;
                    case (int)MatchFormat.DROP_TYPE.drop2:
                        return drop2Image;
                    case (int)MatchFormat.DROP_TYPE.drop3:
                        return drop3Image;
                    case (int)MatchFormat.DROP_TYPE.dropItemCollector:
                        return dropItemCollectorImage;
                    case (int)MatchFormat.DROP_TYPE.dropNone:
                        return dropNoneImage;
                }
            }
            return dropItemImage;
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
