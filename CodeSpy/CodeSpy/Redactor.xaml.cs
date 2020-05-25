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

        Button sendToUrl;

        Label connectionMessage;

        Editor codeEditor;

        Frame RoundedEditor;

        public Redactor()
        {
            InitializeComponent();

            if (CrossConnectivity.Current.IsConnected)
            {
                sendToUrl = new Button
                {
                    Text = "Send",
                    TextColor = 
                       (Color)Resources["RedactorFontColor"],
                    BackgroundColor = 
                       (Color)Resources["RedactorButtonBGColor"],
                    CornerRadius = 20,
                    HeightRequest = 40,
                    WidthRequest = 40,
                    BorderWidth = 2,
                    BorderColor = (Color)Resources["CodeEditorBorderColor"]
                };
                sendToUrl.Clicked += SendContent_ButtonClick;

                codeEditor = new Editor
                {
                    HeightRequest = 400,
                    FontFamily = "Consolas",
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    BackgroundColor =
                        (Color)Resources["CodeEditorBackground"]
                };

                RoundedEditor = new Frame
                {
                    Padding = 10,
                    CornerRadius = 20,
                    BackgroundColor = 
                       (Color)Resources["CodeEditorBackground"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = codeEditor,
                    BorderColor =
                       (Color)Resources["CodeEditorBorderColor"]
                };

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 3,
                        Children = { RoundedEditor, sendToUrl }
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
                    Padding = 3,
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
                sendToUrl = new Button
                {
                    Text = "Send",
                    TextColor = (Color)Resources["FontColor"]
                };
                sendToUrl.Clicked += SendContent_ButtonClick;

                codeEditor = new Editor
                {
                    HeightRequest = 400,
                    FontFamily = "Consolas",
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    BackgroundColor =
                        (Color)Resources["CodeEditorBackground"]
                };

                RoundedEditor = new Frame
                {
                    Padding = 10,
                    CornerRadius = 20,
                    BackgroundColor =
                        (Color)Resources["CodeEditorBackground"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = codeEditor
                };
                RoundedEditor.BorderColor =
                        (Color)Resources["CodeEditorBorderColor"];

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 3,
                        Children = { RoundedEditor, sendToUrl }
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
                    Padding = 3,
                    Children = { connectionMessage }
                };
            };
        }

        private async void SendContent_ButtonClick(object sender, EventArgs e)
        {
            jdoodle.SendMessage(codeEditor.Text);


            await DisplayAlert("Compiler message", $"Result: {jdoodle.compilerResult.output}\n" +
                $"Status code: {jdoodle.compilerResult.statusCode}\n" +
                $"Memory used: {jdoodle.compilerResult.memory}\n" +
                $"Cpu time: {jdoodle.compilerResult.cpuTime}", "Ok");
        }
    }
}