using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.IO;
using System.Transactions;
using System.Text.RegularExpressions;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redactor : ContentPage
    {
        internal JDoodle jdoodle;

        Image noConnection;

        ImageButton sendToUrl;
        ImageButton saveFile;
        ImageButton deleteFile;
        ImageButton increaseFont;
        ImageButton decreaseFont;

        Label connectionMessage;
        Label stringNumberBar;

        internal Editor codeEditor;

        Frame roundedEditor;
        Frame roundedStringNumberBar;

        internal ToolbarItem fileName;
        internal ToolbarItem languageName;

        AbsoluteLayout absoluteLayout;

        RelativeLayout relativeLayout;

        ScrollView scrollView;

        StackLayout stackView;
        
        FileManaging fileManagingPage;

        Regex regex;

        MatchCollection matches;

        public Redactor(FileManaging _fileManagingPage)
        {
            jdoodle = new JDoodle();

            fileManagingPage = _fileManagingPage;

            InitializeComponent();

            Linking();

            ConnectivityCheck();

            CrossConnectivity.Current.ConnectivityChanged +=
                Current_ConnectivityChanged;
        }

        private void Current_ConnectivityChanged(object sender,
           ConnectivityChangedEventArgs e)
        {
            ConnectivityCheck();
        }

        private void Linking()
        {
            regex = new Regex(@"\n");

            fileName = new ToolbarItem
            {
                Text = "newFile.txt"
            };
            fileName.Clicked += FileName_Clicked;

            sendToUrl = new ImageButton
            {
                Padding = 10,
                Source = "processorPurple.png",
                Style = (Style)Resources["RedactorButtonsStyle"]
            };
            sendToUrl.Clicked += SendContent_ButtonClick;

            saveFile = new ImageButton
            {
                Padding = 10,
                Source = "save.png",
                Style = (Style)Resources["RedactorButtonsStyle"]
            };
            saveFile.Clicked += SaveFile_Clicked;

            deleteFile = new ImageButton
            {
                Padding = 10,
                Source = "delete.png",
                Style = (Style)Resources["RedactorButtonsStyle"]
            };
            deleteFile.Clicked += DeleteFile_Clicked;

            increaseFont = new ImageButton
            {
                Padding = 10,
                Source = "plus.png",
                Style = (Style)Resources["RedactorButtonsStyle"]
            };
            increaseFont.Clicked += IncreaseFont_Clicked;

            decreaseFont = new ImageButton
            {
                Padding = 10,
                Source = "minus.png",
                Style = (Style)Resources["RedactorButtonsStyle"]
            };
            decreaseFont.Clicked += DecreaseFont_Clicked;

            codeEditor = new Editor
            {
                FontFamily = "Consolas",
                Text = new string('\n', 21),
                TextColor = (Color)Resources["CodeEditorTextColor"],
                AutoSize = EditorAutoSizeOption.TextChanges,
                BackgroundColor =
                    (Color)Resources["CodeEditorBackground"]
            };
            codeEditor.TextChanged += StringNumberBar_Change;

            stringNumberBar = new Label
            {
                TextColor = (Color)Resources["NumberBarTextColor"],
                FontSize = codeEditor.FontSize,
                HorizontalOptions = LayoutOptions.Center
            };

            roundedStringNumberBar = new Frame
            {
                Margin = new Thickness(3, 3, 0, 64),
                Padding = new Thickness(5, 17, 0, 10),
                CornerRadius = 20,
                IsClippedToBounds = true,
                HasShadow = false,
                BorderColor =
                   (Color)Resources["CodeEditorBorderColor"],
                BackgroundColor =
                   (Color)Resources["CodeEditorBackground"],
                HeightRequest = codeEditor.HeightRequest,
                Content = stringNumberBar
            };

            NumberBarFilling();

            roundedEditor = new Frame
            {
                Margin = new Thickness(0, 3, 3, 64),
                Padding = new Thickness(7, 7, 7, 30),
                CornerRadius = 20,
                BackgroundColor =
                   (Color)Resources["CodeEditorBackground"],
                IsClippedToBounds = true,
                HasShadow = false,
                Content = codeEditor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderColor =
                   (Color)Resources["CodeEditorBorderColor"]
            };

            connectionMessage = new Label
            {
                Text = "Подключение отсутствует",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            noConnection = new Image
            {
                Source = "no.png",
                HeightRequest = 100,
                WidthRequest = 100
            };

            absoluteLayout = new AbsoluteLayout();

            relativeLayout = new RelativeLayout();

            scrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Both
            };

            stackView = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            stackView.Children.Add(roundedStringNumberBar);
            stackView.Children.Add(roundedEditor);

            scrollView.Content = stackView;

            //ресайз 
            relativeLayout.Children.Add(scrollView,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height; }));

            AbsoluteLayout.SetLayoutBounds(saveFile,
                new Rectangle(0.97, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(saveFile,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(deleteFile,
               new Rectangle(0.77, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(deleteFile,
                AbsoluteLayoutFlags.PositionProportional);

            AbsoluteLayout.SetLayoutBounds(sendToUrl,
                new Rectangle(0.57, 0.98, 50, 50));
            AbsoluteLayout.SetLayoutFlags(sendToUrl,
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
            absoluteLayout.Children.Add(saveFile);
            absoluteLayout.Children.Add(deleteFile);
            absoluteLayout.Children.Add(sendToUrl);
            absoluteLayout.Children.Add(increaseFont);
            absoluteLayout.Children.Add(decreaseFont);

            if (this.ToolbarItems.Count == 0)
            {
                this.ToolbarItems.Add(fileName);
            }
        }

        private void ConnectivityCheck()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Content = absoluteLayout;
            }
            else
            {
                Content = new Grid
                {
                    Padding = 35,
                    Children = { noConnection }
                };
            };
        }

        async private void FileName_Clicked(object sender, EventArgs e)
        {
            if ((fileName.Text = await DisplayPromptAsync("Название файла", "Введите название файла")) == "")
            {
                fileName.Text = "newFile.txt";
            }
        }

        async private void SaveFile_Clicked(object sender, EventArgs e)
        {
            if ((fileName.Text = await DisplayPromptAsync("Название файла", "Введите название файла")) == "")
            {
                fileName.Text = "newFile.txt";
            }

            Save(fileName.Text, codeEditor.Text);
        }

        private void DeleteFile_Clicked(object sender, EventArgs e)
        {
            Delete(fileName.Text);

            codeEditor.Text = new string('\n', 21);

            fileName.Text = "newFile.txt";
        }

        private void IncreaseFont_Clicked(object sender, EventArgs e)
        {
            if(codeEditor.FontSize <= 30)
            {
                codeEditor.FontSize += 2;
                stringNumberBar.FontSize += 2;
            }
        }

        private void DecreaseFont_Clicked(object sender, EventArgs e)
        {
            if (codeEditor.FontSize >= 14)
            {
                codeEditor.FontSize -= 2;
                stringNumberBar.FontSize -= 2;
            }
        }

        private async void SendContent_ButtonClick(object sender, EventArgs e)
        {
            jdoodle.SendMessage(codeEditor.Text);

            await DisplayAlert("Compiler message",
                $"Result: {jdoodle.compilerResult.output}\n" +
                $"Status code: {jdoodle.compilerResult.statusCode}\n" +
                $"Memory used: {jdoodle.compilerResult.memory}\n" +
                $"Cpu time: {jdoodle.compilerResult.cpuTime}", "Ok");
        }

        async void Delete(string fileName)
        {
            // удаляем файл из списка
            await DependencyService.Get<IFileManaging>().DeleteAsync(fileName);
        }

        internal async void Save(string fileName, string fileText)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return;
            } 
            else if (await DependencyService.Get<IFileManaging>().ExistAsync(fileName))
            {
                // запрашиваем разрешение на перезапись
                bool isRewrited = await DisplayAlert("Подверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");

                if (isRewrited == false) return;
            }

            // перезаписываем файл
            await DependencyService.Get<IFileManaging>().SaveTextAsync(fileName, fileText);
        }

        private void NumberBarFilling()
        {
            matches = regex.Matches(codeEditor.Text);

            stringNumberBar.Text = null;

            for (int i = 0; i <= matches.Count; i++)
            {
                stringNumberBar.Text += $"{i + 1}" + 
                    new string(' ', (i == 0) ? 0 : (int)Math.Log10(Math.Abs(i))) + '\n';
            }
        }

        private void StringNumberBar_Change(object sender, TextChangedEventArgs e)
        {
            NumberBarFilling();
        }
    }
}