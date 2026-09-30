using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Config
{
    public class GarageConfig
    {
        public int NumberOfFloors { get; set; } = 10;
        public int SpotsPerFloor { get; set; } = 50;
        /*Här har vi get  set på SpotCapacity OCH det är för för ifall
          "ägaren" vill ändra parkerings platsernas storlek så kan hen göra det.*/
        public int SpotCapacity { get; set; } = 4;
        public int TotalSpots => NumberOfFloors * SpotsPerFloor;
        public int FreeMinutes { get; set; } = 5;

        /*OrdinalIgnoreCase för att slippa att det ska fela mellan stora eller små bokstäver*/
        public Dictionary<string, decimal> HourlyRates { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Car"] = 20m,
            ["MC"] = 10m,
            ["Bike"] = 5m,
            ["Bus"] = 80,
        };
        public GarageConfig() { }

        public decimal GetHourlyRate(string vehicleType)
        {
            if (HourlyRates.TryGetValue(vehicleType, out decimal rate))
            {
                return rate;
            }
            return 20m;
        }

        /*half onödiga valderingar MEN används i början för att hitta fel som kan hända.*/
        public void Validate()
        {
            if (NumberOfFloors <= 0)
                throw new ArgumentException("Antal våningar måste vara över 1.", nameof(NumberOfFloors));

            if (SpotsPerFloor <= 0)
                throw new ArgumentException("Antal platser per plan måste vara minst 1", nameof(SpotsPerFloor));

            if (SpotCapacity <= 0)
                throw new ArgumentException("Kapaciteten för platser måste vara större än 0.", nameof(SpotCapacity));

            if (FreeMinutes < 0)
                throw new ArgumentException("Free-Minutes kan inte vara negativa -.-", nameof(FreeMinutes));
        }
    }
}