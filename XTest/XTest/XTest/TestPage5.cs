using XCalendar;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XTest
{
    public class TestPage5 : ContentPage
    {
        public TestPage5()
        {
            
            Calendar calendar = new Calendar() 
            {
                HideOtherMonthsDates = false,
                WeeklyHolydays = 65,
                UnplannedWorkingDays = new List<DateTime> { new DateTime(2020, 03, 28) },
                UnplannedHolydays = new List<DateTime> { new DateTime(2020, 03, 25), new DateTime(2020, 03, 24) }
            };
            calendar.Init();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(calendar);
            ScrollView scrollView = new ScrollView
            {
                Content = stackLayout
            };
            Content = scrollView;
        }
    }
}