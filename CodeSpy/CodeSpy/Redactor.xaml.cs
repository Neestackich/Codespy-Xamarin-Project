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
using System.Net.Http.Headers;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redactor : ContentPage
    {
        JDoodle jdoodle;

        Button sendToUrl;
        Button saveFile;
        Button deleteFile;

        Label connectionMessage;
        Label stringNumberBar;

        internal Editor codeEditor;

        Frame roundedEditor;
        Frame roundedStringNumberBar;

        AbsoluteLayout absoluteLayout;

        RelativeLayout relativeLayout;

        ScrollView scrollView;

        StackLayout stackView;

        FileManaging fileManagingPage;

        int stringCounter;
        int textSize;

        public Redactor(FileManaging _fileManagingPage)
        {
            fileManagingPage = _fileManagingPage;

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

                saveFile = new Button
                {
                    Text = "S",
                    Style = (Style)Resources["RedactorButtonsStyle"]
                };
                saveFile.Clicked += SaveFile_Clicked;

                codeEditor = new Editor
                {
                    FontFamily = "Consolas",
                    Text = new string('\n', 21),
                    TextColor = (Color)Resources["CodeEditorTextColor"],
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    BackgroundColor =
                        (Color)Resources["CodeEditorBackground"]
                };
                codeEditor.TextChanged += StringNumberBar_Change;

                stringNumberBar = new Label
                {
                    TextColor = (Color)Resources["NumberBarTextColor"],
                    FontSize = codeEditor.FontSize,
                    HorizontalOptions = LayoutOptions.Center
                };

                roundedStringNumberBar = new Frame
                {
                    Margin = new Thickness(3, 3, -1, 64),
                    Padding = new Thickness(6, 17, 1, 10),
                    CornerRadius = 20,
                    IsClippedToBounds = true,
                    HasShadow = false,
                    BorderColor =
                       (Color)Resources["CodeEditorBorderColor"],
                    BackgroundColor =
                       (Color)Resources["CodeEditorBackground"],
                    HeightRequest = codeEditor.HeightRequest,
                    Content = stringNumberBar
                };

                NumberBarFilling();

                roundedEditor = new Frame
                {
                    Margin = new Thickness(0, 3, 3, 64),
                    Padding = new Thickness(7, 7, 7, 30),
                    CornerRadius = 20,
                    BackgroundColor =
                       (Color)Resources["CodeEditorBackground"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = codeEditor,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BorderColor =
                       (Color)Resources["CodeEditorBorderColor"]
                };

                absoluteLayout = new AbsoluteLayout();

                relativeLayout = new RelativeLayout();

                scrollView = new ScrollView
                {
                    Orientation = ScrollOrientation.Both
                };

                stackView = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                stackView.Children.Add(roundedStringNumberBar);
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

        private void SaveFile_Clicked(object sender, EventArgs e)
        {
            //передать имя и текст файла в редакторе
            //fileManagingPage.Save();
        }

        private void NumberBarFilling()
        {
            stringCounter = 1;
            textSize = codeEditor.Text.Length;

            for (int i = 0; i < codeEditor.Text.Length; i++)
            {
                if (codeEditor.Text[i] == '\n')
                {
                    stringCounter++;
                }
            }

            stringNumberBar.Text = null;

            for (int i = 1; i <= stringCounter; i++)
            {
                stringNumberBar.Text += $"{i}\n";
            }
        }

        private void StringNumberBar_Change(object sender, TextChangedEventArgs e)
        {
            NumberBarFilling();
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