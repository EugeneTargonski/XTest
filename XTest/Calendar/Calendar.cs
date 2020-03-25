using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XCalendar
{
    public class Calendar : ContentView
    {
        public DateTime? SelectedDate { get; private set; } = null;
        public byte WeeklyHolydays { get; set; }
        public List<DateTime> UnplannedWorkingDays { get; set; }
        public List<DateTime> UnplannedHolydays { get; set; }
        public DateTime InitialDate { get; set; } = DateTime.Now;
        public bool HideOtherMonthsDates { get; set; } = true;
        public Style UnselectedDateStyle { get; set; } = new Style(typeof(Button))
        {
            Setters = {
                                                                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                                                                        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                                                                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                                                                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                                                                        }
        };
        public Style SelectedDateStyle { get; set; } = new Style(typeof(Button))
        {
            Setters = {
                                                                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#E8AD00") },
                                                                        new Setter { Property = Button.TextColorProperty, Value = Color.White },
                                                                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                                                                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                                                                        }
        };
        public Style HolydayStyle { get; set; } = new Style(typeof(Button))
        {
            Setters = {
                                                                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                                                                        new Setter { Property = Button.TextColorProperty, Value = Color.Red },
                                                                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                                                                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                                                                        }
        };
        
        public void Init()
        {
            Content = RefreshGrid();
        }

        private Button lastClickedButton = null;
        private Style lastClickedButtonStyle = null;

        private Grid RefreshGrid()
        {
            var newCalendarGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            for (var i = 2; i <= 6; i++)
                newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 0; i <= 6; i++)
                newCalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var firstDayOfMonth = new DateTime(InitialDate.Year, InitialDate.Month, 1);
            var dayOfWeekEU = firstDayOfMonth.DayOfWeek == 0 ? 7 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU + 1);
            var leftButton = new Button { Text = "<", Style = UnselectedDateStyle, IsEnabled = true };
            leftButton.Clicked += LeftButtonClicked;
            newCalendarGrid.Children.Add(leftButton, 0, 0);
            var rightButton = new Button { Text = ">", Style = UnselectedDateStyle, IsEnabled = true };
            rightButton.Clicked += RigthButtonClicked;
            newCalendarGrid.Children.Add(rightButton, 6, 0);
            bool IsEnabled = true;
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
                        newCalendarGrid.Children.Add(dayOfWeekLabel, x, 1);
                    };
                    if (HideOtherMonthsDates&&currCalDay.Month != InitialDate.Month)
                    {
                        currCalDay = currCalDay.AddDays(1);
                        continue;
                    }
                    

                    IsEnabled = currCalDay.Month == InitialDate.Month;

                    int currentDayOfWeek = Convert.ToInt32(Math.Pow(2, (double)currCalDay.DayOfWeek));
                    var buttonStyle = ((currentDayOfWeek & WeeklyHolydays) > 0) ? HolydayStyle : UnselectedDateStyle;

                    //TODO need check with Resharper                    
                    buttonStyle = (UnplannedWorkingDays?.Contains(currCalDay) ?? false) ? UnselectedDateStyle : buttonStyle;
                    buttonStyle = (UnplannedHolydays?.Contains(currCalDay)??false) ? HolydayStyle : buttonStyle;

                    var dateButton = new Button { Text = currCalDay.Day.ToString(), Style = buttonStyle, IsEnabled = IsEnabled};
                    dateButton.Clicked += DateClicked;
                    newCalendarGrid.Children.Add(dateButton, x, y);
                    currCalDay = currCalDay.AddDays(1);
                }

            var monthLabel = new Label
            {
                Text = InitialDate.ToString("MMMM yyyy"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 20
            };
            newCalendarGrid.Children.Add(monthLabel, 1, 0);
            Grid.SetColumnSpan(monthLabel, 5);
            return newCalendarGrid;
        }
        private void DateClicked(object sender, EventArgs e)
        {
            var pressedButton = (Button)sender;
            
            if (lastClickedButton != null)
            {
                lastClickedButton.Style = lastClickedButtonStyle;
            }
            lastClickedButtonStyle = pressedButton.Style;
            pressedButton.Style = SelectedDateStyle;

            SelectedDate = new DateTime(InitialDate.Year, InitialDate.Month, int.Parse(pressedButton.Text));
            lastClickedButton = pressedButton;
        }
        private void LeftButtonClicked(object sender, EventArgs e)
        {
            InitialDate = InitialDate.AddMonths(-1);
            this.Content = RefreshGrid();
        }
        private void RigthButtonClicked(object sender, EventArgs e)
        {
            InitialDate = InitialDate.AddMonths(1);
            this.Content = RefreshGrid();
        }
    }
}
