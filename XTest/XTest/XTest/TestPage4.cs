using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private readonly StackLayout stackLayout = new StackLayout();
        private readonly Style plainButton = new Style(typeof(Button))
        {
            Setters = {
                        new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                        new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
                        new Setter { Property = Button.FontSizeProperty, Value = 16 }
                      }
        };

        public TestPage4()
        {
            var controlGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };
            for (var i = 1; i <= 7; i++)
                controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (var i = 1; i <= 7; i++)
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var dayOfWeekEU = (int)firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek;
            var currCalDay = firstDayOfMonth.AddDays(-dayOfWeekEU);
            for (var y = 1; y <= 6; y++) 
                for (var x = 0; x <= 6; x++)
                {
                    controlGrid.Children.Add(new Button { Text = currCalDay.Day.ToString(), Style = plainButton }, x, y);
                    currCalDay =  currCalDay.AddDays(1);
                }


            stackLayout.Children.Add(label);
            stackLayout.Children.Add(controlGrid);
            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;
            Content = scrollView;
        }
    }
}