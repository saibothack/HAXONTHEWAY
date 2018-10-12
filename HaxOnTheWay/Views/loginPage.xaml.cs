using System;
using System.Collections.Generic;
using HaxOnTheWay.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginPage : ContentPage
    {
        private LoginPageViewModel viewModel;
        public loginPage()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("HaxOnTheWay.Images.Hax_on.png");

            BindingContext = viewModel = new LoginPageViewModel();
            viewModel.Navigation = this.Navigation;
        }
    }
}
