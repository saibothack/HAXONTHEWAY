using System;
using System.Collections.Generic;
using System.IO;
using HaxOnTheWay.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HaxOnTheWay.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPhotoPage : ContentPage
    {
        static dbLogic database;
        public int iAction { get; set; }
        public int iCommand { get; set; }
        public byte[] sImgStream { get; set; }
        public Command OptionsCommand { get; private set; }

        public CommandPhotoPage(int idCommand, int idAction)
        {
            InitializeComponent();

            iCommand = idCommand;
            iAction = idAction;
            actIndicador.IsEnabled = true;
            actIndicador.IsRunning = true;
            actIndicador.IsVisible = true;
            System.Diagnostics.Debug.WriteLine("before takePhoto");
            takePhotoAsync();
            System.Diagnostics.Debug.WriteLine("after takePhoto");
            OptionsCommand = new Command(Options);

        }

        public void Options()
        {
            App.Current.MainPage.Navigation.PushAsync(new CommandOptionsPage(iCommand));
        }

        async void takePhotoAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            StoreCameraMediaOptions options = new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                PhotoSize = PhotoSize.Small
            };
            System.Diagnostics.Debug.WriteLine("optionss " + options);
            var file = await CrossMedia.Current.TakePhotoAsync(options);

            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("file null");
                return;
            }

            Stream imageStream = file.GetStream();
            BinaryReader br = new BinaryReader(imageStream);
            sImgStream = br.ReadBytes((int)imageStream.Length);

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                System.Diagnostics.Debug.WriteLine("file");
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private void Foto_Clicked(object sender, EventArgs e)
        {
            takePhotoAsync();
        }

        async void Hecho_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("hecho 1");
            Tracings tracings = new Tracings();
            tracings.iCommand = iCommand;
            tracings.iAction = iAction;
            tracings.sNombre = txtNombre.Text;
            tracings.sNotas = txtNotas.Text;
            System.Diagnostics.Debug.WriteLine("hecho 2");
            System.Diagnostics.Debug.WriteLine("tracings "+tracings);
            TracingSend tracingsSend = new TracingSend();
            tracingsSend.iCommand = iCommand;
            tracingsSend.iAction = iAction;
            tracingsSend.sNombre = txtNombre.Text;
            tracingsSend.sNotas = txtNotas.Text;
            tracingsSend.oImagen = Convert.ToBase64String(sImgStream);
            System.Diagnostics.Debug.WriteLine("hecho 3");

            //Commands oCommand = await App.Database.GetCommandAsync(iCommand);
            //oCommand.iEstatus = Constants.iDelivery;

            //await App.Database.UpdateCommandAsync(oCommand);
            //Commands oCommandasdf = await App.Database.GetCommandAsync(iCommand);

            System.Diagnostics.Debug.WriteLine("hecho 4");
            await App.Database.SaveTracingAsync(tracings);
            await App.oServiceManager.SendTracing(tracingsSend);
            System.Diagnostics.Debug.WriteLine("hecho 5");
            App.Database.dropTablesCommands();
            List<Drivers> drivers = await App.Database.GetAllDriversAsync();
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);
            System.Diagnostics.Debug.WriteLine("hecho 6");
            foreach (Commands itm in requestCommands)
            {
                await App.Database.SaveCommandAsync(itm);
            }

            System.Diagnostics.Debug.WriteLine("hecho 7");
           if (iAction == Constants.iDelivery)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CommandPageCal(iCommand));
            }
            else if (iAction == Constants.iPickup)
            {
                App.Current.MainPage = new NavigationPage(new CommandsPage());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("entra else function");
                //App.Current.MainPage.SendBackButtonPressed();
                App.Current.MainPage = new NavigationPage(new CommandsPage());
            }
        }
        private void Commands_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new CommandsPage());
        }
        private void Borrar_Clicked(object sender, EventArgs e)
        {
            PhotoImage.Source = ImageSource.FromStream(() => {
                return null;
            });

            txtNombre.Text = "";
            txtNotas.Text = "";
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new CommandOptionsPage(iCommand));
        }
    }
}
