using LoFF.Features.AmClient;
using System;

namespace LoFF.Models
{
    public class Paramz
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public DateTimeOffset departureDate { get; set; }
        public DateTimeOffset? returnDate { get; set; }
        public DateTimeOffset? arrivalBy { get; set; }
        public DateTimeOffset? returnBy { get; set; }
        public int? adults { get; set; }
        public int? children { get; set; }
        public int? infants { get; set; }
        public int? seniors { get; set; }
        public TravelClass2? travelClass { get; set; }
        public string includeAirlines { get; set; }
        public string excludeAirlines { get; set; }
        public bool? nonStop { get; set; }
        public string currency { get; set; }
        public int? maxPrice { get; set; }
        public int? max { get; set; }
    }
}