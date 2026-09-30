using PragueParking.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;

namespace PragueParking.Core.Models
{
    public class ParkingSpot
    {
        /*Standardkapacitet är satt till 4 per "parkingsruta" */
        public const int DefaultCapacity = 4;
        public int SpotNumber { get; set; }
        public int FloorNumber { get; set; }
        /*Maxkapacitet per parkingsrutta ( Denna kommer användas senare för bus p-platser )*/
        public int MaxCapacity { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new ();
        /*Räknar ut hur mycket plats som upptas via LINQ.
        Om ett fordon är större än ruttan, så räknar vi det som MaxCapacity*/
        public int CurrentLoad => Vehicles.Sum(v => v.Size > MaxCapacity ? MaxCapacity : v.Size);
        /*Räknar ut hur många enheter som finns kvar att uttnytja på rutan*/
        public int AvailableCapacity => MaxCapacity - CurrentLoad;
        /*Returnerar true om listan med fordon är tom*/
        public bool IsEmpty => Vehicles.Count == 0;
        /*Returnerar true om listan är full/inte finns någon kapacitet kvar.*/
        public bool IsFull => AvailableCapacity <= 0;

        /*Talar om för Json att den här konstruktorn ska anropas vid inläsning.*/
        [JsonConstructor]
        /*Konstruktor = bygger upp rutan, "vehicles" är valfri ( där av nullable) ifall en ruta är tom vid inläsning*/
        public ParkingSpot(int spotNumber, int floorNumber, int maxCapacity = DefaultCapacity, List<Vehicle>? vehicles = null)
        {
            /*Valdering som säkerställer att rutnumret är positvit ( kan inte vara under 1 )*/
            if (spotNumber <= 0)
                throw new ArgumentException("Rutnummer måste vara större än 0.", nameof(spotNumber));
                /*Samma sak här för våningarna*/
            if (floorNumber <= 0)
                throw new ArgumentException("Våningsnummer måst vara större än 0", nameof(floorNumber));

            SpotNumber = spotNumber;
            FloorNumber = floorNumber;
            MaxCapacity = maxCapacity;
            /*OM "vehicles" är null behålls den tomma listan med egenskapen ovanför*/
            if (vehicles != null)
            {
                Vehicles = vehicles;
            }
        }

        /*Kontroll för fordon som ska parkera på en ruta(utan att översrkida reglerna)*/
        public bool CanFit(Vehicle vehicle)
        {
        /*OM fordon är null så nekas den*/
            if (vehicle == null)
                return false;

            /*OM fordonsobjektet är större än (4) t.ex buss som är size 16. krävs det att ruttan är tom*/
            if (vehicle.Size > MaxCapacity)
            {
                return IsEmpty;
            }
            /*OM fordon storlek av 4 eller mindre ( bil,mc och cyckel) :
             Så kontrollerar vi om det finns tillräckligt med ledigt utrymme kvar i ruttan just nu
             för ett mindre fordon av 4 eller lägre*/
            return AvailableCapacity >= vehicle.Size;
        }

        /*Utför parkeringen*/
        public bool Park(Vehicle vehicle)
        {
            /*OM INTE fordonet kan fåplats enkelt CanFit så nekas försöket*/
            if (!CanFit(vehicle))
                return false;
            /*Raka motsatsen, om den har blivit godkänd så adderas fordonet och returnerar true*/
            Vehicles.Add(vehicle);
            return true;
        }
        /*Söker upp fordon basear på regnummer*/
        public bool Remove(string regNumber, out Vehicle? removedVehicle)
        {
            /*Här har vi fina grejer :) Genom LINQ så kan vi hitta fordonet i listan OAVSETT om det
             är stora/småbokstäver och mellanslag ( dock har vi redan gjort så allt ska vara stort)*/
            removedVehicle = Vehicles.FirstOrDefault(v => v.RegNumber.Equals(regNumber.Trim(),
            StringComparison.OrdinalIgnoreCase));
            /*OM ett fordon hittades med regnummer*/
            if (removedVehicle != null)
            {
            Vehicles.Remove(removedVehicle);
            return true;
            }
            /*OM inget fordon hittades.*/ 
            return false;
            }
        }
    }
