using Contracts.Models;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Auth;
using Xamarin.Forms;
using XamForms.Controls;

namespace XTest
{
    public class TestPage1 : ContentPage
    {
        private User user = new User()
        {
            Id = CrossSettings.Current.GetValueOrDefault("Id", 0),
            FirstName = CrossSettings.Current.GetValueOrDefault("FirstName", null),
            LastName = CrossSettings.Current.GetValueOrDefault("LastName", null),
            GoogleID = CrossSettings.Current.GetValueOrDefault("GoogleID", null),
            EMail = CrossSettings.Current.GetValueOrDefault("EMail", null),
            Description = CrossSettings.Current.GetValueOrDefault("Description", null),
        };
        private readonly Label label = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
        };
        private readonly Button buttonAdd = new Button
        {
            Text = "Add user to base",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
            BorderWidth = 1,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        private readonly Button buttonLogin = new Button
        {
            Text = "Login",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
            BorderWidth = 1,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        private readonly Button buttonTest = new Button
        {
            Text = "Test",
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
            BorderWidth = 1,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        private readonly Button buttonShowCal = new Button
        {
            Text = "Calendar",
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
            BorderWidth = 1,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        private readonly Calendar calendar = new Calendar
        {
            BorderColor = Color.LightGray,
            BorderWidth = 1,
            BackgroundColor = Color.White,
            StartDay = DayOfWeek.Monday,
            StartDate = DateTime.Now,
            DatesFontSize = Device.GetNamedSize(NamedSize.Small, typeof(Calendar)),
            TitleLabelFontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Calendar)),

        };
        private readonly StackLayout stackLayout = new StackLayout();
        public TestPage1()
        {
            Title = "hate XAML";
            label.Text = user.GoogleID;

            buttonAdd.Clicked += OnButtonAddClicked;
            buttonLogin.Clicked += OnButtonLoginClicked;
            buttonLogin.Clicked += OnButtonLoginClicked;
            buttonTest.Clicked += OnButtonTestClicked;
            buttonShowCal.Clicked += OnButtonShowCalClicked;
            calendar.DateClicked += DateClicked;
            //calendar.SpecialDates = new List<SpecialDate>();

            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(-1)) { BackgroundColor = Color.LightGreen, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(0)) { BackgroundColor = Color.PaleGoldenrod, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(1)) { BackgroundColor = Color.Gold, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(2)) { BackgroundColor = Color.Orange, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(3)) { BackgroundColor = Color.Goldenrod, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(4)) { BackgroundColor = Color.OrangeRed, Selectable = true });

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(buttonAdd);
            stackLayout.Children.Add(buttonLogin);
            stackLayout.Children.Add(buttonTest);
            stackLayout.Children.Add(buttonShowCal);

            Content = stackLayout;
        }
        private async void OnButtonAddClicked(object sender, System.EventArgs e)
        {
            string json = JsonSerializer.Serialize(user);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://xtestapplication.azurewebsites.net/api/users");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var param = new StringContent(json, Encoding.Unicode, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, param);
                label.Text = "User GoogleID=" + user?.GoogleID + " added to database";
            }
        }
        private void OnButtonShowCalClicked(object sender, System.EventArgs e)
        {
            if (stackLayout.Children.Contains(calendar))
                stackLayout.Children.Remove(calendar);
            else stackLayout.Children.Add(calendar);
        }
        private void OnButtonLoginClicked(object sender, System.EventArgs e)
        {
            Login();
        }
        private void DateClicked(object sender, System.EventArgs e)
        {
            label.Text = calendar.SelectedDate.ToString() + " clicked";
        }
        private void OnButtonTestClicked(object sender, System.EventArgs e)
        {
        }
        void Login()
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

            var authenticator = new OAuth2Authenticator(clientId, null, Constants.Scope, new Uri(Constants.AuthorizeUrl), new Uri(redirectUri),
                                                        new Uri(Constants.AccessTokenUrl), null, true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;
            AuthenticationState.Authenticator = authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private void AfterLogin()
        {
            CrossSettings.Current.AddOrUpdateValue("Id", user.Id);
            CrossSettings.Current.AddOrUpdateValue("FirstName", user.FirstName);
            CrossSettings.Current.AddOrUpdateValue("LastName", user.LastName);
            CrossSettings.Current.AddOrUpdateValue("GoogleID", user.GoogleID);
            CrossSettings.Current.AddOrUpdateValue("EMail", user.EMail);
            CrossSettings.Current.AddOrUpdateValue("Description", user.Description);

            label.Text = "User GoogleID=" + user?.GoogleID + " logined";
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
                AfterLogin();
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