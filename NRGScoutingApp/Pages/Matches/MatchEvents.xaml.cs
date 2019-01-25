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
        public MatchEvents()
        {
            BindingContext = this;
            InitializeComponent();
            setListVisibility(false);
        }

        //Constructor for Events View that Users see
        public class EventListFormat
        {
            public String image { get; set; }
            public String eventName { get; set; }
            public String timeOccur { get; set; }
        }

        List<EventListFormat> eventsList;

        protected override void OnAppearing()
        {
            if (NewMatchStart.events.Count > 0)
            {
                Console.WriteLine("passedif");
                eventsList = EventViewList((List<MatchFormat.Data>)NewMatchStart.events);
                setListVisibility();
                listView.ItemsSource = eventsList;
            }
            else
            {
                Console.WriteLine("faildif");
                setListVisibility();
            }
        }

        /*
         * Sets the visibility of the list based on boolean and the sad error opposite
         * So if list.IsVisible = true, then sadNoMatch.IsVisible = false
         */        
        private void setListVisibility(bool setList)
        {
            listView.IsVisible = setList;
            sadNoEvent.IsVisible = !setList;
        }
        private void setListVisibility()
        {
            listView.IsVisible = NewMatchStart.events.Count > 0;
            sadNoEvent.IsVisible = !listView.IsVisible;
        }

        //Populates List that contains all data for each timer event to appear on the Match Events Screen
        public static List<EventListFormat> EventViewList(List<MatchFormat.Data> matchData)
        {
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

        //Returns Text based on enum for action
        public static String returnEventText(int eventType)
        {
            switch (eventType)
            {
                case (int)MatchFormat.ACTION.drop1:
                    return ConstantVars.DROP_1_TEXT;
                case (int)MatchFormat.ACTION.drop2:
                    return ConstantVars.DROP_2_TEXT;
                case (int)MatchFormat.ACTION.drop3:
                    return ConstantVars.DROP_3_TEXT;
                case (int)MatchFormat.ACTION.drop4:
                    return ConstantVars.DROP_4_TEXT;
                case (int)MatchFormat.ACTION.dropNone:
                    return ConstantVars.DROP_NONE_TEXT;
                case (int)MatchFormat.ACTION.pick1:
                    return ConstantVars.PICK_ITEM_1_TEXT;
                case (int)MatchFormat.ACTION.pick2:
                    return ConstantVars.PICK_ITEM_2_TEXT;
                case (int)MatchFormat.ACTION.startClimb:
                    return ConstantVars.START_CLIMB_TEXT;
            }
            return ConstantVars.DROP_ITEM_TEXT;
        }

        public static String returnEventImage(int eventType)
        {
            switch (eventType)
            {
                case (int)MatchFormat.ACTION.drop1:
                    return ConstantVars.DROP_1_IMAGE;
                case (int)MatchFormat.ACTION.drop2:
                    return ConstantVars.DROP_2_IMAGE;
                case (int)MatchFormat.ACTION.drop3:
                    return ConstantVars.DROP_3_IMAGE;
                case (int)MatchFormat.ACTION.drop4:
                    return ConstantVars.DROP_COLLECTOR_IMAGE;
                case (int)MatchFormat.ACTION.dropNone:
                    return ConstantVars.DROP_NONE_IMAGE;
                case (int)MatchFormat.ACTION.pick1:
                    return ConstantVars.PICK_ITEM_1_IMAGE;
                case (int)MatchFormat.ACTION.pick2:
                    return ConstantVars.PICK_ITEM_2_IMAGE;
                case (int)MatchFormat.ACTION.startClimb:
                    return ConstantVars.START_CLIMB_IMAGE;
            }
            return ConstantVars.DROP_ITEM_IMAGE;
        }

        async void eventTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            int index = (listView.ItemsSource as List<EventListFormat>).IndexOf(e.Item as EventListFormat);
            var del = await DisplayAlert("Alert", "Are you sure you want to delete this event?", "No", "Yes");
            if (!del)
            {
                if (eventsList[index].eventName.Contains(ConstantVars.PICK_KEYWORD))
                {
                    if ((index + 1) < eventsList.Count && eventsList[index + 1].eventName.Contains(ConstantVars.DROP_KEYWORD))
                    {
                        removeAtIndex(index + 1);
                        removeAtIndex(index);
                    }
                    else
                    {
                        removeAtIndex(index);
                    }
                }
                else if (eventsList[index].eventName.Contains(ConstantVars.DROP_KEYWORD))
                {
                    if ((index - 1) >= 0 && eventsList[index - 1].eventName.Contains(ConstantVars.PICK_KEYWORD))
                    {
                        removeAtIndex(index - 1);
                        removeAtIndex(index - 1);
                    }
                    else
                    {
                        removeAtIndex(index);
                    }
                }
                else
                {
                    removeAtIndex(index);
                }
            }
            listView.ItemsSource = null;
            listView.ItemsSource = eventsList;
            setListVisibility();
        }

        void removeAtIndex(int index)
        {
            NewMatchStart.events.RemoveAt(index);
            eventsList.RemoveAt(index);
            CubeDroppedDialog.saveEvents();
        }

    }
}
