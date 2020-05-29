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
        public FilePreview()
        {
            InitializeComponent();
        }
    }
}