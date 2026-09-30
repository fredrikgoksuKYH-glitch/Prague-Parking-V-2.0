using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Models
{
    public class Boat : Vehicle
    {
        public override int Size => 100;
        public override string VehicleType => "Bike";

        public Boat(string regNumber) : base(regNumber) { }
        public Boat(string regNumber, DateTime parkedTime) : base(regNumber, parkedTime) { }
    }
}
