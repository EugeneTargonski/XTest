using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamForms.Controls;

namespace XTest
{
    public class TestPage3 : ContentPage
    {

        private List<MeetingType> meetingTypes = new List<MeetingType>();
        private readonly ListView listView = new ListView();
        public readonly Label label = new Label
        {
            Text = "List of operations^",
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
        };
        public TestPage3()
        {
            MeetingTypesLoadAndShow();
            StackLayout stackLayout = new StackLayout();
            //stackLayout.Children.Add(sendButton);
            //sendButton.Clicked += SendButtonClicked;
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(listView);
            ScrollView scrollView = new ScrollView
            {
                Content = stackLayout
            };
            Content = scrollView;
        }
        private async void MeetingTypesLoadAndShow()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync("http://xtestapplication.azurewebsites.net/api/meetingtypes");
                var CCopt = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                meetingTypes = JsonSerializer.Deserialize<List<MeetingType>>(response, CCopt);
                listView.ItemsSource = meetingTypes;
            }
        }
        private async void SendButtonClicked(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync("http://xtestapplication.azurewebsites.net/api/meetingtypes");
                var CCopt = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                meetingTypes = JsonSerializer.Deserialize<List<MeetingType>>(response, CCopt);
            }
        }
     
    }
}