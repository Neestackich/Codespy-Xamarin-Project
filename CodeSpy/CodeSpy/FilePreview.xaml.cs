using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilePreview : ContentPage
    {
        ImageButton save;
        ImageButton delete;
        ImageButton increaseFont;
        ImageButton decreaseFont;
        ImageButton sendToRedactor;

        Entry fileName;

        Editor textEditor;

        Frame roundedEditor;
        Frame roundedFileName;

        Redactor redactorPage;

        AbsoluteLayout absoluteLayout;

        RelativeLayout relativeLayout;

        ScrollView scrollView;

        StackLayout stackLayout;

        public FilePreview(string _fileName, string _fileText, 
            Redactor _redactorPage)
        {
            InitializeComponent();

            redactorPage = _redactorPage;

            Linking();

            fileName.Text = _fileName;

            textEditor.Text = _fileText;
        }

        private void Linking()
        {
            save = new ImageButton
            {
                Padding = 10,
                Source = "save.png",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            save.Clicked += SaveButton_Click;

            delete = new ImageButton
            {
                Padding = 10,
                Source = "delete.png",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            delete.Clicked += DeleteButton_Click;

            sendToRedactor = new ImageButton
            {
                Padding = 10,
                Source = "processorPurple.png",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            sendToRedactor.Clicked += RedactButton_Clicked;

            increaseFont = new ImageButton
            {
                Padding = 10,
                Source = "plus.png",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            increaseFont.Clicked += IncreaseFont_Clicked;

            decreaseFont = new ImageButton
            {
                Padding = 10,
                Source = "minus.png",
                Style = (Style)Resources["FilePrevButtonsStyle"]
            };
            decreaseFont.Clicked += DecreaseFont_Clicked;

            fileName = new Entry
            {
                TextColor = (Color)Resources["TextEditorTextColor"]
            };

            textEditor = new Editor
            {
                AutoSize = EditorAutoSizeOption.TextChanges,
                TextColor = (Color)Resources["TextEditorTextColor"]
            };

            roundedFileName = new Frame
            {
                Margin = new Thickness(3, 3, 3, 3),
                Padding = new Thickness(7, 7, 7, 7),
                CornerRadius = 20,
                BackgroundColor =
                       (Color)Resources["TextEditorBackground"],
                Content = fileName,
                IsClippedToBounds = true,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderColor =
                       (Color)Resources["TextEditorBorderColor"]
            };

            roundedEditor = new Frame
            {
                Margin = new Thickness(3, 3, 3, 64),
                Padding = new Thickness(7, 7, 7, 30),
                CornerRadius = 20,
                BackgroundColor =
                       (Color)Resources["TextEditorBackground"],
                Content = textEditor,
                IsClippedToBounds = true,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderColor =
                       (Color)Resources["TextEditorBorderColor"]
            };

            absoluteLayout = new AbsoluteLayout();

            relativeLayout = new RelativeLayout();

            scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Both
            };

            stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            stackLayout.Children.Add(roundedFileName);
            stackLayout.Children.Add(roundedEditor);

            scrollView.Content = stackLayout;

            relativeLayout.Children.Add(scrollView,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Height; }));

            AbsoluteLayout.SetLayoutBounds(save,
                    new Rectangle(0.97, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(save,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(delete,
                    new Rectangle(0.77, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(delete,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(sendToRedactor,
                    new Rectangle(0.57, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(sendToRedactor,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(increaseFont,
                    new Rectangle(0.37, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(increaseFont,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(decreaseFont,
                    new Rectangle(0.17, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(decreaseFont,
                AbsoluteLayoutFlags.PositionProportional);

            absoluteLayout.Children.Add(relativeLayout);
            absoluteLayout.Children.Add(save);
            absoluteLayout.Children.Add(delete);
            absoluteLayout.Children.Add(sendToRedactor);
            absoluteLayout.Children.Add(increaseFont);
            absoluteLayout.Children.Add(decreaseFont);

            Content = absoluteLayout;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save(fileName.Text, textEditor.Text);
        }

        private void IncreaseFont_Clicked(object sender, EventArgs e)
        {
            if (textEditor.FontSize <= 30)
            {
                fileName.FontSize += 2;
                textEditor.FontSize += 2;
            }
        }

        private void DecreaseFont_Clicked(object sender, EventArgs e)
        {
            if (textEditor.FontSize >= 14)
            {
                fileName.FontSize -= 2;
                textEditor.FontSize -= 2;
            }
        }

        async private void DeleteButton_Click(object sender, EventArgs e)
        {
            Delete(fileName.Text);

            await Navigation.PopAsync();
        }

        private void RedactButton_Clicked(object sender, EventArgs e)
        {
            redactorPage.codeEditor.Text += textEditor.Text;
            redactorPage.fileName.Text = fileName.Text;
        }

        async void Delete(string fileName)
        {
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
    }
}