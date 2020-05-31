using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Browser : ContentPage
    {
        WebView gitHubPage;

        Image noConnection;

        public Browser()
        {
            InitializeComponent();

            Linking();

            ConnectivityCheck();

            CrossConnectivity.Current.ConnectivityChanged +=
                Current_ConnectivityChanged;
        }

        private void Current_ConnectivityChanged(object sender,
            ConnectivityChangedEventArgs e)
        {
            ConnectivityCheck();
        }

        private void ConnectivityCheck()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Content = new Grid
                {
                    Children = { gitHubPage }
                };
            }
            else
            {
                Content = new Grid
                {
                    Padding = 35,
                    Children = { noConnection }
                };
            }
        }

        private void Linking()
        {
            noConnection = new Image
            {
                Source = "no.png"
            };

            gitHubPage = new WebView
            {
                Source = new UrlWebViewSource { Url = "https://github.com/login" },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }
    }
}