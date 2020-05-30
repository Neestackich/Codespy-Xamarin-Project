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
    public partial class Settings : ContentPage
    {
        Dictionary<string, string> languages;

        Redactor redactorPage;

        Picker language;
        Picker version;

        Frame roundedLanguage;
        Frame roundedVersion;

        public Settings(Redactor _redactorPage)
        {
            InitializeComponent();

            languages = new Dictionary<string, string>();

            redactorPage = _redactorPage;

            Linking();
        }

        private void Linking()
        {
            languages = new Dictionary<string, string>();

            languages.Add("csharp", "using System; \nclass Program " +
            "\n{ \nstatic void Main() \n{ " +
            "\nint x = 10; \nint y = 25; \nint z = x + y; " +
            "\nConsole.Write(\"Sum of x + y = \" + z); \n} \n}");

            languages.Add("c", "#include<stdio.h> \nint main() \n{ " +
            "\nint x = 10; \nint y = 25; \nint z = x + y; " +
            "\nprintf(\"Sum of x+y = %i\", z); \n}");

            languages.Add("java", "public class MyClass \n{ " +
            "\npublic static void main(String args[]) \n{ " +
            "\nint x = 10; \nint y = 25; \nint z = x + y;" +
            "\nSystem.out.println(\"Sum of x+y = \" + z); \n} \n}");

            languages.Add("cpp", "#include <iostream> \nusing namespace std; " +
                "\nint main() \n{ \nint x = 10; \nint y = 25; \nint z = x + y; " +
                "\ncout << \"Sum of x+y = \" << z; \n} \n}");

            languages.Add("ruby", "x = 10; \ny = 25; \nz = x + y; " +
                "\nprint \"Sum of x + y = \", z;");

            languages.Add("go", "package main \nimport \"fmt\" " +
                "\nfunc main() {\nx:= 25\ny:= 10\nz:= x + y" +
                "\nfmt.Printf(\"Sum of x + y = %d\", z)\n}");

            language = new Picker
            {
                Title = "Язык",
                TitleColor = (Color)Resources["FontColor"],
                TextColor = (Color)Resources["FontColor"]
            };
            language.Items.Add("C#");
            language.Items.Add("Java");
            language.Items.Add("C");
            language.Items.Add("C++");
            language.Items.Add("Ruby");
            language.Items.Add("GO");
            language.SelectedIndexChanged += Language_SelectedIndexChanged;

            version = new Picker
            {
                Title = "Версия транслятора",
                TitleColor = (Color)Resources["FontColor"],
                TextColor = (Color)Resources["FontColor"]
            };
            version.SelectedIndexChanged += Version_SelectedIndexChanged;

            roundedLanguage = new Frame
            {
                Margin = new Thickness(3, 6, 3, 3),
                Padding = 3,
                CornerRadius = 20,
                BackgroundColor = 
                    (Color)Resources["SettPickerBackground"],
                IsClippedToBounds = true,
                HasShadow = false,
                BorderColor =
                    (Color)Resources["PickerBorderColor"],
                Content = language
            };

            roundedVersion = new Frame
            {
                Margin = new Thickness(3, 6, 3, 3),
                Padding = 3,
                CornerRadius = 20,
                BackgroundColor =
                    (Color)Resources["SettPickerBackground"],
                IsClippedToBounds = true,
                HasShadow = false,
                BorderColor =
                    (Color)Resources["PickerBorderColor"],
                Content = version
            };

            Content = new StackLayout
            {
                Children = { roundedLanguage, roundedVersion }
            };
        }

        private void Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (language.Items[language.SelectedIndex] == "C#")
            {
                redactorPage.codeEditor.Text = languages["csharp"];
                redactorPage.jdoodle.infoToCompileMessage.language = "csharp";

                version.Items.Add("mono 4.2.2");
                version.Items.Add("mono 5.0.0");
                version.Items.Add("mono 5.10.1");
                version.Items.Add("mono 6.0.0");
            }
            else if(language.Items[language.SelectedIndex] == "Java")
            {
                redactorPage.codeEditor.Text = languages["java"];
                redactorPage.jdoodle.infoToCompileMessage.language = "java";

                version.Items.Add("JDK 1.8.0_66");
                version.Items.Add("JDK 9.0.1");
                version.Items.Add("JDK 10.0.1");
                version.Items.Add("JDK 11.0.4");
            }
            else if (language.Items[language.SelectedIndex] == "C")
            {
                redactorPage.codeEditor.Text = languages["c"];
                redactorPage.jdoodle.infoToCompileMessage.language = "c";

                version.Items.Add("GCC 5.3.0");
                version.Items.Add("Zapcc 5.0.0");
                version.Items.Add("GCC 7.2.0");
                version.Items.Add("GCC 8.1.0");
                version.Items.Add("GCC 9.1.0");
            }
            else if (language.Items[language.SelectedIndex] == "C++")
            {
                redactorPage.codeEditor.Text = languages["cpp"];
                redactorPage.jdoodle.infoToCompileMessage.language = "cpp";

                version.Items.Add("GCC 5.3.0");
                version.Items.Add("Zapcc 5.0.0");
                version.Items.Add("GCC 7.2.0");
                version.Items.Add("GCC 8.1.0");
                version.Items.Add("GCC 9.1.0");
            }
            else if (language.Items[language.SelectedIndex] == "Ruby")
            {
                redactorPage.codeEditor.Text = languages["ruby"];
                redactorPage.jdoodle.infoToCompileMessage.language = "ruby";

                version.Items.Add("2.2.4");
                version.Items.Add("2.4.2p198");
                version.Items.Add("2.5.1p57");
                version.Items.Add("2.6.5");
            }
            else
            {
                redactorPage.codeEditor.Text = languages["go"];
                redactorPage.jdoodle.infoToCompileMessage.language = "go";

                version.Items.Add("1.5.2");
                version.Items.Add("1.9.2");
                version.Items.Add("1.10.2");
                version.Items.Add("1.13.1");
            }
        }

        private void Version_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (version.Items[version.SelectedIndex] == "mono 4.2.2")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "0";
            }
            else if (version.Items[version.SelectedIndex] == "mono 5.0.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "1";
            }
            else if (version.Items[version.SelectedIndex] == "mono 5.10.1")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "2";
            }
            else if (version.Items[version.SelectedIndex] == "mono 6.0.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "3";
            }
            else if (version.Items[version.SelectedIndex] == "JDK 1.8.0_66")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "0";
            }
            else if (version.Items[version.SelectedIndex] == "JDK 9.0.1")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "1";
            }
            else if (version.Items[version.SelectedIndex] == "JDK 10.0.1")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "2";
            }
            else if (version.Items[version.SelectedIndex] == "JDK 11.0.4")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "3";
            }
            else if (version.Items[version.SelectedIndex] == "GCC 5.3.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "0";
            }
            else if (version.Items[version.SelectedIndex] == "Zapcc 5.0.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "1";
            }
            else if (version.Items[version.SelectedIndex] == "GCC 7.2.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "2";
            }
            else if (version.Items[version.SelectedIndex] == "GCC 8.1.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "3";
            }
            else if (version.Items[version.SelectedIndex] == "GCC 9.1.0")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "4";
            }
            else if (version.Items[version.SelectedIndex] == "2.2.4")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "0";
            }
            else if (version.Items[version.SelectedIndex] == "2.4.2p198")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "1";
            }
            else if (version.Items[version.SelectedIndex] == "2.5.1p57")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "2";
            }
            else if (version.Items[version.SelectedIndex] == "2.6.5")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "3";
            }
            else if (version.Items[version.SelectedIndex] == "1.5.2")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "0";
            }
            else if (version.Items[version.SelectedIndex] == "1.9.2")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "1";
            }
            else if (version.Items[version.SelectedIndex] == "1.10.2")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "2";
            }
            else if (version.Items[version.SelectedIndex] == "1.13.1")
            {
                redactorPage.jdoodle.infoToCompileMessage.versionIndex = "3";
            }
        }
    }
}