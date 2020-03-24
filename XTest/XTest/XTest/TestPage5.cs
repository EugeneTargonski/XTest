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
            Calendar calendar = new Calendar();
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