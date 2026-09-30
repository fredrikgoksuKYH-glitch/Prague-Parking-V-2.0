using PragueParking.Core.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PragueParking.Core.Models
{
    public class ParkingGarage
    {
        public GarageConfig Config { get; }
        public List<ParkingSpot> Spots { get; set; } = new();

        public ParkingGarage(GarageConfig config, List<ParkingSpot>? existingSpots = null)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Config.Validate();

            if (existingSpots != null && existingSpots.Count > 0)
            {
                Spots = existingSpots;
            }
            else
            {
                InitializeSpots();
            }
        }

        private void InitializeSpots()
        {
            int currentSpotNumber = 1;

            for (int floor = 1; floor <= Config.NumberOfFloors; floor++)
            {
                for (int spot = 1; spot <= Config.SpotsPerFloor; spot++)
                {
                    Spots.Add(new ParkingSpot(currentSpotNumber, floor, Config.SpotCapacity));
                    currentSpotNumber++;
                }
            }
        }

        public ParkingSpot? FindVehicleSpot(string regNumber)
        {
            if (string.IsNullOrWhiteSpace(regNumber))
                return null;

            return Spots.FirstOrDefault(s => s.Vehicles.Any(v =>
            v.RegNumber.Equals(regNumber.Trim(), StringComparison.OrdinalIgnoreCase)));
        }

        public bool IsVehicleParked(string regNumber)
        {
            return FindVehicleSpot(regNumber) != null;
        }

        public bool ParkVehicle(Vehicle vehicle, out string message)
        {
            if (vehicle == null)
            {
                message = "Ogiltigt fordon, (TOMT)";
                return false;
            }
            if (IsVehicleParked(vehicle.RegNumber))
            {
                message = $"Fordon med regnr {vehicle.RegNumber} är redan parkerad i garaget";
                return false;
            }

            int spotNeeded = (int)Math.Ceiling((double)vehicle.Size / Config.SpotCapacity);

            if (spotNeeded > 1)
            {
                return ParkMultiSpotVehicle(vehicle, spotNeeded, out message);
            }

            return ParkSingleSpotVehicle(vehicle, out message);
        }

        private bool ParkSingleSpotVehicle(Vehicle vehicle, out string message)
        {
            ParkingSpot? targetSpot = null;

            if (vehicle.Size < Config.SpotCapacity)
            {
                targetSpot = Spots.FirstOrDefault(s =>
                    !s.IsEmpty &&
                    s.Vehicles.Any(v => v.VehicleType == vehicle.VehicleType) &&
                    s.CanFit(vehicle));
            }

            targetSpot ??= Spots.FirstOrDefault(s => s.CanFit(vehicle));

            if (targetSpot == null)
            {
                message = $"Det finns inga lediga platser för {vehicle.VehicleType}{vehicle.RegNumber}";
                return false;
            }

            targetSpot.Park(vehicle);
            message = $"{vehicle.VehicleType} {vehicle.RegNumber} parkerades på parkerings rutan :{targetSpot.SpotNumber} (Plan:{targetSpot.FloorNumber}";
            return true;
        }

        private bool ParkMultiSpotVehicle(Vehicle vehicle, int spotNeeded, out string message)
        {
            var floorGroups = Spots.GroupBy(s => s.FloorNumber).OrderBy(g => g.Key);

            foreach (var floorGroup in floorGroups)
            {
                var floorSpots = floorGroup.OrderBy(s => s.SpotNumber).ToList();

                for (int i = 0; i <= floorSpots.Count - spotNeeded; i++)
                {
                    var sequence = floorSpots.Skip(i).Take(spotNeeded).ToList();

                    bool allEmpty = sequence.All(s => s.IsEmpty);
                    bool isConsecutive = true;

                    for (int j = 0; j < sequence.Count - 1; j++)
                    {
                        if (sequence[j + 1].SpotNumber != sequence[j].SpotNumber + 1)
                        {
                            isConsecutive = false;
                            break;
                        }
                    }
                    if (allEmpty && isConsecutive)
                    {
                        foreach (var spot in sequence)
                        {
                            spot.Park(vehicle);
                        }

                        message = $"{vehicle.VehicleType} [{vehicle.RegNumber}] parkerades över rutorna {sequence.First().SpotNumber}–{sequence.Last().SpotNumber} (Plan {sequence.First().FloorNumber}).";
                        return true;
                    }
                }
            }
            message = $"Kunde inte hitta {spotNeeded} sammanhängande lediga rutor för {vehicle.VehicleType} [{vehicle.RegNumber}].";
            return false;
        }

        public record CheckoutReceipt(
        Vehicle Vehicle,
        TimeSpan Duration,
        decimal TotalCost,
        decimal HourlyRate,
        List<int> FreedSpots
        );

        public bool CheckoutVehicle(string regNumber, out CheckoutReceipt? receipt, out string message)
        {
            receipt = null;

            if (string.IsNullOrWhiteSpace(regNumber))
            {
                message = "Registreringsnummer kan inte vara tomt/null";
                return false;
            }

            var occupiedSpots = Spots.Where(s => s.Vehicles.Any(v =>
            v.RegNumber.Equals(regNumber.Trim(), StringComparison.OrdinalIgnoreCase))).ToList();

            if (occupiedSpots.Count == 0)
            {
                message = $"Fordon med regnr {regNumber.Trim().ToUpper()} kunde inte hittas i garaget";
                return false;
            }

            Vehicle? vehicleToRemove = null;
            List<int> freedSpotNumber = new();

            foreach (var spot in occupiedSpots)
            {
                if (spot.Remove(regNumber, out Vehicle? removed))
                {
                    vehicleToRemove ??= removed;
                    freedSpotNumber.Add(spot.SpotNumber);
                }
            }

            if(vehicleToRemove == null)
            {
                message = "Ett oväntat fel ! ( ParkingGarage.cs/MSG RAD 186";
                return false;
            }

            TimeSpan duration = DateTime.Now - vehicleToRemove.ParkedTime;

            decimal hourlyRate = Config.GetHourlyRate(vehicleToRemove.VehicleType);
            decimal totalCost = 0m;

            if(duration.TotalMinutes > Config.FreeMinutes)
            {
                int billedHours = (int)Math.Ceiling(duration.TotalHours);
                totalCost = billedHours * hourlyRate;
            }

            receipt = new CheckoutReceipt(vehicleToRemove, duration, totalCost, hourlyRate, freedSpotNumber);

            message = $"";
            return true;
        }
    }
}