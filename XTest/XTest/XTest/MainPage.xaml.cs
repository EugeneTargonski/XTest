using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;



namespace XTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static readonly string GOOGLE_ID = "21170702167-jriifs3n881an76deo58c5lb92l6fj11.apps.googleusercontent.com";
        public static readonly string GOOGLE_SCOPE = "https://www.googleapis.com/auth/userinfo.email";
        public static readonly string GOOGLE_AUTH = "https://accounts.google.com/o/oauth2/auth";
        public static readonly string GOOGLE_REDIRECTURL = "https://www.googleapis.com/plus/v1/people/me";
        public static readonly string GOOGLE_REQUESTURL = "https://www.googleapis.com/oauth2/v2/userinfo";
        readonly OAuth2Authenticator authenticator = new OAuth2Authenticator(GOOGLE_ID, GOOGLE_SCOPE, new Uri(GOOGLE_AUTH), new Uri(GOOGLE_REDIRECTURL));

        private readonly Label label = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "Welcome to Xamarin Forms!",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        };


        public MainPage()
        {
            //InitializeComponent();
            StackLayout stackLayout = new StackLayout();
            Button button = new Button
            {
                Text = "Нажми!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            button.Clicked += OnButtonClicked;
            var authenticator = new OAuth2Authenticator(
                "21170702167-jriifs3n881an76deo58c5lb92l6fj11.apps.googleusercontent.com",//"21170702167-shcpgfp2385u39188gne29la3udp7k9g.apps.googleusercontent.com",
                null,//"wBlzvVKmeS_DeT4Fspniy2P_",
                "https://www.googleapis.com/auth/userinfo.email",
                new Uri("https://accounts.google.com/o/oauth2/auth"),
                new Uri("urn:ietf:wg:oauth:2.0:oob"),
                new Uri("https://oauth2.googleapis.com/token"),
                null,
                true);

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(button);
            this.Content = stackLayout;
        }

        private void OnButtonClicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            button.Text = "Нажато!";
            //button.BackgroundColor = Color.Red;
        }

        private void OnAuthCompleted(object sender, System.EventArgs e)
        {
            //OAuth2Authenticator auth = (OAuth2Authenticator)sender;
            if (authenticator.IsAuthenticated())
            {
                //Authentication failed Do something
                label.Text = "Is Authenticated";
                return;
            }
            label.Text = "Isn't Authenticated";
        }
    }
}
