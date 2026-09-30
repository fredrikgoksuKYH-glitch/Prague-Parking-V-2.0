using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PragueParking.Core.Models
{
    /*Grupperar och organiserar.*/
    [JsonDerivedType(typeof(Car), typeDiscriminator: "Car")]
    [JsonDerivedType(typeof(MC), typeDiscriminator: "MC")]
    [JsonDerivedType(typeof(Bike), typeDiscriminator: "Bike")]
    [JsonDerivedType(typeof(Bus), typeDiscriminator: "Bus")]
    /*Våran "Mall" denna kan vi endast "kopiera" det går inte att instansiera utan detta är våran
     "grund" för alla andra fordon (BIL.MC,BUS osv)*/
    public abstract class Vehicle
    {
        /*Alla gemmensamma variablar för Fordon. get,set på de variablar somm ska kunna ändras.
        Endast get på Size och VehicleType för det ska ges och inte kunna skrivas över.*/
        public string RegNumber { get; set; }
        public DateTime ParkedTime { get; set; }
        public abstract int Size { get; }
        public abstract string VehicleType { get; }

        /*Json-deserialsering som ska användas när det läser upp från FIL.*/
        [JsonConstructor]
        protected Vehicle(string regNumber, DateTime parkedTime)
        {
        /*En kontroll att inget fordon kan sparas utan regNumber.*/
        if(string.IsNullOrWhiteSpace(regNumber))
        {
                throw new ArgumentException("Registreringsnummer får inte vara tomt.", nameof(regNumber));
        }
        /*Valdierar att DateTime inte har blivit korrupt och är orimligt långt ifrån dagens datum.*/
        if ( parkedTime == default)
        { 
        parkedTime = DateTime.Now;
        }
        /*Validera parkeringstid
         Har adderat en minut så det inte ska bli ett precitions race med datorns CPU */
        if (parkedTime > DateTime.Now.AddMinutes(1))
        {
                throw new ArgumentException("Parkeringstiden kan inte vara i framtiden.", nameof(parkedTime));
        }
        if (parkedTime < new DateTime(2000, 1, 1))
        {
                throw new ArgumentException("Parkeringstiden är orimligt långt bakåt i tiden.", nameof(parkedTime));
        }
            RegNumber = regNumber.Trim().ToUpper();
            ParkedTime = parkedTime;
        }
        protected Vehicle(string regNumber) : this(regNumber, DateTime.Now)
        {
        }
    }
}
