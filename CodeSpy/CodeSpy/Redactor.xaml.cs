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
using System.Runtime.InteropServices;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redactor : ContentPage
    {
        JDoodle jdoodle;

        Button sendToUrl;

        Label connectionMessage;

        Editor codeEditor;

        Frame roundedEditor;

        AbsoluteLayout absoluteLayout;

        RelativeLayout relativeLayout;

        ScrollView scrollView;

        StackLayout stackView;

        public Redactor()
        {
            InitializeComponent();

            ConnectivityCheck();

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
                    Text = ">",
                    Style = (Style)Resources["RedactorButtonsStyle"]
                };
                sendToUrl.Clicked += SendContent_ButtonClick;

                codeEditor = new Editor
                {
                    FontFamily = "Consolas",
                    Text = new string('\n', 22),
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    BackgroundColor =
                        (Color)Resources["CodeEditorBackground"]
                };

                roundedEditor = new Frame
                {
                    Margin = new Thickness(20, 3, 3, 80),
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

                absoluteLayout = new AbsoluteLayout();

                relativeLayout = new RelativeLayout();

                scrollView = new ScrollView();

                stackView = new StackLayout();

                stackView.Children.Add(roundedEditor);

                scrollView.Content = stackView;

                //ресайз 
                relativeLayout.Children.Add(scrollView,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Height; }));

                //кнопка
                sendToUrl.HorizontalOptions = LayoutOptions.End;
                sendToUrl.VerticalOptions = LayoutOptions.CenterAndExpand;

                AbsoluteLayout.SetLayoutBounds(sendToUrl, new Rectangle(0.97, 0.98, 50, 50));
                AbsoluteLayout.SetLayoutFlags(sendToUrl, AbsoluteLayoutFlags.PositionProportional);

                absoluteLayout.Children.Add(relativeLayout);
                absoluteLayout.Children.Add(sendToUrl);

                Content = absoluteLayout;
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