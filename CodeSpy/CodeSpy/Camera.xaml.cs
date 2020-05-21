using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Xamarin.Forms;


namespace CodeSpy
{
    public partial class Camera : ContentPage
    {
        Image Photo;

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
                    Text = "Capture"
                };
                Capture.Clicked += Capture_OnClicked;

                Select = new Button
                {
                    Text = "Select"
                };
                Select.Clicked += Select_OnClicked;

                Photo = new Image();

                capturedText = new Label();

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children = { Capture, Select, Photo, capturedText }
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
                    Text = "Capture"
                };
                Capture.Clicked += Capture_OnClicked;

                Select = new Button
                {
                    Text = "Select"
                };
                Select.Clicked += Select_OnClicked;

                Photo = new Image();

                capturedText = new Label();

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Children = { Capture, Select, Photo, capturedText }
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

        public void Clear()
        {
            Photo.Source = null;

            capturedText.Text = null;
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
        }
    }
}