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
            GetWorkingTime();
        }
        private async Task GetWorkingTime()
        {
            //string json = JsonSerializer.Serialize(user);
            using (HttpClient httpClient = new HttpClient())
            {
                string response = await httpClient.GetStringAsync("http://xtestapplication.azurewebsites.net/api/workingtimes");//.ConfigureAwait(false);
                var CCopt = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                workingTimes = JsonSerializer.Deserialize<List<WorkingTime>>(response, CCopt);

            }
            ResetView();
        }
        private void ResetView()
        {
            //TODO: need 1 time to run

            calendarGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            for (var i = 2; i <= 6; i++)
                calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 0; i <= 7; i++)
                calendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var firstDayOfMonth = new DateTime(initialDate.Year, initialDate.Month, 1);
            var dayOfWeekEU = firstDayOfMonth.DayOfWeek == 0 ? 7 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU+1);

            var leftButton = new Button { Text = "<", Style = plainButton, IsEnabled = IsEnabled };
            leftButton.Clicked += LeftButtonClicked;
            calendarGrid.Children.Add(leftButton, 0, 0);
            var rightButton = new Button { Text = ">", Style = plainButton, IsEnabled = IsEnabled };
            rightButton.Clicked += RigthButtonClicked;
            calendarGrid.Children.Add(rightButton, 6, 0);
            
            for (var y = 2; y <= 7; y++)
                for (var x = 0; x <= 6; x++)
                {
                    if (y == 2)
                    {
                        var dayOfWeekLabel = new Label()
                        {
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = currCalDay.ToString("ddd")
                        };
                        calendarGrid.Children.Add(dayOfWeekLabel, x, 1);
                    };
                    if (currCalDay.Month != initialDate.Month)
                    {
                        currCalDay = currCalDay.AddDays(1);
                        continue; 
                    }
                    Color datecolor = Color.Black;
                    //TODO need check for empty
                    //TODO workingTimes need for cleaning by year
                    foreach (var workingTime in workingTimes)
                        if (currCalDay >= workingTime.BeginDate && currCalDay <= workingTime.EndDate)
                        {
                            int rewrew = Convert.ToInt32(Math.Pow(2, (double)currCalDay.DayOfWeek));
                            if ((rewrew & workingTime.WeeklyHolydays)>0)
                                datecolor = Color.Red;
                        }
                    /*  */
                    bool IsEnabled = currCalDay.Month == initialDate.Month;
                    var dateButton = new Button { Text = currCalDay.Day.ToString(), Style = plainButton, IsEnabled = IsEnabled, TextColor = datecolor };
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