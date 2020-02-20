using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamForms.Controls;

namespace XTest
{
    public class TestPage3 : ContentPage
    {
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
        public DateTime? selectedDate;
        public TestPage3()
        {
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(-1)) { BackgroundColor = Color.LightGreen, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(0)) { BackgroundColor = Color.PaleGoldenrod, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(1)) { BackgroundColor = Color.Gold, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(2)) { BackgroundColor = Color.Orange, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(3)) { BackgroundColor = Color.Goldenrod, Selectable = true });
            calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(4)) { BackgroundColor = Color.OrangeRed, Selectable = true });
            calendar.DateClicked += Calendar_DateClicked;

            stackLayout.Children.Add(calendar);
            Content = stackLayout;
        }

        private void Calendar_DateClicked(object sender, DateTimeEventArgs e)
        {
            selectedDate = calendar.SelectedDate;
        }
    }
}