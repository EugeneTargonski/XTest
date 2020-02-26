using Contracts.Models;
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

        public TestPage3()
        { 

        }
        /*
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
        private readonly ListView listView = new ListView();
        // определяем источник данных
       
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

            stackLayout.Children.Add(calendar);
            stackLayout.Children.Add(listView);
            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;
            Content = scrollView.Content;
        }

        private void Calendar_DateClicked(object sender, DateTimeEventArgs e)
        {
            selectedDate = calendar.SelectedDate;
        }
        /**/
    }
}