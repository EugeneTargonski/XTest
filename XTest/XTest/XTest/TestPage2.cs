using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XTest.Services;
using Xamarin.Forms;

namespace XTest
{
    public class TestPage2 : ContentPage
    {
        private readonly Button buttonScan = new Button
        {
            Text = "Scan",
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
            BorderWidth = 1,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
        private readonly Entry txtBarcode = new Entry
        {
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            Placeholder = "Barcode Result"
        };
        private readonly StackLayout stackLayout = new StackLayout();
        public TestPage2()
        {
            Title = "QR-scanner";
            buttonScan.Clicked += OnButtonScanClicked;
            stackLayout.Children.Add(txtBarcode);
            stackLayout.Children.Add(buttonScan);
            Content = stackLayout;
        }

        private async void OnButtonScanClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    txtBarcode.Text = result;
                }
            }
            catch
            {
                //Not scanned
            }
        }
    }
}