using System;
using Rg.Plugins.Popup.Services;
using Plugin.FirebaseAuth;
using Xamarin.Essentials;
using Plugin.CloudFirestore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NRGScoutingApp
{
    public partial class AuthPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        async void RegisterClicked(object sender, System.EventArgs e)
        {
            try
            {
                int teamNum = Convert.ToInt32(frcNum.Text);
                await firebaseUserChecker(teamNum.ToString(), true);
                LoginClicked(sender, e);
            }
            catch (FirebaseAuthException s)
            {
                await DisplayAlert("Error", "One of the following errors occured:" +
                        "\n- Email/Team Already exists\n- Incorrect email/team format", "OK");
                Console.WriteLine(s.StackTrace);
            }
        }

        async private Task firebaseUserChecker(String teamNum, bool createOrLogin)
        {
            login.IsEnabled = false;
            reg.IsEnabled = false;
            IDocumentSnapshot teamAssociations = await CrossCloudFirestore.Current.Instance.
                    GetCollection("TeamLogins").
                    GetDocument("LoginJSON").
                    GetDocumentAsync();
            JObject uid = JObject.Parse(teamAssociations.Data["UIDConnections"].ToString());
            //if (uid.ContainsKey(teamNum) && createOrLogin)
            //{
            //    throw new FirebaseAuthException("Bad FRC Team Number", Plugin.FirebaseAuth.ErrorType.InvalidUser);
            //}
            if (createOrLogin)
            {
                Console.WriteLine(teamNum + "FRC TEMA NUM");
                IAuthResult result = await CrossFirebaseAuth.Current.Instance.CreateUserWithEmailAndPasswordAsync(email.Text, pwd.Text);
                if (uid.ContainsKey(teamNum))
                {
                    JArray teamUID = (JArray)uid[teamNum];
                    teamUID.Add(result.User.Uid);
                }
                else
                {
                    uid.Add(new JProperty(teamNum, new JArray(result.User.Uid)));
                }
                await CrossCloudFirestore.Current.Instance.
                    GetCollection("TeamLogins").
                    GetDocument("LoginJSON").
                    UpdateDataAsync(new Dictionary<string, object> { ["UIDConnections"] = JsonConvert.SerializeObject(uid) });
                //await CrossCloudFirestore.Current
                //    .Instance
                //    .GetCollection(ConstantVars.APP_YEAR)
                //    .AddDocumentAsync(new );
                //await CrossCloudFirestore.Current
                //.Instance
                //.GetCollection(ConstantVars.APP_YEAR)
                //.GetDocument(teamNum)
                //.UpdateDataAsync(new Dictionary<String, object> { ["AllData"] = "{}" });
            }
            else
            {
                IAuthResult result = await CrossFirebaseAuth.Current.Instance.SignInWithEmailAndPasswordAsync(email.Text, pwd.Text);
                if (!uid.ContainsKey(teamNum) || !uid[teamNum].ToList().Contains(result.User.Uid))
                {
                    Console.WriteLine(result.User.Uid);
                    foreach (String s in uid[teamNum])
                    {
                        Console.WriteLine(s);
                    }
                    CrossFirebaseAuth.Current.Instance.SignOut();
                    throw new FirebaseAuthException("Email not linked with team but exists", Plugin.FirebaseAuth.ErrorType.InvalidCredentials);
                }
                else
                {
                    Preferences.Set("loginTeamNum", teamNum);
                }
            }
        }

        async void LoginClicked(object sender, System.EventArgs e)
        {
            try
            {
                await firebaseUserChecker(frcNum.Text.ToString(), false);
                Console.WriteLine("Login as FRC TEAM " + Preferences.Get("loginTeamNum", "failiure"));
                login.IsEnabled = true;
                reg.IsEnabled = true;
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception s)
            {
                await DisplayAlert("Error", "One of the following errors occured:" +
                    "\n- Team/email combo incorrect\n- Incorrect email/password", "OK");
                login.IsEnabled = true;
                reg.IsEnabled = true;
                Console.WriteLine(s.StackTrace);
            }
        }
    }
}
