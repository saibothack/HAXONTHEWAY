using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HaxOnTheWay.Helpers;
using HaxOnTheWay.Models;
using HaxOnTheWay.Views;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;

namespace HaxOnTheWay
{
    public partial class App : Application
    {
        static dbLogic database;
        public static Services.ServiceManager oServiceManager { get; private set; }

        public App()
        {
            InitializeComponent();

            //HaxOnTheWay.Models.Drivers Token = new HaxOnTheWay.Models.Drivers();

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                //Token.sToken =  p.Token;
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");
                //RestService.UpdateComands();
                updateDataBase();

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                }

            };

            oServiceManager = new Services.ServiceManager(new Services.RestService());

            if (Settings.IsLoggedIn)
            {
                if (Settings.IsSinature)
                {
                    MainPage = new NavigationPage(new CommandPageCal(Settings.IdCommand));
                }
                else
                {
                    MainPage = new NavigationPage(new CommandsPage());
                }
            }
            else
            {
                Settings.IsSinature = false;
                MainPage = new indexPage();
            }

        }

        async Task updateDataBase() {
            database.dropTablesCommands();
            List<Drivers> drivers = await database.GetAllDriversAsync();
            //Conlta sus comandas
            List<Commands> requestCommands = await App.oServiceManager.RefreshDataAsync(drivers[0]);

            foreach (Commands itm in requestCommands)
            {
                System.Diagnostics.Debug.WriteLine("entra update");
                //se guardan las comandas del conductor
                await App.Database.SaveCommandAsync(itm);

            }
            //await App.Current.MainPage.Navigation.PushAsync(new CommandsPage());

            var newPage = new CommandsPage();
            //await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PushAsync(newPage);
            //Debug.WriteLine("the new page is now showing");
            //Debug.WriteLine("the new page is dismissed");
            //Debug.WriteLine(Object.ReferenceEquals(newPage, poppedPage));
        }

        public static dbLogic Database
        {
            get
            {
                if (database == null)
                {
                    database = new dbLogic(DependencyService.Get<IFileHelper>().GetLocalFilePath("HaexpressSQLite.db3"));
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            updateDataBase();
            System.Diagnostics.Debug.WriteLine("OnStart");
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            //updateDataBase();
            System.Diagnostics.Debug.WriteLine("OnSleep");
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            //updateDataBase();
            System.Diagnostics.Debug.WriteLine("OnResume");
            // Handle when your app resumes
        }
    }
}
