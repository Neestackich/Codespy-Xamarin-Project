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

        public SplashPageNew()
        {
            //убираем у экземпляра этого класса шапку навигации
            NavigationPage.SetHasNavigationBar(this, false);

            var absLayout = new AbsoluteLayout();

            splashImage = new Image
            {
                Source = "hacker.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            absLayout.Children.Add(splashImage, new Rectangle(0.5, 0.5, 
                AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(splashImage,
                AbsoluteLayoutFlags.PositionProportional);

            Content = absLayout;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //первый аргумент - размер, второй аргумент время в милисекундах
            await splashImage.ScaleTo(1, 1600);
            await splashImage.ScaleTo(0.9, 300, Easing.Linear);
            await splashImage.ScaleTo(0.9, 100);
            await splashImage.ScaleTo(2, 250, Easing.Linear);

            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}