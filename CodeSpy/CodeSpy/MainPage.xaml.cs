using CodeSpy.Activity;
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
        public List<MasterPageMenuItem> menuList { get; set; }

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            menuList = new List<MasterPageMenuItem>();
            menuList.Add(new MasterPageMenuItem()
            {
                Title = "Camera",
                TargetType = typeof(Camera)
            });
            menuList.Add(new MasterPageMenuItem()
            {
                Title = "Redactor",
                TargetType = typeof(Redactor)
            });
            menuList.Add(new MasterPageMenuItem()
            {
                Title = "File manager",
                TargetType = typeof(FileManaging)
            });
            menuList.Add(new MasterPageMenuItem()
            {
                Title = "Brouser",
                TargetType = typeof(Browser)
            });
            menuList.Add(new MasterPageMenuItem()
            {
                Title = "Settings",
                TargetType = typeof(Settings)
            });

            NavigationButtonsList.ItemsSource = menuList;

            //страница, которая будет сразу показываться пользователю
            Detail = new NavigationPage(new Redactor())
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            }; 

             //свойство, показ мастер страницы
             IsPresented = true;
        }

        private void MenuItem_Click(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageMenuItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page))
            {
                BarBackgroundColor = (Color)Resources["BarBackGroundColor"],
                BarTextColor = (Color)Resources["BarTextColor"]
            };

            IsPresented = false;
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
