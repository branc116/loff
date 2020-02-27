using LoFF.Features.AmClient;
using LoFF.Features.Cache;
using LoFF.Features.Helpers;
using LoFF.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoFF.Controllers
{
    [Route("v1")]
    public class AmController : Controller
    {
        private readonly CacheManager _amClient;

        public AmController(CacheManager amClient)
        {
            _amClient = amClient;
        }
        [HttpPost]
        /// <summary>Find the cheapest bookable flights.</summary>
        /// <param name="origin">city/airport [IATA code](http://www.iata.org/publications/Pages/code-search.aspx) from which the traveler will depart, e.g. BOS for Boston</param>
        /// <param name="destination">city/airport [IATA code](http://www.iata.org/publications/Pages/code-search.aspx) to which the traveler is going, e.g. PAR for Paris</param>
        /// <param name="departureDate">the date on which the traveler will depart from the origin to go to the destination. Dates are specified in the [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) YYYY-MM-DD format, e.g. 2017-12-25</param>
        /// <param name="returnDate">the date on which the traveler will depart from the destination to return to the origin. If this parameter is not specified, only one-way itineraries are found. If this parameter is specified, only round-trip itineraries are found. Dates are specified in the [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) YYYY-MM-DD format, e.g. 2018-02-28</param>
        /// <param name="arrivalBy">the date and time by which the last flight of the outbound should arrive at the destination. Datetimes are specified in the [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) YYYY-MM-ddThh:mm format, e.g. 2016-12-31T23:59</param>
        /// <param name="returnBy">the date and time by which the last flight of the inbound should arrive at the origin. Datetimes are specified in the [ISO 8601](https://en.wikipedia.org/wiki/ISO_8601) YYYY-MM-ddThh:mm format, e.g. 2017-12-31T12:01</param>
        /// <param name="adults">the number of adult travelers (age 12 or older on date of departure). If specified, this number should be greater than or equal to 0</param>
        /// <param name="children">the number of child travelers (older than age 2 and younger than age 12 on date of departure) who will each have their own separate seat. If specified, this number should be greater than or equal to 0</param>
        /// <param name="infants">the number of infant travelers (whose age is less or equal to 2 on date of departure). Infants travel on the lap of an adult or a senior traveler, and thus the number of infants must not exceed the sum of the number of adults and seniors. If specified, this number should be greater than or equal to 0</param>
        /// <param name="seniors">the number of senior travelers (age 65 or older on date of departure). If specified, this number should be greater than or equal to 0</param>
        /// <param name="travelClass">most of the flight time should be spent in a cabin of this quality or higher. The accepted travel class is economy, premium economy, business or first class. If no travel class is specified, the search considers any travel class</param>
        /// <param name="includeAirlines">if specified, the flight offer will include at least one segment per bound marketed by one of these airlines. Airlines are specified as [IATA airline codes](http://www.iata.org/publications/Pages/code-search.aspx) and are comma-separated, e.g. 6X,7X,8X</param>
        /// <param name="excludeAirlines">if specified, the flight offer will exclude all the flights marketed by one of these airlines. Airlines are specified as [IATA airline codes](http://www.iata.org/publications/Pages/code-search.aspx) and are comma-separated, e.g. 6X,7X,8X</param>
        /// <param name="nonStop">if set to true, the search will find only flights going from the origin to the destination with no stop in between</param>
        /// <param name="currency">the preferred currency for the flight offers. Currency is specified in the [ISO 4217](https://en.wikipedia.org/wiki/ISO_4217) format, e.g. EUR for Euro</param>
        /// <param name="maxPrice">maximum price of the flight offers to find, in EUR unless some other currency is specified. By default, no limit is applied. If specified, the value should be a positive number with no decimals</param>
        /// <param name="max">maximum number of flight offers to return. 
        /// 
        /// If specified, the value should be between 1 and 250. When not specified, system uses the default value **50**.</param>
        /// <returns>Success</returns>
        public async Task<FlightOffers> GetFlightOffers([FromBody]Paramz paramz)
        {
            var res = await _amClient.GetAsync(paramz);
            return res;
        }
        [HttpGet("meta")]
        public List<MetaData> GetMetaData()
        {
            return KeyGenHelper.GetMetaData<Paramz>();
        }
    }
}

