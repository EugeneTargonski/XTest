using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XTest
{
    public class MainPage : TabbedPage
    {
        private readonly TestPage1 testPage1 = new TestPage1();
        private readonly TestPage2 testPage2 = new TestPage2();
        private readonly TestPage3 testPage3 = new TestPage3();
        private readonly TestPage4 testPage4 = new TestPage4();

        public MainPage()
        {
            Children.Add(testPage1);
            Children.Add(testPage2);
            Children.Add(testPage3);
            Children.Add(testPage4);
            CurrentPageChanged += MainPage_CurrentPageChanged;
        }

        private void MainPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if(CurrentPage == testPage4)
                testPage4.label.Text = testPage3.selectedDate.ToString();
        }
    }
}