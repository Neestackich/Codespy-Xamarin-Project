using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPageNew : ContentPage
    {
        Image splashImage;
        Image splashPageBackground;

        public SplashPageNew()
        {
            //BackgroundColor = Color.FromHex("#38323b");

            //убираем у экземпляра этого класса шапку навигации
            NavigationPage.SetHasNavigationBar(this, false);

            var absLayout = new AbsoluteLayout();

            splashPageBackground = new Image
            {
                Source = "SplashPageScreen.jpg"
            };

            splashImage = new Image
            {
                Source = "spy.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            //absLayout.Children.Add(splashPageBackground, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absLayout.Children.Add(splashImage, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(splashImage,
                AbsoluteLayoutFlags.PositionProportional);

            Content = splashPageBackground;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //первый аргумент - размер, второй аргумент время в милисекундах
            await splashImage.ScaleTo(1, 2200);
            await splashImage.ScaleTo(0.9, 300, Easing.Linear);
            await splashImage.ScaleTo(0.9, 100);
            await splashImage.ScaleTo(150, 2000, Easing.Linear);

            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}