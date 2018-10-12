using System;
using System.Collections.Generic;
using System.IO;
using HaxOnTheWay.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPageCal : ContentPage
    {
        public int iCommand { get; set; }
        public int iCalifica { get; set; }

        public SKImage skImage { get; set; }

        public CommandPageCal(int idCommand)
        {
            InitializeComponent();
            iCommand = idCommand;

            imgBueno.Source = ImageSource.FromResource("HaxOnTheWay.Images.bueno_n.png");
            imgRegular.Source = ImageSource.FromResource("HaxOnTheWay.Images.regular.png");
            imgMalo.Source = ImageSource.FromResource("HaxOnTheWay.Images.malo_n.png");

            imgBueno.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnSetScore(Constants.iGood)),
            });

            imgRegular.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnSetScore(Constants.iRegular)),
            });

            imgMalo.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnSetScore(Constants.iBad)),
            });
         
        }

        public async void OnSetScore(int iScore)
        {
            btnCont.IsEnabled = true;
            iCalifica = iScore;
            await DisplayAlert("Notification", "Gracias por calificarnos", "Si");

        }
        private void Hecho_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CommandSignaturePage(iCommand,iCalifica));
        }
    }
}
