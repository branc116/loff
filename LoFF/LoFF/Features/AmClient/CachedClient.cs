using LoFF.Features.Cache;
using LoFF.Features.Helpers;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoFF.Features.AmClient
{
    public class CachedClient : Client, ICache
    {
        public CachedClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public int Prioirity => 10;

        public async Task DeleteOldAsync()
        {
            return;
        }

        public async Task<FlightOffers> GetAsync(LoFF.Models.Paramz key)
        {
            var paramz = key.GetObjDict();
            var meth = typeof(Client).GetMethods().FirstOrDefault(i => i.GetParameters().Length == paramz.Count && i.Name == nameof(GetFlightOffersAsync));
            var meths = typeof(Client).GetMethods().Select(i => (i.Name, i.GetParameters().Length)).ToArray();
            var pars = meth.GetParameters();
            var parsValue = new object[pars.Length];
            for (int i = 0; i < pars.Length; i++)
            {
                parsValue[i] = paramz[pars[i].Name];
            }
            var fos = meth.Invoke(this, parsValue) as Task<FlightOffers>;
            return await fos;
        }

        public async Task SetAsync(LoFF.Models.Paramz key, FlightOffers fo)
        {
            return;
        }
        //void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        //{
        //    settings.
        //}
    }
}
