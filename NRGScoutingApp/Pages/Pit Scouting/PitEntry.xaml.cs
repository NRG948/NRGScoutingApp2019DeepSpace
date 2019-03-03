using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;

namespace NRGScoutingApp {
    public partial class PitEntry : ContentPage {
       //Object for storing all the pit notes for JSON conversion
        public String[] vals = new string[ConstantVars.QUESTIONS.Length + 1];
       
        protected override bool OnBackButtonPressed () {
            return true;
        }
        private Editor[] inputs = new Editor[ConstantVars.QUESTIONS.Length];
        private Label[] questions = new Label[ConstantVars.QUESTIONS.Length];
        //The boolean will hide the delete button if the entry is new
        public PitEntry (bool newCreation) {
            NavigationPage.SetHasBackButton (this, false);
            InitializeComponent ();
            vals[vals.Length - 1] = Preferences.Get("teamStart", "memes not recieve");
            for (int i = 0; i < inputs.Length; i++)
            {
                questions[i] = new Label
                {
                    Text = ConstantVars.QUESTIONS[i],
                    MinimumWidthRequest = 1000,
                    MinimumHeightRequest = 300,
                    Margin = new Thickness(10, 5, 0, 0),
                    FontAttributes = FontAttributes.Bold,
                };
                inputs[i] = new Editor
                {
                    Placeholder = "Type Here",
                    MinimumWidthRequest = 1000,
                    MinimumHeightRequest = 300,
                    Margin = new Thickness(10, 5, 0, 0),
                    Keyboard = Keyboard.Text,
                    AutoSize = EditorAutoSizeOption.TextChanges
                };
                inputs[i].TextChanged += new EventHandler<TextChangedEventArgs>(Comment_Box_Updated);
                mainLayout.Children.Add(questions[i]);
                mainLayout.Children.Add(inputs[i]);
            }
            deleteButton.IsVisible = !newCreation;
            Preferences.Set ("appState", 2);
            cacheCheck ();
        }

        protected void Comment_Box_Updated (object sender, Xamarin.Forms.TextChangedEventArgs e) {
            updateAllBoxes ();
            updateItems ();
        }

        void updateAllBoxes () {
            for(int i = 0; i < vals.Length-1; i++) {
                vals[i] = inputs[i].Text;
            }
        }

        async void deleteClicked (object sender, System.EventArgs e) {
            bool text = await DisplayAlert ("Are you sure you want to delete??", "Data CANNOT be recovered", "No", "Yes");
            if (!text) {
                JObject data = JObject.Parse (Preferences.Get ("matchEventsString", ""));
                JArray pitNotes = (JArray) data["PitNotes"];
                var delItem = pitNotes.ToList ().Find (x => x["team"].ToString ().Equals (Preferences.Get ("teamStart", "")));
                pitNotes.Remove (delItem);
                if (pitNotes.Count <= 0) {
                    data.Remove ("PitNotes");
                }
                if (data.Count <= 0) {
                    Preferences.Set ("matchEventsString", "");
                }
                Preferences.Set ("matchEventsString", JsonConvert.SerializeObject (data));
                try {
                    if (!Matches.appRestore) {
                        await Navigation.PopToRootAsync (true);
                    } else if (Matches.appRestore) {
                        Matches.appRestore = false;
                        await Navigation.PopAsync (true);
                    }
                } catch (System.NullReferenceException) { }
                clearMatchItems ();
            }
        }

        async void backClicked (object sender, System.EventArgs e) {
            bool text = await DisplayAlert ("Alert", "Do you want to discard progress?", "Yes", "No");
            if (text) {
                clearMatchItems ();
                try {
                    if (Matches.appRestore == false) {
                        Matches.appRestore = false;
                        await Navigation.PopToRootAsync (true);
                    } else if (Matches.appRestore == true) {
                        Matches.appRestore = false;
                        await Navigation.PopAsync (true);
                    }
                } catch (System.NullReferenceException) {

                }
            }
        }

        void updateItems () {
            Dictionary<String, String> temp = new Dictionary<string, string>();
            Console.WriteLine("DAM IT");
            for (int i = 0; i < vals.Length-1; i++) {
                temp.Add("q" + i, vals[i]);
                Console.WriteLine(vals[i]);
            }
            temp["team"] =  vals[vals.Length-1];
            Preferences.Set("tempPitNotes", JsonConvert.SerializeObject(temp));
        }

        //Clears all properties for use in next match
        void clearMatchItems () {
            Preferences.Set ("teamStart", "");
            Preferences.Set ("appState", 0);
            Preferences.Set ("tempPitNotes", "");
        }

