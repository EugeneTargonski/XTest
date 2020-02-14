using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Settings;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Auth;
using Contracts.Models;
using System.Net.Http;
using System.Text.Json;
using XamForms.Controls;

namespace XTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
        }
    }
}