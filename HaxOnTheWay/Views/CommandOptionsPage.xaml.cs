using System;
using System.Collections.Generic;
using HaxOnTheWay.Models;
using Xamarin.Forms;    
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandOptionsPage : ContentPage
    {
        public int iCommand { get; set; }

        public CommandOptionsPage(int idCommand)
        {
            InitializeComponent();
            iCommand = idCommand;
        }

        private void Button_ClickedAsync(object sender, EventArgs e)
        {
            //setNewStatus(Constants.iNoStock);
            NavigationContinue(Constants.iNoStock);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //setNewStatus(Constants.iAbsent);
            NavigationContinue(Constants.iAbsent);
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            //setNewStatus(Constants.iRejected);
            NavigationContinue(Constants.iRejected);
        }
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            //setNewStatus(Constants.iCancelado);
            NavigationContinue(Constants.iCancelado);
        }
        private void Commands_Clicked(object sender, EventArgs e){
            App.Current.MainPage = new NavigationPage(new CommandsPage());
        }
        private async void NavigationContinue(int idAction)
        {
          
            Tracings tracings = new Tracings();
            tracings.iCommand = iCommand;
            tracings.iAction = idAction;

            TracingSend tracingsSend = new TracingSend();
            tracingsSend.iCommand = iCommand;
            tracingsSend.iAction = idAction;

            //Commands oCommand = await App.Database.GetCommandAsync(iCommand);
            //oCommand.iEstatus = Constants.iDelivery;
            //await App.Database.UpdateCommandAsync(oCommand);
            App.Database.dropTablesCommands();
            List<Drivers> drivers = await App.Database.GetAllDriversAsync();
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);
            foreach (Commands itm in requestCommands){
                await App.Database.SaveCommandAsync(itm);
            }

            await App.Database.SaveTracingAsync(tracings);
            await App.oServiceManager.SendTracing(tracingsSend);

            Commands item = await App.Database.GetCommandAsync(iCommand);
//            await App.Database.DeleteCommandAsync(item);


            App.Current.MainPage = new NavigationPage(new CommandPhotoPage(iCommand, idAction));
            //await App.Current.MainPage.Navigation.PushAsync(new CommandPhotoPage(iCommand, idAction));
        }
        private async void setNewStatus(int iAction)
        {
            Tracings tracings = new Tracings();
            tracings.iCommand = iCommand;
            tracings.iAction = iAction;

            TracingSend tracingsSend = new TracingSend();
            tracingsSend.iCommand = iCommand;
            tracingsSend.iAction = iAction;

            //Commands oCommand = await App.Database.GetCommandAsync(iCommand);
            //oCommand.iEstatus = Constants.iDelivery;
            //await App.Database.UpdateCommandAsync(oCommand);
            App.Database.dropTablesCommands();
            List<Drivers> drivers = await App.Database.GetAllDriversAsync();
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);
            foreach (Commands itm in requestCommands){
                await App.Database.SaveCommandAsync(itm);
            }

            await App.Database.SaveTracingAsync(tracings);
            await App.oServiceManager.SendTracing(tracingsSend);

            //Commands item = await App.Database.GetCommandAsync(iCommand);
            //await App.Database.DeleteCommandAsync(item);
        

            //App.Current.MainPage = new NavigationPage(new CommandsPage());
        }
    }
}
