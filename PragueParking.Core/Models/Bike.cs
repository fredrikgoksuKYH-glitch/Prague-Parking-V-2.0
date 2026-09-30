using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking.Core.Models
{
    public class Bike : Vehicle
    {
        public override int Size => 1;
        public override string VehicleType => "Bike";

        public Bike(string regNumber) : base(regNumber){ }
        public Bike(string regNumber, DateTime parkedTime) : base(regNumber, parkedTime) { }
    }
}