        void saveClicked (object sender, System.EventArgs e) {
            //Disables save button so app doesn't crash when user taps many times
            saveButton.IsEnabled = false;

            vals[vals.Length-1] = Preferences.Get ("teamStart", "oof");
            Console.WriteLine("team");
            Console.WriteLine(vals[vals.Length - 1]);
            Console.WriteLine(Preferences.Get("teamStart", "oof"));
            Dictionary<String,String> s = new Dictionary<String,String>();
            for(int i = 0; i < vals.Length-1; i++) {
                s.Add("q" + i,vals[i]);
            }
            s.Add("team", vals[vals.Length - 1]);
            JObject notes = JObject.FromObject(s);
            Console.WriteLine(notes);
            if (isAllEmpty (notes)) {
                try {
                    if (Matches.appRestore == false) {
                        Navigation.PopToRootAsync (true);
                    } else if (Matches.appRestore == true) {
                        Matches.appRestore = false;
                        Navigation.PopAsync (true);
                    }
                } catch (System.InvalidOperationException) { }
                clearMatchItems ();
            } else {
                //Adds or creates new JObject to start all data in app cache
                JObject data = MatchParameters.initializeEventsObject ();
                if (!data.ContainsKey ("PitNotes")) {
                    data.Add (new JProperty ("PitNotes", new JArray ()));
                    pushBackToHome (data, new JArray (), notes);
                } else {
                    JArray temp = (JArray) data["PitNotes"];
                    if (temp.ToList ().Exists (x => x["team"].Equals (notes["team"]))) {
                        var item = temp.ToList ().Find (x => x["team"].Equals (notes["team"]));
                        temp.Remove (item);
                        for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++) {
                            item["q" + (i)] = giveNewString (item["q" + (i)].ToString (), notes["q" + (i)].ToString ());
                        }
                    }
                    pushBackToHome (data, temp, notes);
                }
            }
        }

        //calls all final methods to return to home as it updates all the data
        async void pushBackToHome (JObject data, JArray temp, JObject parameters) {
            temp.Add (new JObject (parameters));
            data["PitNotes"] = temp;
            Console.WriteLine(temp);
            Preferences.Set ("matchEventsString", JsonConvert.SerializeObject (data));
            Console.WriteLine (Preferences.Get ("matchEventsString", ""));
            try {
                if (!Matches.appRestore) {
                    await Navigation.PopToRootAsync (true);
                } else if (Matches.appRestore) {
                    Matches.appRestore = false;
                    await Navigation.PopAsync (true);
                }
            } catch (System.NullReferenceException) { }
            clearMatchItems ();
        }
        private string giveNewString (String old, String add) {
            if (String.IsNullOrWhiteSpace (add) && !String.IsNullOrWhiteSpace (old)) {
                DisplayAlert ("Alert", "Try deleting this entry instead", "ok");
                return old;
            }
            return add;
        }

        //Checks if all the question answers are empty
        private bool isAllEmpty (JObject valsIn) {
            bool total = true;
            for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++) {
                total = String.IsNullOrWhiteSpace (valsIn["q" + i].ToString ()) && total;
            }
            return total;
        }

        //Populates and checks in case of app crash
        void cacheCheck () {
            JObject mainObject = new JObject ();
            if (!String.IsNullOrWhiteSpace (Preferences.Get ("matchEventsString", ""))) {
                mainObject = JObject.Parse (Preferences.Get ("matchEventsString", ""));
            }
            JArray scoutArray = new JArray ();
            if (mainObject.ContainsKey ("PitNotes")) {
                scoutArray = (JArray) mainObject["PitNotes"];
            } else {
                scoutArray = new JArray ();
            }
            Console.WriteLine(scoutArray);
            String team = Preferences.Get ("teamStart", "oof");
            Console.WriteLine("tempPit");
            Console.WriteLine(team);
            Dictionary<String, String> jsonNotes;
            try
            {
                jsonNotes = JsonConvert.DeserializeObject<Dictionary<String, String>>(Preferences.Get("tempPitNotes", ""));
            }
            catch (JsonException)
            {
                jsonNotes = new Dictionary<string, string>();
            }
            String[] s = new string[ConstantVars.QUESTIONS.Length];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = "";
            }
            try
            {
                if (scoutArray.Count > 0 && scoutArray.ToList().Exists(x => x["team"].ToString().Equals(team)))
                {
                    var final = scoutArray.ToList().Find(x => x["team"].ToString().Equals(team));
                    Console.WriteLine("DAM 1");
                    for (int i = 0; i < vals.Length - 1; i++)
                    {
                        s[i] =  (final["q" + i].ToString());
                        Console.WriteLine(s[i].ToString());
                    }
                }
            }
            catch (System.NullReferenceException) { }
            try {
                if(!Preferences.Get("tempPitNotes", "").Equals("")) {
                    for(int i = 0; i < jsonNotes.Count - 1; i++)
                    {
                        Console.WriteLine(jsonNotes["q"+i]);
                    }
                    Console.WriteLine("DAM 2");
                    for (int i = 0; i < jsonNotes.Count - 1; i++)
                    {
                        s[i] += jsonNotes["q" + i];
                        Console.WriteLine(s[i].ToString());
                    }
                    }
                else
                {}
            } catch (JsonException) {}
            Console.WriteLine("DAM 3");
            for (int i = 0; i < ConstantVars.QUESTIONS.Length; i++)
            {
                inputs[i].Text += s[i];
                Console.WriteLine(s[i].ToString());
            }
            vals[vals.Length - 1] = team;
            //Attempting to set all text boxes
            updateAllBoxes();
            updateItems ();

        }
    }
}