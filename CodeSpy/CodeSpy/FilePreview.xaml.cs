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
    public partial class FilePreview : ContentPage
    {
        Button save;
        Button delete;

        Redactor redactorPage;

        public FilePreview(string fileName, string fileText, Redactor _redactorPage)
        {
            redactorPage = _redactorPage;

            InitializeComponent();

            Linking();
        }

        private void Linking()
        {
            save = new Button
            {
                Text = "S",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            //save.Clicked += 

            delete = new Button
            {
                Text = "D",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
        }

        async void Delete(object sender, EventArgs args)
        {
            // получаем имя файла
            string fileName = (string)((MenuItem)sender).BindingContext;

            // удаляем файл из списка
            await DependencyService.Get<IFileManaging>().DeleteAsync(fileName);
        }

        internal async void Save(string fileName, string fileText)
        {
            if (String.IsNullOrEmpty(fileName)) return;

            // если файл существует
            if (await DependencyService.Get<IFileManaging>().ExistAsync(fileName))
            {
                // запрашиваем разрешение на перезапись
                bool isRewrited = await DisplayAlert("Подверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");

                if (isRewrited == false) return;
            }

            // перезаписываем файл
            await DependencyService.Get<IFileManaging>().SaveTextAsync(fileName, fileText);
        }

        private void RedactButton_Clicked(object sender, EventArgs e)
        {
            if (redactorPage.codeEditor.Text == new string('\n', 21))
            {
                redactorPage.codeEditor.Text = null;
                //redactorPage.codeEditor.Text = 
            }
            else
            {
                //redactorPage.codeEditor.Text += 
            }
        }
    }
}