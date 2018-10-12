using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaxOnTheWay.Services
{
    public interface IRestService
    {
        Task<List<Models.Drivers>> LoginAsync(Models.Drivers item);

        Task<List<Models.Commands>> RefreshDataAsync(Models.Drivers item);

        Task<List<Models.Coord>> GetCoord(Models.Drivers item);

        Task<List<Models.Estatus>> GetEstatus();

        Task SaveCommandItemAsync(Models.Commands item, bool isNewItem);

        //Task GetEstatus();

        Task SendTracing(Models.TracingSend item);

        Task SendSignture(Models.Signature item);
    }
}
