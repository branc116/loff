using LoFF.Features.AmClient;
using System.Threading.Tasks;

namespace LoFF.Features.Cache
{
    public interface ICache
    {
        int Prioirity { get; }
        Task<FlightOffers> GetAsync(LoFF.Models.Paramz key);
        Task SetAsync(LoFF.Models.Paramz key, FlightOffers fo);
        Task DeleteOldAsync();
    }
}
