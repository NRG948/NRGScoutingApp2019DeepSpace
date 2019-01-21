﻿using System;
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
        }

        //Constructor for Events View that Users see
        public class EventListFormat{ 
            public String image { get; set; }
            public String eventName { get; set; }
            public String timeOccur { get; set; }
        }

        List<EventListFormat> eventsList;

        protected override void OnAppearing()
        {
            if (!String.IsNullOrWhiteSpace(NewMatchStart.events.ToString()))
            {
                eventsList = EventViewList((List<MatchFormat.Data>)NewMatchStart.events);
                listView.ItemsSource = eventsList;
            }
        }

        //Populates List that contains all data for each timer event to appear on the Match Events Screen
        public static List<EventListFormat> EventViewList(List<MatchFormat.Data> matchData) {
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

        async void eventTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            int index = (listView.ItemsSource as List<EventListFormat>).IndexOf(e.Item as EventListFormat) ;
            var del = await DisplayAlert("Alert", "Are you sure you want to delete this event?", "No", "Yes");
            if (!del)
            {
                if (eventsList[index].eventName.Contains(ConstantVars.PICK_KEYWORD)) {
                    if((index+1) < eventsList.Count && eventsList[index + 1].eventName.Contains(ConstantVars.DROP_KEYWORD)) {
                        removeAtIndex(index + 1);
                        removeAtIndex(index);
                    }
                    else {
                        removeAtIndex(index);
                    }
                }
                else if(eventsList[index].eventName.Contains(ConstantVars.DROP_KEYWORD)) 
                {
                    if ((index - 1) >= 0 && eventsList[index - 1].eventName.Contains(ConstantVars.PICK_KEYWORD))
                    {
                        removeAtIndex(index - 1);
                        removeAtIndex(index - 1);
                    }
                    else {
                        removeAtIndex(index);
                    }
                }
                else { 
                removeAtIndex(index);
                }
            }
            listView.ItemsSource = null;
            listView.ItemsSource = eventsList;
        }

        void removeAtIndex(int index) {
            NewMatchStart.events.RemoveAt(index);
            eventsList.RemoveAt(index);
            CubeDroppedDialog.saveEvents();
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            listView.ItemsSource = EventViewList((List<MatchFormat.Data>)NewMatchStart.events);
        }
    }
}