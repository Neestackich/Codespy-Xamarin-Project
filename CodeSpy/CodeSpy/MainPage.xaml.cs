using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeSpy
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //страница, которая будет сразу показываться пользователю
            Detail = new NavigationPage(new Redactor())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            //свойство, показ мастер страницы
            IsPresented = true;
        }

        //открываем страницу с камерой
        private void CameraButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Camera())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            //чтоб меню пропадало
            IsPresented = false;
        }

        //открываем страницу с редактором
        private void RedactorButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Redactor())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            IsPresented = false;
        }

        //открываем страницу с менеджером файлов
        private void FileManagingButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new FileManaging())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            IsPresented = false;
        }

        //открываем страницу с браузером
        private void BrowserButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Browser())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            IsPresented = false;
        }

        //открываем страницу с настройками
        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Settings())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            IsPresented = false;
        }
    }
}
