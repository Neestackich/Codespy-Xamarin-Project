using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace CodeSpy
{
    public partial class Camera : ContentPage
    {
        Image Photo;

        Frame RoundedPhoto;
        Frame RoundedText;

        Button Capture;
        Button Select;

        Label connectionMessage;
        Label capturedText;

        public Camera()
        {
            InitializeComponent();

            if (CrossConnectivity.Current.IsConnected)
            {
                Capture = new Button
                {
                    Text = "C",
                    Style = (Style)Resources["LocalButtonsStyle"]
                };
                Capture.Clicked += Capture_OnClicked;

                Select = new Button
                {
                    Text = "+",
                    Style = (Style)Resources["LocalButtonsStyle"]
                };
                Select.Clicked += Select_OnClicked;

                capturedText = new Label
                {
                    TextColor = (Color)Resources["FontColor"]
                };

                Photo = new Image();

                RoundedPhoto = new Frame
                {
                    Padding = 0,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = Photo
                };

                RoundedText = new Frame
                {
                    Margin = 0,
                    Padding = 5,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = capturedText
                };

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 3,
                        Children = { Capture, Select, RoundedPhoto, RoundedText }
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

            //проверяет подключение
            //если подключение есть - появляются кнопки и прочее
            //если нет - то оставляем толко сообщенеи о том, 
            ///что подключение отсутствует
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
            if(CrossConnectivity.Current.IsConnected)
            {
                Capture = new Button
                {
                    Text = "C",
                    Style = (Style)Resources["LocalButtonsStyle"]
                };
                Capture.Clicked += Capture_OnClicked;

                Select = new Button
                {
                    Text = "+",
                    Style = (Style)Resources["LocalButtonsStyle"]
                };
                Select.Clicked += Select_OnClicked;

                capturedText = new Label
                {
                    TextColor = (Color)Resources["FontColor"]
                };

                Photo = new Image();

                RoundedPhoto = new Frame
                {
                    Padding = 0,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = Photo
                };

                RoundedText = new Frame
                {
                    Padding = 5,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = capturedText
                };

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Padding = 3,
                        Children = { Capture, Select, RoundedPhoto, RoundedText }
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
            }
        }

        public void Clear()
        {
            Photo.Source = null;

            capturedText.Text = null;

            RoundedText.BackgroundColor =
                (Color)Resources["PageBackGroundColor"];
        }

        private async void Capture_OnClicked(object sender, EventArgs e)
        {
            Clear();

            var photo = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });

            Update(photo);
        }

        private async void Select_OnClicked(object sender, EventArgs e)
        {
            Clear();

            var photo = await CrossMedia.Current.PickPhotoAsync(
                new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });

            Update(photo);
        }

        private void Update(MediaFile photo)
        {
            Task.Run(async () =>
            {
                var text = await Ocr.GetTextAsync(
                    photo.GetStreamWithImageRotatedForExternalStorage());

                Device.BeginInvokeOnMainThread(() => capturedText.Text = text);
            });

            Photo.Source = ImageSource.FromStream(photo.GetStream);

            RoundedText.BackgroundColor =
                (Color)Resources["CodeEditorBackground"];
            RoundedText.BorderColor = 
                (Color)Resources["ExtractedTextBorderColor"];
        }
    }
}