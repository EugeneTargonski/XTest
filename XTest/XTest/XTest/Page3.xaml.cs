using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XTest.Services;

namespace XTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page3 : ContentPage
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
        public Page3()
        {
            InitializeComponent();
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