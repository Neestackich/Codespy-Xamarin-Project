using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Collections;

namespace CodeSpy
{
    public partial class Camera : ContentPage
    {
        Image photoToShow;

        Frame roundedPhoto;
        Frame roundedText;
        Frame roundedFramePhoto;

        Button capture;
        Button select;
        Button sendToRedact;

        Label connectionMessage;
        Label capturedText;

        AbsoluteLayout absoluteLayout;

        RelativeLayout relativeLayout;

        ScrollView scrollView;

        StackLayout stackView;

        Redactor redactorPage;

        public Camera(Redactor _redactorPage)
        {
            redactorPage = _redactorPage;

            InitializeComponent();

            ConnectivityCheck();

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
            if (CrossConnectivity.Current.IsConnected)
            {
                //камера 
                capture = new Button
                {
                    Text = "C",
                    Style = (Style)Resources["CameraButtonsStyle"]
                };
                capture.Clicked += Capture_OnClicked;

                //выбор из галереи
                select = new Button
                {
                    Text = "+",
                    Style = (Style)Resources["CameraButtonsStyle"]
                };
                select.Clicked += Select_OnClicked;

                //пересылка полученного текста в редактор
                sendToRedact = new Button
                {
                    Text = "S",
                    Style = (Style)Resources["CameraButtonsStyle"]
                };
                sendToRedact.Clicked += SendToRedact_Clicked;

                capturedText = new Label
                {
                    TextColor = (Color)Resources["FontColor"]
                };

                photoToShow = new Image();

                roundedPhoto = new Frame
                {
                    Margin = 3,
                    Padding = 0,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = photoToShow
                };

                roundedFramePhoto = new Frame
                {
                    Margin = 3,
                    Padding = 1,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = roundedPhoto
                };

                roundedText = new Frame
                {
                    Margin = new Thickness(3, 3, 3, 66),
                    Padding = 8,
                    CornerRadius = 20,
                    BackgroundColor = (Color)Resources["PageBackGroundColor"],
                    IsClippedToBounds = true,
                    HasShadow = false,
                    Content = capturedText
                };

                absoluteLayout = new AbsoluteLayout();

                relativeLayout = new RelativeLayout();

                scrollView = new ScrollView();

                stackView = new StackLayout();
                stackView.Children.Add(roundedFramePhoto);
                stackView.Children.Add(roundedText);

                scrollView.Content = stackView;

                relativeLayout.Children.Add(scrollView,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Height; }));

                AbsoluteLayout.SetLayoutBounds(capture,
                    new Rectangle(0.97, 0.98, 50, 50));
                AbsoluteLayout.SetLayoutFlags(capture,
                    AbsoluteLayoutFlags.PositionProportional);

                AbsoluteLayout.SetLayoutBounds(select,
                   new Rectangle(0.77, 0.98, 50, 50));
                AbsoluteLayout.SetLayoutFlags(select,
                    AbsoluteLayoutFlags.PositionProportional);

                AbsoluteLayout.SetLayoutBounds(sendToRedact,
                   new Rectangle(0.57, 0.98, 50, 50));
                AbsoluteLayout.SetLayoutFlags(sendToRedact,
                    AbsoluteLayoutFlags.PositionProportional);

                absoluteLayout.Children.Add(relativeLayout);
                absoluteLayout.Children.Add(capture);
                absoluteLayout.Children.Add(select);
                absoluteLayout.Children.Add(sendToRedact);

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

                Content = new AbsoluteLayout
                {
                    Padding = 3,
                    Children = { connectionMessage }
                };
            }
        }

        private void SendToRedact_Clicked(object sender, EventArgs e)
        {
            if (redactorPage.codeEditor.Text == new string('\n', 21))
            {
                redactorPage.codeEditor.Text = capturedText.Text;
            }
            else
            {
                redactorPage.codeEditor.Text += capturedText.Text;
            }
        }

        public void Clear()
        {
            photoToShow.Source = null;

            capturedText.Text = null;

            roundedText.BorderColor =
                (Color)Resources["PageBackGroundColor"];
            roundedText.BackgroundColor =
                (Color)Resources["PageBackGroundColor"];

            roundedFramePhoto.BackgroundColor =
                    (Color)Resources["PageBackGroundColor"];
            roundedFramePhoto.BorderColor =
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

                roundedText.BackgroundColor =
                    (Color)Resources["CodeEditorBackground"];
                roundedText.BorderColor =
                    (Color)Resources["ExtractedTextBorderColor"];
            });

            photoToShow.Source = ImageSource.FromStream(photo.GetStream);

            roundedFramePhoto.BackgroundColor =
                (Color)Resources["CodeEditorBackground"];
            roundedFramePhoto.BorderColor =
                (Color)Resources["ExtractedTextBorderColor"];
        }
    }
}