using HaxOnTheWay.Helpers;
using HaxOnTheWay.Models;
using HaxOnTheWay.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HaxOnTheWay.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Commands
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Properties
        private Drivers _driver = new Drivers();

        public Drivers driver
        {
            get { return _driver; }
            set { SetProperty(ref _driver, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        #endregion

        public LoginPageViewModel()
        {
            LoginCommand = new Command(Login);
        }

        public async void Login()
        {
            IsBusy = true;
            Title = string.Empty;
            try
            {
                if (driver.sEmail != null)
                {
                    if (driver.sPhone != null)
                    {
                        if (await Task.Run(() => LoginTask())) {
                            App.Current.MainPage = new NavigationPage(new CommandsPage());
                        } else {
                            IsBusy = false;
                            await App.Current.MainPage.DisplayAlert("Notificación", "Sus datos no son correctos", "Ok");
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        await App.Current.MainPage.DisplayAlert("Notificación", "La contraseña es requerido", "Ok");
                    }
                }
                else
                {
                    IsBusy = false;
                    await App.Current.MainPage.DisplayAlert("Notificación", "El email es requerido", "Ok");
                }

            }
            catch (Exception e)
            {
                IsBusy = false;
                System.Diagnostics.Debug.WriteLine("error " + e);
                await App.Current.MainPage.DisplayAlert("Notificación", "Los datos ingresados son incorrectos, por favor verifique" + e, "Ok");
            }
        }

        async Task<Boolean> LoginTask() {
            
                List<Drivers> requestDriver = await App.oServiceManager.LoginAsync(driver);
                //System.Diagnostics.Debug.WriteLine("task 1");

                if (requestDriver.Count() > 0)
                {
                    if (requestDriver[0].iDriver != 0)
                    {
                        //System.Diagnostics.Debug.WriteLine("task -- " + requestDriver[0]);
                        //Se guarda el usuario
                        await App.Database.SaveDriverAsync(requestDriver[0]);
                        //System.Diagnostics.Debug.WriteLine("task 2");            
                        HaxOnTheWay.Models.Location Longitud = new HaxOnTheWay.Models.Location();
                        Longitud.idConductor = requestDriver[0].iDriver;
                        //System.Diagnostics.Debug.WriteLine("id conductor " + Longitud.idConductor);

                        //Conlta sus comandas
                        List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(requestDriver[0]);

                        if (requestCommands.Count() > 0)
                        {
                            foreach (Commands itm in requestCommands)
                            {
                            
                                //se guardan las comandas del conductor
                                await App.Database.SaveCommandAsync(itm);
                            }
                        }
                        //List<Estatus> requestDriver = await App.oServiceManager.LoginAsync(driver);
                        List<Estatus> requestEstatus = await App.oServiceManager.GetEstatus();

                        if (requestEstatus.Count() > 0)
                        {
                            foreach (Estatus itm in requestEstatus)
                            {
                                //se guardan las comandas del conductor
                                await App.Database.SaveEstatusAsync(itm);
                            }
                        }


                    List<Coord> requestCoord = await App.oServiceManager.GetCoord(requestDriver[0]);
                        if (requestCoord.Count() > 0){
                            foreach (Coord itm in requestCoord){
                            //se guardan las comandas del conductor
                                await App.Database.SaveCoordAsync(itm);
                            }
                        }



                        Settings.IsLoggedIn = true;
                    }
                }
                else
                {
                    IsBusy = false;
                    await App.Current.MainPage.DisplayAlert("Notificación", "Sus datos no son correctos por favor verifique", "Ok");
                }

            return Settings.IsLoggedIn;
        }

    
    }
}
