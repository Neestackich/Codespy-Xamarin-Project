using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSpy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Redactor : ContentPage
    {
        JDoodle jdoodle;

        public Redactor()
        {
            jdoodle = new JDoodle();

            InitializeComponent();
        }

        private void SendContent_ButtonClick(object sender, EventArgs e)
        {
            jdoodle.SendMessage();
        }
    }
}