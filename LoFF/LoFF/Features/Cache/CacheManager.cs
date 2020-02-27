using LoFF.Features.AmClient;
using LoFF.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoFF.Features.Cache
{
    public class CacheManager
    {
        private readonly List<ICache> _caches;
        private readonly ILogger<CacheManager> _logger;

        public CacheManager(ILogger<CacheManager> logger, IServiceProvider services)
        {
            _caches = services.GetServices<ICache>().OrderBy(i => i.Prioirity).ToList();
            _logger = logger;
        }
        public async Task<FlightOffers> GetAsync(Paramz key)
        {
            var missed = new List<ICache>();
            FlightOffers val = null;
            foreach (var source in _caches)
            {
                val = await source.GetAsync(key);
                if (val is null)
                {
                    _logger.LogInformation($"{source.GetType()} cache miss.");
                    missed.Add(source);
                }
                else
                {
                    break;
                }
            }
            await FillCacheAsync(missed, key, val);
            return val;
        }

        private async Task FillCacheAsync(List<ICache> missed, Paramz key, FlightOffers val)
        {
            if (val is null)
                return;
            foreach (var miss in missed)
            {
                await miss.SetAsync(key, val);
            }
            return;
        }
    }
}
