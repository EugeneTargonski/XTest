using System;
using Xamarin.Forms;

namespace XCalendar
{
    public class Calendar : ContentView
    {
        public DateTime? SelectedDate { get; private set; } = null;
        public Calendar() : this(DateTime.Now)
        {
        }
        public Calendar(DateTime dateTime): this(dateTime, null, null)
        {
        }
        public Calendar(DateTime dateTime, Style unselectedDateStyle, Style selectedDateStyle)
        {
            initialDate = dateTime;
            if (unselectedDateStyle == null)
                unselectedDateStyle = new Style(typeof(Button))
                {
                    Setters = {
                                                                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                                                                        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                                                                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                                                                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                                                                        }
                };
            if (selectedDateStyle == null)
                selectedDateStyle = new Style(typeof(Button))
                {
                    Setters = {
                                                                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#E8AD00") },
                                                                        new Setter { Property = Button.TextColorProperty, Value = Color.White },
                                                                        new Setter { Property = Button.CornerRadiusProperty, Value = 0 },
                                                                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                                                                        }
                };
            UnselectedDateStyle = unselectedDateStyle;
            SelectedDateStyle = selectedDateStyle;
            this.Content = RefreshGrid();
        }
        private DateTime initialDate;
        private Button lastClickedButton = null;
        private readonly Style UnselectedDateStyle;
        private readonly Style SelectedDateStyle;
        private Grid RefreshGrid()
        {
            var newCalendarGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            for (var i = 2; i <= 6; i++)
                newCalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 0; i <= 6; i++)
                newCalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var firstDayOfMonth = new DateTime(initialDate.Year, initialDate.Month, 1);
            var dayOfWeekEU = firstDayOfMonth.DayOfWeek == 0 ? 7 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU + 1);
            var leftButton = new Button { Text = "<", Style = UnselectedDateStyle, IsEnabled = true };
            leftButton.Clicked += LeftButtonClicked;
            newCalendarGrid.Children.Add(leftButton, 0, 0);
            var rightButton = new Button { Text = ">", Style = UnselectedDateStyle, IsEnabled = true };
            rightButton.Clicked += RigthButtonClicked;
            newCalendarGrid.Children.Add(rightButton, 6, 0);
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
                    var dateButton = new Button { Text = currCalDay.Day.ToString(), Style = UnselectedDateStyle, IsEnabled = IsEnabled, TextColor = datecolor };
                    dateButton.Clicked += DateClicked;
                    newCalendarGrid.Children.Add(dateButton, x, y);
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
            newCalendarGrid.Children.Add(monthLabel, 1, 0);
            Grid.SetColumnSpan(monthLabel, 5);
            return newCalendarGrid;
        }
        private void DateClicked(object sender, EventArgs e)
        {
            var pressedButton = (Button)sender;
            pressedButton.Style = SelectedDateStyle;
            if (lastClickedButton != null)
            {
                lastClickedButton.Style = UnselectedDateStyle;
            }
            SelectedDate = new DateTime(initialDate.Year, initialDate.Month, int.Parse(pressedButton.Text));
            lastClickedButton = pressedButton;
        }
        private void LeftButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(-1);
            this.Content = RefreshGrid();
        }
        private void RigthButtonClicked(object sender, EventArgs e)
        {
            initialDate = initialDate.AddMonths(1);
            this.Content = RefreshGrid();
        }
    }
}
