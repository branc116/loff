using LoFF.Features.AmClient;
using LoFF.Features.Cache;
using LoFF.Features.Helpers;
using LoFF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoFF.Features.DataCache
{
    public class CachedDataContext : ICache
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly double _holdCache;
        public int Prioirity => 0;

        public CachedDataContext(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _holdCache = configuration.GetValue<double>("HoldCacheForHours");
        }

        public async Task<FlightOffers> GetAsync(Paramz key)
        {
            var strKey = key.GenerateKey();
            var critTime = DateTime.Now.AddHours(-_holdCache);
            var json = await _dataContext.Entries.Where(i => i.Key == strKey && i.DateAdded > critTime)
                .Select(i => i.JsonValue)
                .FirstOrDefaultAsync();
            return json switch
            {
                null => null,
                _ => Newtonsoft.Json.JsonConvert.DeserializeObject<FlightOffers>(json)
            };
        }

        public async Task SetAsync(Paramz key, FlightOffers fo)
        {
            var strKey = key.GenerateKey();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(fo);
            await _dataContext.Entries.AddAsync(new Models.Entry
            {
                DateAdded = DateTime.Now,
                JsonValue = json,
                Key = strKey
            });
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteOldAsync()
        {
            var critDate = DateTime.Now.AddHours(_holdCache);
            var toRemove = _dataContext.Entries.Where(i => i.DateAdded < critDate);
            _dataContext.Entries.RemoveRange(toRemove);
            await _dataContext.SaveChangesAsync();
        }
    }
}
