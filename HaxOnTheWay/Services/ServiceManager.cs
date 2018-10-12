using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaxOnTheWay.Services
{
    public class ServiceManager
    {
        IRestService restService;

        public ServiceManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<Models.Drivers>> LoginAsync(Models.Drivers item)
        {
            return restService.LoginAsync(item);
        }

        public Task<List<Models.Commands>> RefreshDataAsync(Models.Drivers item)
        {
            return restService.RefreshDataAsync(item);
        }

        public Task<List<Models.Coord>> GetCoord(Models.Drivers item)
        {
            return restService.GetCoord(item);
        }

        public Task SaveCommandItemAsync(Models.Commands item, bool isNewItem = false)
        {
            return restService.SaveCommandItemAsync(item, isNewItem);
        }

        public Task<List<Models.Estatus>> GetEstatus()
        {
            return restService.GetEstatus();
        }

        public Task SendTracing(Models.TracingSend item)
        {
            return restService.SendTracing(item);
        }

        public Task SendSignture(Models.Signature item)
        {
            return restService.SendSignture(item);
        }
    }
}
