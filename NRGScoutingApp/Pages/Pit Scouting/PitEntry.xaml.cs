using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace NRGScoutingApp
{
    public partial class PitEntry : ContentPage
    {
        string titleName;
        public string teamName { get { return titleName; } }

        //Object for storing all the pit notes for JSON conversion
        public class PitQs {
            public String team { get; set; }
            public String q0 { get; set; }
            public String q1 { get; set; }
            public String q2 { get; set; }
            public String q3 { get; set; }
            public String q4 { get; set; }
            public String q5 { get; set; }
            public String q6 { get; set; }
            public String q7 { get; set; }
            public String q8 { get; set; }
            public String q9 { get; set; }
            public String q10 { get; set; }
        }

        PitQs vals = new PitQs {
            q0 = "",
            q1 = "",
            q2 = "",
            q3 = "",
            q4 = "",
            q5 = "",
            q6 = "",
            q7 = "",
            q8 = "",
            q9 = "",
            q10 = "",
            team = ""
        };

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        public PitEntry()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            App.Current.Properties["appState"] = 2;
            App.Current.SavePropertiesAsync();
            setLabels();
            cacheCheck();
        }

        void setLabels() {
            q1.Text = ConstantVars.QUESTIONS[0];
            q2.Text = ConstantVars.QUESTIONS[1];
            q3.Text = ConstantVars.QUESTIONS[2];
            q4.Text = ConstantVars.QUESTIONS[3];
            q5.Text = ConstantVars.QUESTIONS[4];
            q6.Text = ConstantVars.QUESTIONS[5];
            q7.Text = ConstantVars.QUESTIONS[6];
            q8.Text = ConstantVars.QUESTIONS[7];
            q9.Text = ConstantVars.QUESTIONS[8];
            q10.Text = ConstantVars.QUESTIONS[9];
            q11.Text = ConstantVars.QUESTIONS[10];
        }

        void Comment_Box_Updated(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            updateAllBoxes();
            updateItems();
        }

        void updateAllBoxes() {
            vals.q0 = q1Text.Text;
            vals.q1 = q2Text.Text;
            vals.q2 = q3Text.Text;
            vals.q3 = q4Text.Text;
            vals.q4 = q5Text.Text;
            vals.q5 = q6Text.Text;
            vals.q6 = q7Text.Text;
            vals.q7 = q8Text.Text;
            vals.q8 = q9Text.Text;
            vals.q9 = q10Text.Text;
            vals.q10 = q11Text.Text;
        }

        async void backClicked(object sender, System.EventArgs e)
        {
            var text = await DisplayAlert("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text)
            {
                clearMatchItems();
                if (Matches.appRestore == false)
                {
                    Matches.appRestore = false;
                    await Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    await Navigation.PopAsync(true);
                }
            }
        }

        void updateItems() {
            App.Current.Properties["tempPitNotes"] = vals;
            App.Current.SavePropertiesAsync();
       }

        //Clears all properties for use in next match
        void clearMatchItems()
        {
            App.Current.Properties["teamStart"] = "";
            App.Current.Properties["appState"] = 0;
            App.Current.Properties["tempPitNotes"] = "";
            App.Current.SavePropertiesAsync();
            // CLEAR OBJECT CONTAINING THE STRINGS: .events.Clear();
        }

        void saveClicked(object sender, System.EventArgs e)
        {
            //Disables save button so app doesn't crash when user taps many times
            saveButton.IsEnabled = false;

            vals.team = App.Current.Properties["teamStart"].ToString();
            JObject notes = JObject.FromObject(vals);

            if (isAllEmpty(notes)) {
                try
                {
                    if (Matches.appRestore == false)
                    {
                        Navigation.PopToRootAsync(true);
                    }
                    else if (Matches.appRestore == true)
                    {
                        Matches.appRestore = false;
                        Navigation.PopAsync(true);
                    }
                }
                catch (System.InvalidOperationException)
                {
                    Console.WriteLine("caught");
                }
                clearMatchItems();
            }
            else {
                //Adds or creates new JObject to start all data in app cache
                JObject data = MatchParameters.initializeEventsObject();
                if (data.Count <= 0 || !data.ContainsKey("PitNotes"))
                {
                    data.Add(new JProperty("PitNotes", new JArray()));
                    pushBackToHome(data, new JArray(), notes);
                }
                else
                {
                    JArray temp = (JArray)data["PitNotes"];
                    if (temp.ToList().Exists(x => x["team"].Equals(notes["team"])))
                    {
                        var item = temp.ToList().Find(x => x["team"].Equals(notes["team"]));
                        for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++)
                        {
                            item["q" + (i)] = giveNewString(item["q" + (i)].ToString(), notes["q" + (i)].ToString());
                        }
                    }
                    pushBackToHome(data, temp, notes);
                }
            }

        }

        //calls all final methods to return to home as it updates all the data
        async void pushBackToHome(JObject data, JArray temp, JObject parameters)
        {
            temp.Add(new JObject(parameters));
            data["PitNotes"] = temp;
            App.Current.Properties["matchEventsString"] = JsonConvert.SerializeObject(data);
            await App.Current.SavePropertiesAsync();
            try
            {
                if (Matches.appRestore == false)
                {
                     Navigation.PopToRootAsync(true);
                }
                else if (Matches.appRestore == true)
                {
                    Matches.appRestore = false;
                    Navigation.PopAsync(true);
                }
            }
            catch (System.InvalidOperationException)
            {
                Navigation.PopAsync(true);
            }
            clearMatchItems();
        }
        private string giveNewString(String old, String add)
        {
            if(String.IsNullOrWhiteSpace(add) && !String.IsNullOrWhiteSpace(old)) {
                DisplayAlert("Alert", "Try deleting this entry instead", "ok");
                return old;
            }
            return add;
        }

        //Checks if all the question answers are empty
        private bool isAllEmpty(JObject vals) {
            bool total = true;
            for(int i = 0; i < ConstantVars.QUESTIONS.Length; i++) {
                total = String.IsNullOrWhiteSpace(vals["q" + i].ToString()) && total;
            }
            return total;
        }

        //Populates and checks in case of app crash
        void cacheCheck(){
            JObject mainObject = new JObject();
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["matchEventsString"].ToString()))
            {
                mainObject = JObject.Parse(App.Current.Properties["matchEventsString"].ToString());
            }
            JArray scoutArray = new JArray();
            if (mainObject.ContainsKey("PitNotes")) { 
                scoutArray = (JArray) mainObject["PitNotes"];
            }
            else {
                scoutArray = new JArray();
            }
            String team = App.Current.Properties["teamStart"].ToString();
            PitQs temp = new PitQs();
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["tempPitNotes"].ToString()))
            {
               temp = (PitQs)App.Current.Properties["tempPitNotes"];
            }
            //Attempting to set all text boxes
            try
            {
                if (scoutArray.Count > 0 && scoutArray.ToList().Exists(x => x["team"].ToString().Equals(team)))
                {
                    var final = scoutArray.ToList().Find(x => x["team"].ToString().Equals(team));
                    q1Text.Text = final["q0"].ToString();
                    q2Text.Text = final["q1"].ToString();
                    q3Text.Text = final["q2"].ToString();
                    q4Text.Text = final["q3"].ToString();
                    q5Text.Text = final["q4"].ToString();
                    q6Text.Text = final["q5"].ToString();
                    q7Text.Text = final["q6"].ToString();
                    q8Text.Text = final["q7"].ToString();
                    q9Text.Text = final["q8"].ToString();
                    q10Text.Text = final["q9"].ToString();
                    q11Text.Text = final["q10"].ToString();
                }
            }
            catch (System.NullReferenceException) {}
            if (!String.IsNullOrWhiteSpace(App.Current.Properties["tempPitNotes"].ToString())){
                q1Text.Text += temp.q0;
                q2Text.Text += temp.q1;
                q3Text.Text += temp.q2;
                q4Text.Text += temp.q3;
                q5Text.Text += temp.q4;
                q6Text.Text += temp.q5;
                q7Text.Text += temp.q6;
                q8Text.Text += temp.q7;
                q9Text.Text += temp.q8;
                q10Text.Text += temp.q9;
                q11Text.Text += temp.q10;
            }
            updateAllBoxes();
            updateItems();
        }

    }
}