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
        NavigationPage redactor;
        NavigationPage camera;
        NavigationPage fileManager;
        NavigationPage browser;
        NavigationPage settings;

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Linking();

            //страница, которая будет сразу показываться пользователю
            Detail = redactor;

            //свойство, показ мастер страницы
            IsPresented = true;
        }

        private void Linking()
        {
            redactor = new NavigationPage(new Redactor())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            camera = new NavigationPage(new Camera())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            fileManager = new NavigationPage(new FileManaging())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            browser = new NavigationPage(new Browser())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            settings = new NavigationPage(new Settings())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };
        }

        //открываем страницу с камерой
        private void CameraButton_Clicked(object sender, EventArgs e)
        {
            Detail = camera;

            //чтоб меню пропадало
            IsPresented = false;
        }

        //открываем страницу с редактором
        private void RedactorButton_Clicked(object sender, EventArgs e)
        {
            Detail = redactor;

            IsPresented = false;
        }

        //открываем страницу с менеджером файлов
        private void FileManagingButton_Clicked(object sender, EventArgs e)
        {
            Detail = fileManager;

            IsPresented = false;
        }

        //открываем страницу с браузером
        private void BrowserButton_Clicked(object sender, EventArgs e)
        {
            Detail = browser;

            IsPresented = false;
        }

        //открываем страницу с настройками
        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Detail = settings;

            IsPresented = false;
        }
    }
}
