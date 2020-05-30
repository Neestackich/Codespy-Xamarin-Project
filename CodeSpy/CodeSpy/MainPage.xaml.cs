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

        Redactor redactorPage;

        Camera cameraPage;

        FileManaging fileManagingPage;

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
            redactorPage = new Redactor(fileManagingPage);

            redactor = new NavigationPage(redactorPage)
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            cameraPage = new Camera(redactorPage);

            camera = new NavigationPage(cameraPage)
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            fileManagingPage = new FileManaging(redactorPage);

            fileManager = new NavigationPage(fileManagingPage)
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            browser = new NavigationPage(new Browser())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            settings = new NavigationPage(new Settings(redactorPage))
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };
        }

        //открываем страницу с камерой
        private void CameraButton_Clicked(object sender, EventArgs e)
        {
            Detail = camera;

            PageIcon.Source = "photoCamera.png";

            //чтоб меню пропадало
            IsPresented = false;
        }

        //открываем страницу с редактором
        private void RedactorButton_Clicked(object sender, EventArgs e)
        {
            Detail = redactor;

            PageIcon.Source = "processor.png";

            IsPresented = false;
        }

        //открываем страницу с менеджером файлов
        private void FileManagingButton_Clicked(object sender, EventArgs e)
        {
            Detail = fileManager;

            PageIcon.Source = "portableDocument.png";

            IsPresented = false;
        }

        //открываем страницу с браузером
        private void BrowserButton_Clicked(object sender, EventArgs e)
        {
            Detail = browser;

            PageIcon.Source = "navigation.png";

            IsPresented = false;
        }

        //открываем страницу с настройками
        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Detail = settings;

            PageIcon.Source = "settings.png";

            IsPresented = false;
        }
    }
}