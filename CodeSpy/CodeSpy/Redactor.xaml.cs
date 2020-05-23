using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redactor : ContentPage
    {
        JDoodle jdoodle;

        Button SendToUrl;

        Label connectionMessage;

        public Redactor()
        {
            InitializeComponent();

            if (CrossConnectivity.Current.IsConnected)
            {
                SendToUrl = new Button
                {
                    Text = "Send"
                };
                SendToUrl.Clicked += SendContent_ButtonClick;

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children = { SendToUrl }
                    }
                };
            }
            else
            {
                connectionMessage = new Label
                {
                    Text = "Подключение отсутствует",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                Content = new StackLayout
                {
                    Children = { connectionMessage }
                };
            };

            CrossConnectivity.Current.ConnectivityChanged +=
                Current_ConnectivityChanged;

            jdoodle = new JDoodle();
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
                SendToUrl = new Button
                {
                    Text = "Send"
                };
                SendToUrl.Clicked += SendContent_ButtonClick;

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children = { SendToUrl }
                    }
                };
            }
            else
            {
                connectionMessage = new Label
                {
                    Text = "Подключение отсутствует",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                Content = new StackLayout
                {
                    Children = { connectionMessage }
                };
            }
        }

        private void SendContent_ButtonClick(object sender, EventArgs e)
        {
            jdoodle.SendMessage();
        }
    }
}