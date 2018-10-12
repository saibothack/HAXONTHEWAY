using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class indexPage : ContentPage
    {
        public indexPage()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("HaxOnTheWay.Images.HAX_fondo.png");

            loadPage();
        }

        private async void loadPage()
        {
            await Task.Delay(5000);
            App.Current.MainPage = new Views.loginPage();
        }
    }
}
