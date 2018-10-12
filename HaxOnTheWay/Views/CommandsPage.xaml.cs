using System;
using System.Collections.Generic;
using HaxOnTheWay.Helpers;
using HaxOnTheWay.Models;
using HaxOnTheWay.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandsPage : ContentPage
    {
        private CommandsPageViewModel viewModel;
        public CommandsPage()
        {
            InitializeComponent();
            Settings.IsSinature = false;
            Settings.IdCommand = 0;
            BindingContext = viewModel = new CommandsPageViewModel();
            viewModel.Navigation = this.Navigation;

            var settings = new ToolbarItem
            {
                Text = ("Ordenes"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
            };

            this.ToolbarItems.Add(settings);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadDataList();
            loadIdCond();
        }

        public async void loadDataList()
        {
            List<Commands> requestCommands = await App.Database.GetAllCommandsAsync();
            List<dataSourceCommands> reqestSourceCommand = new List<dataSourceCommands>();

            foreach (Commands item in requestCommands)
            {
                dataSourceCommands items = new dataSourceCommands();

                String sAdress = (item.sAddress.ToString() + ",  " + item.sCity.ToString());
                String sAdressDelivery = (item.sAddressDelivery.ToString() + ",  " + item.sCityDelivery.ToString());

                items.ColorCommand = item.sColor;

                System.Diagnostics.Debug.WriteLine("items " + items.ColorCommand);

                System.Diagnostics.Debug.WriteLine("item " + item.sColor);
                items.iCommand = item.iCommand;
                items.sAddress = sAdress;
                items.sAddressDelivery = sAdressDelivery;

                reqestSourceCommand.Add(items);
            }

            listCommands.ItemsSource = reqestSourceCommand;
        }
        public async void loadIdCond()
        {
            List<Drivers> requestCommands = await App.Database.GetAllDriversAsync();
            List<dataSourceCommands> reqestSourceCommand = new List<dataSourceCommands>();

            foreach (Drivers item in requestCommands)
            {
                dataSourceCommands items = new dataSourceCommands();

                HaxOnTheWay.Models.Location Longitud = new HaxOnTheWay.Models.Location();
                Longitud.idConductor = item.iDriver;

                //String sAdress = (item.sAddress.ToString() + ",  " + item.sCity.ToString());
                //String sAdressDelivery = (item.sAddressDelivery.ToString() + ",  " + item.sCityDelivery.ToString());

                //items.ColorCommand = item.sColor;
                //items.iCommand = item.iCommand;
                //items.sAddress = sAdress;
                //items.sAddressDelivery = sAdressDelivery;

                reqestSourceCommand.Add(items);
            }

            //listCommands.ItemsSource = reqestSourceCommand;
        }

        public void markers(){
            DependencyService.Register<IRecordVideoPage>();
            DependencyService.Get<IRecordVideoPage>().StartNativeIntentOrActivity();
        }
        public interface IRecordVideoPage
        {
            void StartNativeIntentOrActivity();
        }

        async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                int sIdCommand = int.Parse(((dataSourceCommands)e.SelectedItem).iCommand.ToString());
                //await App.Current.Navigation.PushAsync(new CommandPage(sIdCommand));
                //await App.Current.MainPage.Navigation.PushAsync(new CommandPage(sIdCommand));
                await App.Current.MainPage.Navigation.PushAsync(new CommandPage(sIdCommand));

            }

            return;
        }

        public async System.Threading.Tasks.Task Logout()
        {
            var answer = await DisplayAlert("Notificación", "¿Seguro que desea desconectarse?", "Si", "No");
            if (answer)
            {
                App.Database.dropTables();
                Settings.IsLoggedIn = false;
                App.Current.MainPage = new Views.indexPage();
            }
        }
    }

   

    class dataSourceCommands
    {
        public int iCommand { get; set; }

        public ImageSource oImagen { get; set; }

        public String sAddress { get; set; }

        public String sAddressDelivery { get; set; }

        public String ColorCommand { get; set; }
    }
}
