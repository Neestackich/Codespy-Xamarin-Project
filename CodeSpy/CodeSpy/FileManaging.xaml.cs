using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileManaging : ContentPage
    {
        Redactor redactorPage;

        public FileManaging(Redactor _redactorPage)
        {
            redactorPage = _redactorPage;

            InitializeComponent();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //Save(fileNameEntry.Text, textEditor.Text);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await UpdateFileList();
        }

        // сохранение текста в файл
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

            // обновляем список файлов
            await UpdateFileList();
        }

        async void FileSelect(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem == null) return;

            // получаем выделенный элемент
            string fileName = (string)args.SelectedItem;
            string fileText = await DependencyService.Get<IFileManaging>().LoadTextAsync((string)args.SelectedItem);

            await Navigation.PushAsync(new FilePreview(fileName, fileText, redactorPage));

            await UpdateFileList();

            // снимаем выделение
            filesList.SelectedItem = null;
        }

        async void Delete(object sender, EventArgs args)
        {
            // получаем имя файла
            string fileName = (string)((MenuItem)sender).BindingContext;

            // удаляем файл из списка
            await DependencyService.Get<IFileManaging>().DeleteAsync(fileName);

            // обновляем список файлов
            await UpdateFileList();
        }

        // обновление списка файлов
        async Task UpdateFileList()
        {
            // получаем все файлы
            filesList.ItemsSource = await DependencyService.Get<IFileManaging>().GetFilesAsync();

            // снимаем выделение
            filesList.SelectedItem = null;
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