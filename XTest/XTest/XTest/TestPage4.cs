using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XTest
{
    public class TestPage4 : ContentPage
    {
        public readonly Label label = new Label
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
        };
        //private readonly StackLayout stackLayout = new StackLayout();
        private readonly Style plainButton = new Style(typeof(Button))
        {
            Setters = {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                      }
        };
        private readonly Style orangeButton = new Style(typeof(Button))
        {
            Setters = {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex("#E8AD00") },
                        new Setter { Property = Button.TextColorProperty, Value = Color.White },
                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
            }
        };
        private DateTime initialDate = DateTime.Now;
        private Button lastClickedButton = null;
        private Grid calendarGrid;
        private readonly ListView listView = new ListView();
        private List<WorkingTime> workingTimes = new List<WorkingTime>();
        public TestPage4()
        {
            Task getWorkingTime = GetWorkingTime();
            getWorkingTime.Wait();

            ResetView();
        }
        private async Task GetWorkingTime()
        {
            //string json = JsonSerializer.Serialize(user);
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://xtestapplication.azurewebsites.net/api/workingtimes");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                await httpClient.SendAsync(request);

                /*
                hc.BaseAddress = new Uri("http://xtestapplication.azurewebsites.net/api/workingtimes");
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //var param = new StringContent(json, Encoding.Unicode, "application/json");
                HttpResponseMessage response = await hc.GetAsync(hc.BaseAddress);*/
                workingTimes = JsonSerializer.Deserialize<List<WorkingTime>>(request.Content.ToString());

                label.Text = "WT done";
            }
        }
        private void ResetView()
        {
            //TODO: need 1 time to run

            calendarGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            for (var i = 1; i <= 7; i++)
                calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 1; i <= 7; i++)
                calendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var firstDayOfMonth = new DateTime(initialDate.Year, initialDate.Month, 1);
            var dayOfWeekEU = firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU);

            var leftButton = new Button { Text = "<", Style = plainButton, IsEnabled = IsEnabled };
            leftButton.Clicked += LeftButtonClicked;
            calendarGrid.Children.Add(leftButton, 0, 0);
            var rightButton = new Button { Text = ">", Style = plainButton, IsEnabled = IsEnabled };
            rightButton.Clicked += RigthButtonClicked;
            calendarGrid.Children.Add(rightButton, 6, 0);
            
            for (var y = 1; y <= 6; y++)
                for (var x = 0; x <= 6; x++)
                {
                    bool IsEnabled = currCalDay.Month == initialDate.Month;
                    var dateButton = new Button { Text = currCalDay.Day.ToString(), Style = plainButton, IsEnabled = IsEnabled };
                    dateButton.Clicked += DateClicked;
                    calendarGrid.Children.Add(dateButton, x, y);
                    currCalDay = currCalDay.AddDays(1);
                }

            var monthLabel = new Label
            {
                Text = initialDate.ToString("MMMM yyyy"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 20
            };
            calendarGrid.Children.Add(monthLabel, 1, 0);
            Grid.SetColumnSpan(monthLabel, 5);

                        StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(calendarGrid);
            stackLayout.Children.Add(listView);
            ScrollView scrollView = new ScrollView
            {
                Content = stackLayout
            };
            Content = scrollView;
            lastClickedButton = null;
        }
        private void DateClicked(object sender, EventArgs e)
        {
            var pressedButton = (Button)sender;
            pressedButton.Style = orangeButton;
            if (lastClickedButton != null)
            {
                lastClickedButton.Style = plainButton;
                lastClickedButton.BorderColor = Color.White;
            }
            label.Text = new DateTime(initialDate.Year, initialDate.Month, int.Parse((pressedButton).Text)).ToString();
            var records = new List<MeetingRecord>();
            var record1 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 9, 0, 0), End = new DateTime(2020, 3, 14, 10, 30, 0) };
            var record2 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 10, 30, 0), End = new DateTime(2020, 3, 14, 12, 0, 0) };
            var record3 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 12, 0, 0), End = new DateTime(2020, 3, 14, 13, 30, 0) };
            var record4 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 14, 30, 0), End = new DateTime(2020, 3, 14, 16, 0, 0) };
            var record5 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 16, 0, 0), End = new DateTime(2020, 3, 14, 17, 30, 0) };
            var record6 = new MeetingRecord() { Begin = new DateTime(2020, 3, 14, 17, 30, 0), End = new DateTime(2020, 3, 14, 19, 0, 0) };
            records.Add(record1);
            records.Add(record2);
            records.Add(record3);
            records.Add(record4);
            records.Add(record5);
            records.Add(record6);

            listView.ItemsSource = records;

            pressedButton.BorderColor = Color.Red;
            

            lastClickedButton = pressedButton;
        }
        private void LeftButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(-1);
            ResetView();
        }
        private void RigthButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(1);
            ResetView();
        }
    }
}