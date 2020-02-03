//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;
using Contracts.Models;
using System.Net.Http;
using System.Text.Json;

namespace XTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //private static readonly HttpClient client = new HttpClient();
        private User user;


        private readonly Label label = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "Welcome to Xamarin Forms!",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        };

        //async Task 
        async void
            LoginAsync()
        {
            //store = AccountStore.Create();

            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }
            //account = (await SecureStorageAccountStore.FindAccountsForServiceAsync(Constants.AppName)).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        public MainPage()
        {
            user = new User()
            {
                FirstName = "firstName",
                LastName = "lastName",
                GoogleID = "id"
            };
            //InitializeComponent();
            StackLayout stackLayout = new StackLayout();
            Button buttonAdd = new Button
            {
                Text = user.LastName,//"Add user to base",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            buttonAdd.Clicked += OnButtonAddClicked;
            LoginAsync();


            stackLayout.Children.Add(label);
            stackLayout.Children.Add(buttonAdd);
            this.Content = stackLayout;

            
        }

        private async void OnButtonAddClicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            string json = JsonSerializer.Serialize(user);
            /*
            var stringContent = new StringContent(json, Encoding.UTF32, "application/json");

            var response = await client.PostAsync("http://xtestapplication.azurewebsites.net/api/users", stringContent);

            var responseString = await response.Content.ReadAsStringAsync();
            */
            using (HttpClient hc = new HttpClient())
            {
                hc.BaseAddress = new Uri("http://xtestapplication.azurewebsites.net/api/users");
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var param = new StringContent(json, Encoding.Unicode, "application/json");
                HttpResponseMessage response = await hc.PostAsync(hc.BaseAddress, param);

                button.Text = "User added";
                label.Text = user?.GoogleID;
            }
        }

        private void OnAuthCompletedOld(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator.IsAuthenticated())
            {
                //Authentication failed Do something
                label.Text = "Is Authenticated";
                return;
            }
            label.Text = "Isn't Authenticated";
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    //user = JsonConvert.DeserializeObject<User>(userJson);
                    
                    //List<Dictionary<string, string>> ValueList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(userJson);
                    Dictionary<string, string> ValueList = JsonSerializer.Deserialize<Dictionary<string, string>>(userJson);
                    ValueList.TryGetValue("given_name", out string firstName);
                    ValueList.TryGetValue("family_name", out string lastName);
                    ValueList.TryGetValue("id", out string id);
                    user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        GoogleID = id
                    };
                }

                await SecureStorageAccountStore.SaveAsync(e.Account, Constants.AppName);
                //await DisplayAlert("Email address", user?.Email, "OK");
                //await DisplayAlert("Name", user.Name, "OK");
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication error: " + e.Message);
        }
    }
}
