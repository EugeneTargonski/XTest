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
    public class Calendar
    {
        //public StackLayout stackLayout;
        public Calendar()
        {
            RefreshView();
        }
        private DateTime initialDate = DateTime.Now;
        private Button lastClickedButton = null;
        public Grid calendarGrid;
        private readonly Style unselectedButton = new Style(typeof(Button))
        {
            Setters = {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                      }
        };
        private readonly Style selectedButton = new Style(typeof(Button))
        {
            Setters = {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex("#E8AD00") },
                        new Setter { Property = Button.TextColorProperty, Value = Color.White },
                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
            }
        };
        private void RefreshView()
        {
            calendarGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            for (var i = 2; i <= 6; i++)
                calendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 0; i <= 6; i++)
                calendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var firstDayOfMonth = new DateTime(initialDate.Year, initialDate.Month, 1);
            var dayOfWeekEU = firstDayOfMonth.DayOfWeek == 0 ? 7 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU + 1);
            var leftButton = new Button { Text = "<", Style = unselectedButton, IsEnabled = true };
            leftButton.Clicked += LeftButtonClicked;
            calendarGrid.Children.Add(leftButton, 0, 0);
            var rightButton = new Button { Text = ">", Style = unselectedButton, IsEnabled = true };
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
                    /*  */
                    bool IsEnabled = currCalDay.Month == initialDate.Month;
                    var dateButton = new Button { Text = currCalDay.Day.ToString(), Style = unselectedButton, IsEnabled = IsEnabled, TextColor = datecolor };
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
        }
        private void DateClicked(object sender, EventArgs e)
        {
            var pressedButton = (Button)sender;
            pressedButton.Style = selectedButton;
            if (lastClickedButton != null)
            {
                lastClickedButton.Style = unselectedButton;
            }
            lastClickedButton = pressedButton;
        }
        private void LeftButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(-1);
            RefreshView();
        }
        private void RigthButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(1);
            RefreshView();
        }
    }
    public class TestPage5 : ContentPage
    {
        public TestPage5()
        {
            Calendar calendar = new Calendar();
            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(calendar.calendarGrid);
            ScrollView scrollView = new ScrollView
            {
                Content = stackLayout
            };
            Content = scrollView;
        }
    }
}