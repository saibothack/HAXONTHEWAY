using System;
using System.Collections.Generic;
using HaxOnTheWay.Helpers;
using HaxOnTheWay.Models;
using HaxOnTheWay.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Net;
using System.Threading.Tasks;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPage : ContentPage
    {
        public int iCommand { get; set; }
        public Command OptionsCommand { get; private set; }

        public CommandPage(int idCommand)
        {
            InitializeComponent();

            btnConfirm.IsEnabled = false;
            btnPickup.IsEnabled = false;
            btnDelivery.IsEnabled = false;

            iCommand = idCommand;

            ImageSource img = ImageSource.FromResource("HaxOnTheWay.Images.pointer.png");
            imgPikup.IsVisible = true;
            imgPikup.Source = img;
            imgDelivery.Source = img;
            loadCommandDetail(iCommand);
            var settings = new ToolbarItem
            {
                Text = ("Orden #" + idCommand),
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
            };

            this.ToolbarItems.Add(settings);

            sPhoneLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnPhoneTapped(sPhone.Text)),
            });

            sPhoneDeliveryLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnPhoneTapped(sPhoneDelivery.Text)),
            });

            imgPikupLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnNavigateButtonClicked(sAddress.Text + sCity.Text + sCP.Text))
            });

            imgDeliveryLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                //Command = new Command(() => OnNavigateButtonClicked(sAddressDelivery.Text + sCityDelivery.Text + sCPDelivery.Text))
                Command = new Command(() => OnNavigateButtonClicked(sAddressDelivery.Text + sCityDelivery.Text))
            });
        }

        void OnNavigateButtonClickedConfirm(object sender, EventArgs e)
        {
            editCommand();
        }
        private void Commands_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CommandsPage());
        }
        private async void editCommand()
        {
            TracingSend tracingsSend = new TracingSend();
            tracingsSend.iCommand = iCommand;
            tracingsSend.iAction = Constants.iConfirm;
            await App.oServiceManager.SendTracing(tracingsSend);

            //Commands oCommand = await App.Database.GetCommandAsync(iCommand);
            //oCommand.iEstatus = Constants.iConfirm;
            //await App.Database.UpdateCommandAsync(oCommand);

            App.Database.dropTablesCommands();
            List<Drivers> drivers = await App.Database.GetAllDriversAsync();
            //Conlta sus comandas
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);

            foreach (Commands itm in requestCommands)
            {
                //System.Diagnostics.Debug.WriteLine("entra update");
                //se guardan las comandas del conductor
                await App.Database.SaveCommandAsync(itm);
            }

            //App.Current.MainPage.SendBackButtonPressed();
            var newPage = new CommandsPage();
            //await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PushAsync(newPage);
        }

        async void OnNavigateButtonClickedPikup(object sender, EventArgs e)
        {
            if (await DisplayAlert(
                             "Confirmación",
                             "¿Seguro que desea realizar la operación?",
                             "Si",
                             "No"))
            {
                NavigationContinue(Constants.iPickup);
            }
        }

        async void OnNavigateButtonClickedDelivery(object sender, EventArgs e)
        {
            if (await DisplayAlert(
                             "Confirmación",
                             "¿Seguro que desea realizar la operación?",
                             "Si",
                             "No"))
            {
                NavigationContinue(Constants.iDelivery);
            }
        }

        async void NavigationContinue(int idAction)
        {
            var answer = await DisplayAlert("Notificación", "¿Quiere agregar una foto?", "Si", "No");
            if (answer)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CommandPhotoPage(iCommand, idAction));
            }
            else
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
                await App.Database.SaveTracingAsync(tracings);
                await App.oServiceManager.SendTracing(tracingsSend);

                App.Database.dropTablesCommands();
                List<Drivers> drivers = await App.Database.GetAllDriversAsync();
                List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);

                foreach (Commands itm in requestCommands)
                {
                    await App.Database.SaveCommandAsync(itm);
                }

                if (idAction == Constants.iPickup)
                {
                    App.Current.MainPage = new NavigationPage(new CommandsPage());
                }
                if (idAction == Constants.iDelivery)
                {
                    Settings.IsSinature = true;
                    Settings.IdCommand = iCommand;
                    App.Current.MainPage = new NavigationPage(new CommandPageCal(iCommand));
                }
                else
                {
                    App.Current.MainPage.SendBackButtonPressed();
                }
            }
        }
        void OnNavigateButtonClicked(String sAddress)
        {
            if (!string.IsNullOrWhiteSpace(sAddress))
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        Device.OpenUri(
                            new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(sAddress))));
                        break;
                    case Device.Android:
                        Device.OpenUri(
                            new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(sAddress))));
                        break;
                    case Device.WinPhone:
                        Device.OpenUri(
                            new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(sAddress))));
                        break;
                }
            }
        }

        public async void OnPhoneTapped(String sNumber)
        {
            if (await this.DisplayAlert(
                             "Marcar a número",
                             "Llamar " + sNumber + "?",
                             "Si",
                             "No"))
            {
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                {
                    dialer.Dial(sNumber);
                }
            }
        }

        private async void loadCommandDetail(int iCommand)
        {
            Models.Commands lsCommand = await App.Database.GetCommandAsync(iCommand);

            //set values for labels
            
            sTypeCommand.Text = lsCommand.sTypeCommand;

            if (lsCommand.sTypeCommandColor != "#000000") {
                sTypeCommand.TextColor = Color.FromHex(lsCommand.sTypeCommandColor);
            }

            sSubservicio.Text = lsCommand.sSubservices;
            if (lsCommand.sSubservicesColor != "#000000") {
                sSubservicio.TextColor = Color.FromHex(lsCommand.sSubservicesColor);
            }

            //sStatus.Text = lsCommand.sStatus;
            //if (lsCommand.sColor != "#000000")
            //{
                //sStatus.TextColor = Color.FromHex(lsCommand.sColor);
            //}

            sDate.Text = lsCommand.sDate;
            sSchedules.Text = lsCommand.sSchedule;
            sCompany.Text = lsCommand.sCompany;
            sAddress.Text = lsCommand.sAddress;
            sCity.Text = lsCommand.sCity;
            sCP.Text = lsCommand.sCP;
            sContact.Text = lsCommand.sContact;
            sPhone.Text = lsCommand.sPhone;
            sCompanyDelivery.Text = lsCommand.sCompanyDelivery;
            sAddressDelivery.Text = lsCommand.sAddressDelivery;
            sCityDelivery.Text = lsCommand.sCityDelivery;
            //sCPDelivery.Text = lsCommand.sCPDelivery;
            sContactDelivery.Text = lsCommand.sContactDelivery;
            sPhoneDelivery.Text = lsCommand.sPhoneDelivery;
            sQuanty.Text = lsCommand.sQuanty;
            sDescription.Text = lsCommand.sDescription;
            sReference.Text = lsCommand.sReference;
            sInstruction.Text = lsCommand.sInstruction;
            buttonsEnabledDisabled(lsCommand.iEstatus);
        }

        private void buttonsEnabledDisabled(int iEstatus)
        {
            System.Diagnostics.Debug.WriteLine("estatus " + iEstatus);
            System.Diagnostics.Debug.WriteLine("pending " + Constants.iPending);
            if (iEstatus == Constants.iPending)
            {
                
                btnConfirm.IsEnabled = true;
                btnPickup.IsEnabled = false;
                btnDelivery.IsEnabled = false;
            }
            else if (iEstatus == Constants.iConfirm)
            {
                System.Diagnostics.Debug.WriteLine("entra confirm");
                btnConfirm.IsEnabled = false;
                btnPickup.IsEnabled = true;
                btnDelivery.IsEnabled = false;
            }
            else if (iEstatus == Constants.iPickup)
            {
                System.Diagnostics.Debug.WriteLine("entra pickup");
                btnConfirm.IsEnabled = false;
                btnPickup.IsEnabled = false;
                btnDelivery.IsEnabled = true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("entra else");
                btnConfirm.IsEnabled = false;
                btnPickup.IsEnabled = false;
                btnDelivery.IsEnabled = false;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new CommandOptionsPage(iCommand));
        }
    }
}
