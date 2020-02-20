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
        public TestPage4()
        {
            label.Text = "qqqqqqq";
            stackLayout.Children.Add(label);
            Content = stackLayout;
        }
    }
}