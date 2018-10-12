using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HaxOnTheWay.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using HaxOnTheWay.Models;
using HaxOnTheWay.Views;
using Plugin.FirebasePushNotification;

namespace HaxOnTheWay.Services
{
    public class RestService : IRestService
    {
        HttpClient client;
        public List<Models.Drivers> ItemsDrivers { get; private set; }
        public List<Models.Commands> ItemsCommands { get; private set; }
        public List<Models.Estatus> ItemsEstatus { get; private set; }
        public List<Models.Coord> ItemsCoord { get; private set; }

        public RestService()
        {
            var authData = string.Format("{0}:{1}", Constants.sUserName, Constants.sPassword);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<Models.Drivers>> LoginAsync(Models.Drivers item)
        {
            //System.Diagnostics.Debug.WriteLine($"TOKEN : {}");


            var uri = new Uri(string.Format(Constants.RestUrl + "/application/drivers/loginMovil.php", string.Empty));
            ItemsDrivers = new List<Models.Drivers>();

            try
            {
                var postDriver = JsonConvert.SerializeObject(item);
                Debug.WriteLine("JSON log: " + postDriver);
                var content = new StringContent(postDriver, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var request = await response.Content.ReadAsStringAsync();
                    ItemsDrivers = JsonConvert.DeserializeObject<List<Models.Drivers>>(request);
                    Debug.WriteLine("response log: " + request + "    " + ItemsDrivers);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

            return ItemsDrivers;
        }
            
        public async Task<List<Models.Commands>> RefreshDataAsync(Models.Drivers item)
        {
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/commands/getCommandsDriver.php", string.Empty));
            ItemsCommands = new List<Models.Commands>();

            try
            {
                var postDriver = JsonConvert.SerializeObject(item);
                var content = new StringContent(postDriver, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    
                    var request = await response.Content.ReadAsStringAsync();

                    ItemsCommands = JsonConvert.DeserializeObject<List<Models.Commands>>(request);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

            return ItemsCommands;
        }

        public async Task<List<Models.Estatus>> GetEstatus()
        {
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/status/getEstatus.php", string.Empty));
            ItemsEstatus = new List<Models.Estatus>();

            try
            {

                    Debug.WriteLine("entra getestatus");
                //var postDriver = JsonConvert.SerializeObject(item);
                var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var request = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("resp estatus "+ request);
                    ItemsEstatus = JsonConvert.DeserializeObject<List<Models.Estatus>>(request);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

            return ItemsEstatus;
        }

        public async Task<List<Models.Coord>> GetCoord(Models.Drivers item){
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/rastreo/getGeocercas.php?dv=" + item.iDriver, string.Empty));
            ItemsCoord = new List<Models.Coord>();
            try{
                var postDriver = JsonConvert.SerializeObject(item);
                var content = new StringContent("", Encoding.UTF8, "application/json");

                Debug.WriteLine("result coord " + uri);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode){
                    var request = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("result coord " + request);
                    ItemsCoord = JsonConvert.DeserializeObject<List<Models.Coord>>(request);
                }
            }
            catch (Exception ex){
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            return ItemsCoord;
        }

        public async Task SaveCommandItemAsync(Models.Commands item, bool isNewItem)
        {
            // RestUrl = http://developer.xamarin.com:8081/api/todoitems
            var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"              TodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
        }

        public async Task SendTracing(Models.TracingSend item)
        {
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/tracing/setTracingPhoto.php", string.Empty));

            try
            {
                var postDriver = JsonConvert.SerializeObject(item);
                var content = new StringContent(postDriver, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var request = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(request);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

        }


        public async Task SendLocation(Models.Location item)
        {
            //loadIdCond();
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/rastreo/setRastreo.php", string.Empty));
            //.WriteLine("ENTRA");
            try
            {
                //Debug.WriteLine("ENTRA TRY");148,244,124,155
                var postDriver = JsonConvert.SerializeObject(item);
                var content = new StringContent(postDriver, Encoding.UTF8, "application/json");
                System.Diagnostics.Debug.WriteLine("JSON " + postDriver);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                Debug.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    var request = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(request);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

        }



        public async Task SendSignture(Models.Signature item)
        {   
            var uri = new Uri(string.Format(Constants.RestUrl + "/application/tracing/setSignature.php", string.Empty));

            try
            {
                var postDriver = JsonConvert.SerializeObject(item);
                Debug.WriteLine("json signature " + postDriver);
                var content = new StringContent(postDriver, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var request = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(request);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

        }
     
    }
}
